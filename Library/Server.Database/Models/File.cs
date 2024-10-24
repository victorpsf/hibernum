namespace Server.Database.Models;

public class File
{
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Mime { get; set; } = string.Empty;
    public long Size { get; set; }
    public DateTime LastModified { get; set; }
    public string Path { get; set; } = String.Empty;
}