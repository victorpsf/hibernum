using Authentication.Server.Controllers.Rules;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Middleware;
using Server.Models.Dto;
using Server.Models.Security;
using Server.Security;
using Server.Services;
using Server.Services.Database;
using Server.Services.Database.Rules;
using Server.Validation;

namespace Authentication.Server.Controllers;

public class AuthenticationController: ControllerBase
{
    private readonly JwtSecurity Jwt;
    private readonly AuthenticationDbService Service;
    private readonly LoggedUser User;

    public AuthenticationController(JwtSecurity jwtSecurity, AuthenticationDbService service, LoggedUser loggedUser)
    {
        this.Jwt = jwtSecurity;
        this.Service = service;
        this.User = loggedUser;
    }

    [HttpPost]
    [AllowAnonymous]
    public IActionResult Index([FromBody] AuthenticationRule input)
    {
        ValidateModel.Validate(input);
        if (string.IsNullOrEmpty(input.Name) && string.IsNullOrEmpty(input.Email))
            return BadRequest(new { Error = "'Name' ou 'Email' deve ser informado" });

        if (!string.IsNullOrEmpty(input.Name))
            input.Name = ServerHash.Create(ServerHashAlgorithm.SHA512).Update(input.Name).ToBase64();

        var result = this.Service.Find(new FindUserRule()
        {
            Name = input.Name,
            Email = input.Email
        });

        if (result is null)
            return NotFound(new { Error = "Usuário ou senha inválido" });
        
        if (!ServerPbkdf2.GetInstance().Verify(result.Passphrase, ServerHash.Create(ServerHashAlgorithm.SHA512).Update(input.Password ?? string.Empty).ToBase64(), Pbkdf2HashDerivation.HMACSHA512))
            return NotFound(new { Error = "Usuário ou senha inválido" });
        
        return Ok(this.Jwt.Write(new() { Id = result.Id }));
    }

    [HttpGet]
    [Authorize]
    public IActionResult Company()
        => Ok(this.Service.ListCompany(this.User.Identifier).Select(a => new PublicCompanyDto(){ Id = a.Id, Name = a.Name }));

    [HttpPost]
    [Authorize]
    public IActionResult Company([FromQuery] long id)
    {
        var results = this.Service.ListCompany(this.User.Identifier)
            .Where(a => a.Id == id)
            .ToList();
        
        if (!results.Any())
            return NotFound(new { Error = "Não possui acesso a está empresa" });
        
        return Ok(
            this.Jwt.Write(new ()
            {
                Id = this.User.Identifier.Id,
                Company = results.First().Id
            })
        );
    }
}