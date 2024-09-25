using System.ComponentModel.DataAnnotations;

namespace Server.Exceptions;

public class ServerValidationException: Exception
{
    public List<ValidationResult> Results { get; private set; }

    public ServerValidationException(List<ValidationResult> results, string? message, Exception? innerException): base(message, innerException)
        => this.Results = results;
    public ServerValidationException(List<ValidationResult> results, string? message): this(results, message, null) {}
    public ServerValidationException(List<ValidationResult> results): this(results, null) {}
}