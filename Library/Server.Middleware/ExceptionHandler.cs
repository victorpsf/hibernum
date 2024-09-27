using System.Net;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Serilog;
using Server.Exceptions;

namespace Server.Middleware;

public class ExceptionHandler
{
    private readonly RequestDelegate next;
    
    public ExceptionHandler(RequestDelegate next)
    {
        this.next = next;
    }
    
    public async Task Invoke(HttpContext context)
    {
        try
        { await this.next(context); }
        catch (Exception ex)
        { await HandleException(context, ex); }
    }
    
    public static async Task HandleException(
        HttpContext context,
        Exception exception
    ) {
        var result = new Dictionary<string, object>();
        if (!string.IsNullOrEmpty(exception.Message))
            Log.Error(exception.Message);

        context.Response.StatusCode = (int) HttpStatusCode.InternalServerError;

#if DEBUG
        result.Add("Message", exception.Message ?? string.Empty);
#endif
        if (exception.GetType() == typeof(ServerValidationException))
        {
            context.Response.StatusCode = (int) HttpStatusCode.BadRequest;
            result.Add("validation", ((ServerValidationException)exception).Model);
        }

        else if (exception.GetType() == typeof(UnauthorizedAccessException))
            context.Response.StatusCode = (int) HttpStatusCode.Unauthorized;
        
        await context.Response.WriteAsJsonAsync(result);
    }
}
