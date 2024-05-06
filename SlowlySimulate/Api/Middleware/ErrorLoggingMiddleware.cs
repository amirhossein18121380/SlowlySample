using Newtonsoft.Json;
using System.Net;

namespace SlowlySimulate.Api.Middleware;



public class ErrorLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ErrorLoggingMiddleware> _logger;
    public ErrorLoggingMiddleware(RequestDelegate next, ILogger<ErrorLoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (AccessViolationException avEx)
        {
            _logger.LogError($"A new violation exception has been thrown: {avEx}");
            await HandleExceptionAsync(context, avEx);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Something went wrong: {ex}");
            await HandleExceptionAsync(context, ex);
        }
        finally
        {
            _logger.LogInformation(
                "Request {method} {url} => {statusCode}",
                context.Request?.Method,
                context.Request?.Path.Value,
                context.Response?.StatusCode);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {

        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        context.Response.ContentType = "application/json";

        var message = exception switch
        {
            AccessViolationException => "Access violation error from the custom middleware",
            _ => "Internal Server Error from the custom middleware."
        };

        await context.Response.WriteAsync(JsonConvert.SerializeObject(new
        {
            StatusCode = context.Response.StatusCode,
            Message = message
        }));
    }
}

public static class ErrorLoggingMiddlewareExtensions
{
    public static IApplicationBuilder UseErrorLoggingMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<ErrorLoggingMiddleware>();
    }
}