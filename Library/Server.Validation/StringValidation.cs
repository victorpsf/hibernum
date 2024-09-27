using System.ComponentModel.DataAnnotations;

namespace Server.Validation;

public class StringValidation: ValidationAttribute
{
    public int min { get; set; }
    public int max { get; set; }
    public bool required { get; set; }

    public override bool IsValid(object? value)
    {
        if (value is null)
            return !this.required;

        try
        {
            var data = value?.ToString() ?? string.Empty;

            if (string.IsNullOrEmpty(data))
                return !this.required;

            if (this.min > 0 && data.Length <= this.min)
                return false;
            
            if (this.max > 0 && this.max <= data.Length)
                return false;

            return true;
        }

        catch
        { return false; }
    }
}
