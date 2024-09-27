using System.ComponentModel.DataAnnotations;

namespace Server.Exceptions;

public class ServerValidationException: Exception
{
    public Dictionary<string, object> Model { get; private set; }

    public ServerValidationException(Dictionary<string, object> model, string? message, Exception? innerException) : base(message, innerException)
    { this.Model = model; }
    public ServerValidationException(Dictionary<string, object> model, string? message): this(model, message, null) {}
    public ServerValidationException(Dictionary<string, object> model): this(model, null) {}
}