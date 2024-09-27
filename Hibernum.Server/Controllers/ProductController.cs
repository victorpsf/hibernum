using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Dtos;
using Server.Middleware;
using Server.Services.Database;
using Server.Services.Database.Rules;
using Server.Validation;

namespace Hibernum.Server.Controllers;

[Authorize]
public class ProductController: ControllerBase
{
    private HibernumDbService Service { get; set; }
    private LoggedUser User { get; set; }
    
    public ProductController(HibernumDbService service, LoggedUser user)
    {
        this.Service = service;
        this.User = user;
    }

    [HttpGet]
    public IActionResult Index(
        [FromQuery] long? id,
        [FromQuery] string? name,
        [FromQuery] string? size
    )
    {
        return Ok(
            this.Service.Find(new FindProductRule()
            {
                Id = id,
                Name = name,
                Size = size
            })
                .Select(a => ProductDTO.By(a))
                .ToList()
        );
    }

    [HttpPost]
    public IActionResult Index(
        [FromBody] ProductDTO product
    )
    {
        ValidateModel<ProductDTO>.Validate(product, "group");
        return Ok(
            ProductDTO.By(
                this.Service.Save(product)
            )
        );
    }
}