using Hibernum.Server.Services;
using Hibernum.Server.Services.Rules;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Dims;
using Server.Dtos;
using Server.Exceptions;
using Server.Middleware;
using Server.Validation;

namespace Hibernum.Server.Controllers;

[Authorize]
public class ProductTypeController: ControllerBase
{
    private ProductTypeService _productType;
    private LoggedUser _user;


    public ProductTypeController(
        ProductTypeService productType,
        LoggedUser user
    )
    {
        this._productType = productType;
        this._user = user;
    }

    [HttpGet]
    public IActionResult Index(
        [FromQuery] long? id,
        [FromQuery] long? productId,
        [FromQuery] string? value
    )
    {
        var results = this._productType.Find(new FindProductTypeRule()
        {
            Id = id,
            Product = productId,
            Value = value
        });
        
        this._productType.LoadRelation(new () { Product = true },results);
        return Ok(
            results.Select(a => ProductTypeDTO.ByEntity(a))
        );
    }

    [HttpPost]
    public IActionResult Index(
        [FromBody] ProductTypeDim productType
    ) {
        ValidateModel<ProductTypeDim>.Validate(productType);
        return Ok(
            ProductTypeDTO.ByEntity(this._productType.Save(productType))
        );
    }

    [HttpDelete]
    public IActionResult Index(
        [FromQuery] long? id
    )
    {
        if (id is null)
        {
            Dictionary<string, object> erroMessage = new();
            erroMessage.Add("Id", "required");
            throw new ServerValidationException(erroMessage);
        }
        
        var result = this._productType.Remove(id ?? 0);
        return Ok(
            ProductTypeDTO.ByEntity(result)
        );
    }
}