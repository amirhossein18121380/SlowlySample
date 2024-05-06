namespace SlowlySimulate.Api.Middleware;

using EasyCaching.Core;
using Microsoft.AspNetCore.Http;
using System.Text;

public class EasyCachingRedisMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IEasyCachingProvider _cacheProvider;

    public EasyCachingRedisMiddleware(RequestDelegate next, IEasyCachingProvider cacheProvider)
    {
        _next = next;
        _cacheProvider = cacheProvider;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var cacheKey = context.Request.GenerateCacheKeyFromRequest();

        if (!await _cacheProvider.ExistsAsync(cacheKey))
        {
            await using var swapStream = new MemoryStream();
            var originalResponseBody = context.Response.Body;
            context.Response.Body = swapStream;
            await _next(context);
            swapStream.Seek(0, SeekOrigin.Begin);
            string responseBody = await new StreamReader(swapStream).ReadToEndAsync();
            swapStream.Seek(0, SeekOrigin.Begin);
            context.Response.Body = originalResponseBody;

            await _cacheProvider.SetAsync(cacheKey, responseBody, TimeSpan.FromMinutes(10));
            await context.Response.WriteAsync(responseBody);
        }
        else
        {
            var cacheValue = await _cacheProvider.GetAsync<string>(cacheKey);
            context.Response.ContentType = "text/plain";
            await context.Response.WriteAsync(cacheValue.Value);
        }

        await this._next(context);
    }
}

public static class CacheExtension
{
    public static string GenerateCacheKeyFromRequest(this HttpRequest httpRequest)
    {
        var keyBuilder = new StringBuilder();
        keyBuilder.Append($"{httpRequest.Path}");
        foreach (var (key, value) in httpRequest.Query.OrderBy(x => x.Key))
        {
            if (!key.Equals("refreshCache"))
                keyBuilder.Append($"|{key}-{value}");
        }

        return keyBuilder.ToString();
    }
}

public static class CacheMiddleware
{
    public static IApplicationBuilder UseCaching(this IApplicationBuilder app)
    {
        return app.UseMiddleware<EasyCachingRedisMiddleware>();
    }
}
