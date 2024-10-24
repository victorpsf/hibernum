namespace Server.Dtos;

public class FileDataDTO
{
    public string Encode { get; set; } = string.Empty;
    public string Data { get; set; } = string.Empty;
    public long Size { get; set; }

    public static FileDataDTO Create(
        FileDataEncode encoded,
        string data,
        long size
    )
    {
        return new()
        {
            Encode = encoded.ToString(),
            Data = data,
            Size = size
        };
    }
}