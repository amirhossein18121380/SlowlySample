using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Web.Middleware;

public class IpFilteringMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<IpFilteringMiddleware> _logger;

    public IpFilteringMiddleware(RequestDelegate next, ILogger<IpFilteringMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var remoteIp = context.Connection.RemoteIpAddress;
        _logger.LogInformation($"Request from Remote IP address: {remoteIp}");

        await _next(context);
    }
}
