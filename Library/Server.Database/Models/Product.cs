using System.Text.Json.Serialization;

namespace Server.Database.Models;

public class Product
{
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Size { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }
    
    [JsonIgnore]
    public long? GroupId { get; set; }
    [JsonIgnore]
    public long? FileId { get; set; }

    public ProductGroup? Group { get; set; }
    public File? File { get; set; }
    public List<ProductType> Types { get; set; } = new();
    public List<ProductDescription> Descriptions { get; set; } = new();
}