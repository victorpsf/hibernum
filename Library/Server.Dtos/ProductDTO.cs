using System.Collections.ObjectModel;
using Server.Database.Entity;
using Server.Validation;

namespace Server.Dtos;

public class ProductDTO
{
    public long? Id { get; set; }
    [StringValidation(max = 1000, required = true, ErrorMessage = "name is required or upper to 1000 characters")]
    public string Name { get; set; } = string.Empty;
    [StringValidation(max = 25, required = true, ErrorMessage = "name is required or upper to 1000 characters")]
    public string Size { get; set; } = string.Empty;
    public ProductGroupDTO? Group { get; set; }
    public List<ProductTypeDTO> Types { get; set; } = new();
    public List<ProductDescriptionDTO> Descriptions { get; set; } = new();

    public ProductEntity ToEntity()
    {
        var entity = new ProductEntity();
        this.Copy(entity);
        return entity;
    }
    
    public void Copy(ProductEntity product)
    {
        if (this.Id is not null)
            product.Id = this.Id ?? default;
        product.Name = this.Name;
        product.Size = this.Size;
        
        if (this.Group is not null)
            product.Group = this.Group.ToEntity();
        
        product.Types = new (this.Types.Select(a => a.ToEntity()).ToList());
        product.Descriptions = new(this.Descriptions.Select(a => a.ToEntity()).ToList());
    }

    public static ProductDTO? By(ProductEntity? entity)
    {
        if (entity is null)
            return null;
        
        return new()
        {
            Id = entity.Id,
            Name = entity.Name,
            Size = entity.Size,
            Group = ProductGroupDTO.ByEntity(entity.Group),
            Types = ProductTypeDTO.ByEntity(entity.Types).Where(a => a is not null).ToList() as List<ProductTypeDTO>,
            Descriptions = ProductDescriptionDTO.ByEntity(entity.Descriptions).Where(a => a is not null).ToList() as List<ProductDescriptionDTO>
        };
    }
}