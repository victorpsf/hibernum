using System.ComponentModel.DataAnnotations;
using Server.Exceptions;

namespace Server.Validation;

public class ValidateModel<T> where T : class, new()
{
    private T Value;
    private List<string> ObjectFields;

    private bool Failed
    {
        get { return this.fieldsValidation.Keys.Any(); }
    }
    private Dictionary<string, object> fieldsValidation = new();

    private ValidateModel(T value, List<string> objectFields)
    {
        this.Value = value;
        this.ObjectFields = objectFields;
    }

    private void validateProperty(string name, object? value)
    {
        var results = new List<ValidationResult>();
        var context = new ValidationContext(this.Value) { MemberName = name };

        bool valid = Validator.TryValidateProperty(value, context, results);
        
        if (!valid)
            this.fieldsValidation.Add(name, results.FirstOrDefault()?.ErrorMessage ?? string.Empty);
    }

    private void validateList(string name, object? value)
    {
        if (value is null)
            return;

        var list = (value as IEnumerable<object>).Cast<object>().ToList();

        var results = new List<Dictionary<string, object>>();
        foreach (var v in list)
            try
            { ValidateModel<object>.Validate(v); }

            catch (ServerValidationException ex)
            { results.Add(ex.Model); }

        if (results.Any())
            this.fieldsValidation.Add(name, results);
    }
    
    private void validate()
    {
        var fields = this.Value.GetType().GetProperties();

        foreach (var field in fields)
        {
            var value = field.GetValue(this.Value);
            var type = field.PropertyType;
            var a = field.MemberType;

            if (ReflecionHelper.IsList(type)) 
            {
                if (value is not null) this.validateList(field.Name, value);
                continue;
            }

            if (this.ObjectFields.Select(a => a.ToUpper()).Where(a => a == field.Name.ToUpper()).Any())
            {
                if (value is not null)
                    try
                    { ValidateModel<object>.Validate(value); }

                    catch (ServerValidationException ex)
                    { if (ex.Model.Keys.Any()) this.fieldsValidation.Add(field.Name, ex.Model); }
                continue;
            }
            
            this.validateProperty(field.Name, value);
        }

        if (this.Failed)
            throw new ServerValidationException(this.fieldsValidation);
    }

    public static void Validate(T value, params string[] objectFields)
        => (new ValidateModel<T>(value, objectFields.ToList())).validate();

    public static void Validate(T value)
        => (new ValidateModel<T>(value, new())).validate();
}