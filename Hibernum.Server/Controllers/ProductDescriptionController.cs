using Hibernum.Server.Services;
using Hibernum.Server.Services.Relations;
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
public class ProductDescriptionController: ControllerBase
{
    private ProductDescriptionService _service;
    private LoggedUser _user;

    public ProductDescriptionController(
        ProductDescriptionService service,
        LoggedUser user
    )
    {
        this._service = service;
        this._user = user;
    }

    [HttpGet]
    public IActionResult Index(
        [FromQuery] long? id,
        [FromQuery] long? productId,
        [FromQuery] string? value
    )
    {
        var descriptions = this._service.Find(new FindProductDescriptionRule()
        {
            Id = id,
            Value = value,
            Product = productId
        });
        
        this._service.LoadRelation(new ProductDescriptionRelation() { Product = true }, descriptions);
        return Ok(
            descriptions.Select(a => ProductDescriptionDTO.ByEntity(a))
        );
    }

    [HttpPost]
    public IActionResult Index(
        [FromBody] ProductDescriptionDim dim
    )
    {
        ValidateModel<ProductDescriptionDim>.Validate(dim);
        return Ok(
            ProductDescriptionDTO.ByEntity(this._service.Save(dim))
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
        
        var result = this._service.Remove(id ?? 0);
        return Ok(
            ProductDescriptionDTO.ByEntity(result)
        );
    }
}