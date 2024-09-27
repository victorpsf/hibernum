using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using Server.Database.Entity;
using Server.Validation;

namespace Server.Dtos;

public class ProductTypeDTO
{
    public long? Id { get; set; }
    [StringValidation(max = 1000, required = true, ErrorMessage = "value is required or lower to 1000 characters")]
    public string Value { get; set; } = string.Empty;

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

    public static ProductTypeDTO? ByEntity(ProductTypeEntity? entity)
    {
        if (entity is null)
            return null;

        return new()
        {
            Id = entity.Id,
            Value = entity.Value
        };
    }

    public static List<ProductTypeDTO?> ByEntity(Collection<ProductTypeEntity> entities)
        => entities.Select(a => ProductTypeDTO.ByEntity(a))
            .ToList();
}