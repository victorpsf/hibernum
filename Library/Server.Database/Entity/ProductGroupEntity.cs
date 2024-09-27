using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace Server.Database.Entity;

[Table(name: "productgroup", Schema = "public")]
public class ProductGroupEntity
{
    [Column(name: "id")]
    public long Id { get; set; }

    [Column(name: "name")]
    public string Name { get; set; }
   
    [Column(name: "created_at")]
    public DateTime CreatedAt { get; set; }
    
    [Column(name: "deleted_at")]
    public DateTime? DeletedAt { get; set; }

    public Collection<ProductEntity> Products { get; set; } = new();

    public void Update(ProductGroupEntity? entity)
    { this.Name = entity?.Name ?? string.Empty; }
}