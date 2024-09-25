using Microsoft.AspNetCore.Http;
using Server.Database.Entity;
using Server.Models.Security;
using Server.Services;
using Server.Services.Database;
using Server.Services.Database.Rules;

namespace Server.Middleware;

public class LoggedUser
{
    private readonly AuthenticationDbService service;
    private readonly JwtSecurity jwt;
    private readonly HttpContext? httpContext;
    private AuthEntity? identifier;
    private CompanyEntity? company;
    private ClaimIdentifier? claim;
    
    private void HandleException() => throw new UnauthorizedAccessException("Unautorized");

    private string Token
    { get => this.httpContext?.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last<string>() ?? string.Empty; }
    
    public ClaimIdentifier Claim
    {
        get
        {
            if (this.httpContext is null) 
                this.HandleException();

            if (this.claim is null)
                this.claim = this.jwt.Read(this.Token);

            return this.claim ?? new();
        }
    }

    public AuthEntity Identifier
    {
        get
        {
            if (this.identifier is null)
                this.identifier = this.service.Find(new FindUserRule() { Id = this.Claim.Id });
            
            if (this.identifier is null)
                this.HandleException();

            return this.identifier ?? new();
        }
    }
    
    public CompanyEntity Company
    {
        get
        {
            if (this.company is null)
                this.company = this.service.Find(new FindCompanyRule() { Id = this.Claim.Company });
            
            if (this.company is null)
                this.HandleException();

            return this.company ?? new();
        }
    }
    
    public LoggedUser(
        IHttpContextAccessor httpContextAcessor,
        AuthenticationDbService service,
        JwtSecurity jwtSecurity
    )
    {
        this.service = service;
        this.jwt = jwtSecurity;
        this.httpContext = httpContextAcessor.HttpContext;
    }
}