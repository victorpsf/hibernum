using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using Server.Database.Entity;
using Server.Validation;

namespace Server.Dtos;

public class ProductGroupDTO
{
    public long? Id { get; set; }

    [StringValidation(max = 1000, required = true, ErrorMessage = "name is required or lower 1000 characters")]
    public string Name { get; set; } = string.Empty;

    public ProductGroupEntity ToEntity()
    {
        var entity = new ProductGroupEntity();
        this.Copy(entity);
        return entity;
    }

    public void Copy(ProductGroupEntity entity)
    {
        if (this.Id is not null)
            entity.Id = this.Id ?? default;
        entity.Name = this.Name;
    }
    
    public static ProductGroupDTO? ByEntity(ProductGroupEntity? entity)
    {
        if (entity is null)
            return null;

        return new()
        {
            Id = entity.Id,
            Name = entity.Name
        };
    }

    public static List<ProductGroupDTO?> ByEntity(Collection<ProductGroupEntity> entities)
        => entities.Select(a => ProductGroupDTO.ByEntity(a))
            .ToList();
}