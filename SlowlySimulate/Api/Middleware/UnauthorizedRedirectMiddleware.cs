namespace SlowlySimulate.Api.Middleware;

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

                // Check if the requested URL is in the allowed list or starts with "/SecAccount/Login"
                if (!_allowedUrls.Contains(requestPath) &&
                    !requestPath.StartsWith("/SecAccount/Login", StringComparison.OrdinalIgnoreCase))
                {
                    // Store the original requested URL (returnUrl)
                    string returnUrl = context.Request.Path + context.Request.QueryString;

                    // Redirect to login page with returnUrl
                    string loginUrl = $"/SecAccount/Login?returnUrl={Uri.EscapeDataString(returnUrl)}";
                    context.Response.Redirect(loginUrl);
                    return;
                }
            }


            // Check if the response status code is 401 (Unauthorized)
            if (context.Response.StatusCode == 401 && !context.Request.Path.StartsWithSegments("/Error"))
            {
                context.Response.Redirect($"/Error/AccessDenied");
            }

            // Call the next delegate/middleware in the pipeline
            await _next(context);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in UnauthorizedRedirectMiddleware: {ex.Message}");
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