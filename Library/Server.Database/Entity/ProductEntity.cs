using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace Server.Database.Entity;

[Table(name: "product", Schema = "public")]
public class ProductEntity
{
    [Column(name: "id")]
    public long Id { get; set; }

    [Column(name: "name")]
    public string Name { get; set; } = string.Empty;

    [Column(name: "size")]
    public string Size { get; set; } = string.Empty;
   
    [Column(name: "created_at")]
    public DateTime CreatedAt { get; set; }
    
    [Column(name: "deleted_at")]
    public DateTime? DeletedAt { get; set; }
    
    public ProductGroupEntity? Group { get; set; }
    public Collection<ProductTypeEntity> Types { get; set; } = new();
    public Collection<ProductDescriptionEntity> Descriptions { get; set; } = new();

    public void Update(ProductEntity entity)
    {
        this.Name = entity.Name;
        this.Size = entity.Size;

        if (entity.Group == null)
            this.Group = null;
        else if (this.Group is null)
            this.Group = entity.Group;
        else
            this.Group?.Update(entity.Group);
    }
}