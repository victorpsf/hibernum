using Server.Validation;

namespace Server.Dims;

public class ProductGroupDim
{
    public long? Id { get; set; }
    [StringValidation(required = true, max = 1000, ErrorMessage = "'NAME' IS REQUIRED OR SIZE IS UPPER TO 1000 CHARACTERS")]
    public string? Name { get; set; }
}