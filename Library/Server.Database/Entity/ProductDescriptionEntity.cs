using System.ComponentModel.DataAnnotations.Schema;

namespace Server.Database.Entity;

[Table(name: "productdescription", Schema = "public")]
public class ProductDescriptionEntity
{
    [Column(name: "id")]
    public long Id { get; set; }
    
    [Column(name: "value")]
    public string Value { get; set; } = string.Empty;

    [Column(name: "created_at")]
    public DateTime CreatedAt { get; set; }
    
    [Column(name: "deleted_at")]
    public DateTime? DeletedAt { get; set; }
    
    public ProductEntity Product { get; set; }
}