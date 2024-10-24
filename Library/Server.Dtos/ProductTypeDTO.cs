using Server.Database.Entity;
using Server.Database.Models;
using Server.Validation;
using System.Text.Json.Serialization;

namespace Server.Dtos;

public class ProductTypeDTO
{
    public long? Id { get; set; }
    [StringValidation(max = 1000, required = true, ErrorMessage = "value is required or lower to 1000 characters")]
    public string Value { get; set; } = string.Empty;
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public ProductDTO? Product { get; set; }

    public ProductTypeEntity ToEntity()
    {
        var entity = new ProductTypeEntity();
        this.Copy(entity);
        return entity;
    }

    public void Copy(ProductTypeEntity entity)
    {
        if (this.Id is not null)
            entity.Id = this.Id ?? default;
        entity.Value = this.Value;
    }

    public static ProductTypeDTO? ByEntity(ProductType? entity)
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

    public static List<ProductTypeDTO?> ByEntity(List<ProductType> entities)
        => entities.Select(a => ByEntity(a))
            .ToList();
}