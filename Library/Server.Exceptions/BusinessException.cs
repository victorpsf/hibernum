namespace Server.Exceptions;

public class BusinessException: Exception
{
    public BusinessExceptionEnum Stack { get; set; }

    public BusinessException(BusinessExceptionEnum stack, string? message, Exception? innerException) : base(message,
        innerException)
    { this.Stack = stack; }
    
    public BusinessException(BusinessExceptionEnum stack, string? message) : this(stack, message, null)
    { }
    
    public BusinessException(BusinessExceptionEnum stack) : this(stack, null)
    { }
}