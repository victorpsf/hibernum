using System.Text.Json.Serialization;
using Server.Database.Models;

namespace Server.Dtos;

public class ProductGroupDTO
{
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public List<ProductDTO>? Products { get; set; }

    public static ProductGroupDTO? ByEntity(ProductGroup? entity)
    {
        if (entity is null)
            return null;

        return new()
        {
            Id = entity.Id,
            Name = entity.Name,
            Products = entity.Products.Any()? entity.Products.Select(a => ProductDTO.By(a)).ToList(): null
        };
    }

    public static List<ProductGroupDTO?> ByEntity(List<ProductGroup> entities)
        => entities.Select(a => ProductGroupDTO.ByEntity(a))
            .ToList();
}