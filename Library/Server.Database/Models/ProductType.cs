using System.Text.Json.Serialization;

namespace Server.Database.Models;

public class ProductType
{
    public long Id { get; set; }
    public string Value { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }

    [JsonIgnore]
    public long ProductId { get; set; }

    public Product? Product { get; set; }
}