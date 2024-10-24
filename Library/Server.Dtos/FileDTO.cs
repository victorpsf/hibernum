using System.Text.Json.Serialization;
using Server.Library;
using File = Server.Database.Models.File;

namespace Server.Dtos;

public class FileDTO
{
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Mime { get; set; } = string.Empty;
    public DateTime LastModified { get; set; }
    public FileDataDTO? Data { get; set; }
    [JsonIgnore]
    public string Path { get; set; } = String.Empty;

    public static FileDTO? ByEntity(File? file)
    {
        if (file is null)
            return null;
        
        var dto = new FileDTO()
        {
            Id = file.Id,
            Name = file.Name,
            Mime = file.Mime,
            LastModified = file.LastModified,
            Path = file.Path
        };
        
        var manager = FileManager.Create(file.Path);
        var bytes = manager.ReadFile();
        var binary = Binary.FromBytes(bytes);
            
        dto.Data = FileDataDTO.Create(
            FileDataEncode.BASE64,
            binary.ToBase64(),
            bytes.Length
        );

        return dto;
    }
}