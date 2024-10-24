using Server.Database.Models;
using Server.Validation;
using System.Text.Json.Serialization;

namespace Server.Dtos;

public class ProductDescriptionDTO
{
    public long? Id { get; set; }

    [StringValidation(required = true, ErrorMessage = "value is required")]
    public string Value { get; set; } = string.Empty;
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public ProductDTO? Product { get; set; }

    public static ProductDescriptionDTO? ByEntity(ProductDescription? entity)
    {
        if (entity is null)
            return null;

        return new()
        {
            Id = entity.Id,
            Value = entity.Value,
            Product = ProductDTO.By(entity.Product)
        };
    }

    public static List<ProductDescriptionDTO?> ByEntity(List<ProductDescription> entities)
        => entities.Select(a => ByEntity(a))
            .ToList();
}