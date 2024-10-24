namespace Server.Dims;

using Server.Validation;

public class FileDim
{
    public long? Id { get; set; }
    // [StringValidation(max = 1000, required = true, ErrorMessage = "name is required or upper to 1000 characters")]
    public string? Name { get; set; }
    // [StringValidation(max = 1000, required = true, ErrorMessage = "mime is required or upper to 500 characters")]
    public string? Mime { get; set; }
    public long? Size { get; set; }
    public List<byte> Bytes { get; set; } = new();
    public DateTime? LastModified { get; set; }
}