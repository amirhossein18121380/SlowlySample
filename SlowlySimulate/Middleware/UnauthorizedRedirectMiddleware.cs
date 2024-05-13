namespace SlowlySimulate.Middleware;

public class UnauthorizedRedirectMiddleware
{
    private readonly RequestDelegate _next;
    private readonly HashSet<string> _allowedUrls;

    public UnauthorizedRedirectMiddleware(RequestDelegate next)
    {
        _next = next;
        _allowedUrls = new HashSet<string>
        {
            "/Home/Index",
            "/",
            "/SecAccount/Login",
            "/SecAccount/Register"
        };
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            if (!context.User.Identity.IsAuthenticated)
            {
                string requestPath = context.Request.Path.Value;

                if (!_allowedUrls.Contains(requestPath) &&
                    !requestPath.StartsWith("/SecAccount/Login", StringComparison.OrdinalIgnoreCase))
                {
                    string returnUrl = context.Request.Path + context.Request.QueryString;

                    string loginUrl = $"/SecAccount/Login?returnUrl={Uri.EscapeDataString(returnUrl)}";
                    context.Response.Redirect(loginUrl);
                    return;
                }
            }


            if (context.Response.StatusCode == 401 && !context.Request.Path.StartsWithSegments("/Error"))
            {
                context.Response.Redirect($"/Error/AccessDenied");
            }

            await _next(context);
        }
        catch (Exception ex)
        {
            throw; // Re-throw the exception to propagate it further if necessary
        }
    }
}

public static class UnauthorizedRedirectMiddlewareExtensions
{
    public static IApplicationBuilder UseUnauthorizedRedirect(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<UnauthorizedRedirectMiddleware>();
    }
}