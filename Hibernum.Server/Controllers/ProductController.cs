using System.ComponentModel.DataAnnotations;
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
public class ProductController: ControllerBase
{
    private ProductService Product;
    private LoggedUser User { get; set; }

    public ProductController(
        ProductService productService, 
        LoggedUser user
    )
    {
        this.Product = productService;
        this.User = user;
    }
    
    [HttpGet]
    public IActionResult Index(
        [FromQuery] long? id,
        [FromQuery] string? name,
        [FromQuery] string? size,
        [FromQuery] long? group
    )
    {
        var products = this.Product.Find(new()
        {
            Id = id,
            Name = name,
            Size = size,
            Group = group
        });
        
        this.Product.LoadRelation(
            new ()
            {
                ProductGroup = true,
                ProductType = true,
                ProductDescription = true,
                File = true
            },
            products
        );
        
        return Ok(products.Select(a => ProductDTO.By(a)));
    }

    [HttpPost]
    public IActionResult Index(
        [FromBody] ProductDim product
    )
    {
        ValidateModel<ProductDim>.Validate(product);

        return Ok(
            ProductDTO.By(
                this.Product.Save(product)
            )
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
        
        return Ok(
            ProductDTO.By(
                this.Product.Remove(id ?? 0)
            )
        );
    }
}