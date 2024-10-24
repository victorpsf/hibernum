using System.ComponentModel.DataAnnotations;
using Server.Validation;

namespace Server.Dims;

public class ProductTypeDim
{
    public long? Id { get; set; }

    [Required(ErrorMessage = "product not imported")]
    public long? Product { get; set; }

    [StringValidation(max = 1000, ErrorMessage = "")]
    public string Value { get; set; } = string.Empty;
}