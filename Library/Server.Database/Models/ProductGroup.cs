namespace Server.Database.Models;

public class ProductGroup
{
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }

    public List<Product> Products { get; set; } = new();
}