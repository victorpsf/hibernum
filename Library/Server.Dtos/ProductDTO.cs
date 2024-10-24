using System.Text.Json.Serialization;
using Server.Database.Models;

namespace Server.Dtos;

public class ProductDTO
{
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Size { get; set; } = string.Empty;
    
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public ProductGroupDTO? Group { get; set; }
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public FileDTO? File { get; set; }
    
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public List<ProductTypeDTO>? Types { get; set; } = new();
    
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public List<ProductDescriptionDTO>? Descriptions { get; set; } = new();

    public static ProductDTO? By(Product? entity)
    {
        if (entity is null)
            return null;
        
        return new()
        {
            Id = entity.Id,
            Name = entity.Name,
            Size = entity.Size,
            Group = ProductGroupDTO.ByEntity(entity.Group),
            Types = entity.Types.Any() ? ProductTypeDTO.ByEntity(entity.Types).Where(a => a is not null).ToList() as List<ProductTypeDTO>: null,
            Descriptions = entity.Descriptions.Any() ? ProductDescriptionDTO.ByEntity(entity.Descriptions).Where(a => a is not null).ToList() as List<ProductDescriptionDTO>: null,
            File = FileDTO.ByEntity(entity.File)
        };
    }
}