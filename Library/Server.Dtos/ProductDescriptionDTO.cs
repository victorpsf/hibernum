using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using Server.Database.Entity;
using Server.Validation;

namespace Server.Dtos;

public class ProductDescriptionDTO
{
    public long? Id { get; set; }

    [StringValidation(required = true, ErrorMessage = "value is required")]
    public string Value { get; set; } = string.Empty;

    public ProductDescriptionEntity ToEntity()
    {
        var entity = new ProductDescriptionEntity();
        this.Copy(entity);
        return entity;
    }

    public void Copy(ProductDescriptionEntity entity)
    {
        if (this.Id is not null)
            entity.Id = this.Id ?? default;
        entity.Value = this.Value;
    }

    public static ProductDescriptionDTO? ByEntity(ProductDescriptionEntity? entity)
    {
        if (entity is null)
            return null;

        return new()
        {
            Id = entity.Id,
            Value = entity.Value
        };
    }

    public static List<ProductDescriptionDTO?> ByEntity(Collection<ProductDescriptionEntity> entities)
        => entities.Select(a => ProductDescriptionDTO.ByEntity(a))
            .ToList();
}