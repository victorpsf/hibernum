using System.ComponentModel.DataAnnotations;
using Server.Exceptions;

namespace Server.Validation;

public class ValidateModel
{
    public static void Validate<T>(T value) where T : class, new()
    {
        var ctx = new ValidationContext(value, null, null);
        var results = new List<ValidationResult>();
        var valid = Validator.TryValidateObject(value, ctx, results, true);

        if (!valid)
            throw new ServerValidationException(results);
    }
}