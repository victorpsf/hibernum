namespace Server.Dims;

using Server.Validation;

public class ProductDim
{
    public long? Id { get; set; }
    [StringValidation(max = 1000, required = true, ErrorMessage = "name is required or upper to 1000 characters")]
    public string? Name { get; set; }
    [StringValidation(max = 25, required = true, ErrorMessage = "size is required or upper to 25 characters")]
    public string? Size { get; set; }
    public long? Group { get; set; }
    public long? File { get; set; }
}
