using Hibernum.Server.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Dims;
using Server.Dtos;
using Server.Exceptions;
using Server.Middleware;
using Server.Validation;

namespace Hibernum.Server.Controllers;

[Authorize]
public class ProductGroupController: ControllerBase
{
    private ProductGroupService ProductGroup;
 
    private LoggedUser User { get; set; }
    
    public ProductGroupController(
        ProductGroupService productGroupService,
        LoggedUser user
    )
    {
        this.ProductGroup = productGroupService;
        this.User = user;
    }
    
    [HttpGet]
    public IActionResult Index(
        [FromQuery] long? id,
        [FromQuery] string? name
    )
    {
        var results = this.ProductGroup.Find(new()
        {
            Id = id,
            Name = name
        });
        
        this.ProductGroup.LoadRelation(new ()
            {
                Product = true
            },
            results
        );
    
        return Ok(results.Select(a => ProductGroupDTO.ByEntity(a)));
    }

    [HttpPost]
    public IActionResult Index(
        [FromBody] ProductGroupDim dim
    ) {
        ValidateModel<ProductGroupDim>.Validate(dim);
        return Ok(
            ProductGroupDTO.ByEntity(this.ProductGroup.Save(dim))
        );
    }
    
    [HttpDelete]
    public IActionResult Index(
        [FromQuery] long? id
    ) {
        if (id is null)
        {
            Dictionary<string, object> erroMessage = new();
            erroMessage.Add("Id", "required");
            throw new ServerValidationException(erroMessage);
        }
        
        return Ok(
            ProductGroupDTO.ByEntity(this.ProductGroup.Remove(id ?? 0))
        );
    }
}