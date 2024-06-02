using FluentValidation;
using Microsoft.AspNetCore.Http;
using System.Net;

namespace Infrastructure.Web.Middleware;

public class FluentValidationMiddleware : IMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IEnumerable<IValidator> _validators;

    public FluentValidationMiddleware(RequestDelegate next, IEnumerable<IValidator> validators)
    {
        _next = next;
        _validators = validators;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var request = await context.Request.ReadFromJsonAsync<object>();
        var validationResults = await ValidateRequestAsync(request);

        if (validationResults.Any(r => r.Errors.Any()))
        {
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            await context.Response.WriteAsJsonAsync(new { errors = validationResults.SelectMany(r => r.Errors).Select(e => e.ErrorMessage) });
            return;
        }

        await _next(context);
    }

    private async Task<IEnumerable<FluentValidation.Results.ValidationResult>> ValidateRequestAsync(object request)
    {
        var validationTasks = _validators.Select(v => v.ValidateAsync(new ValidationContext<object>(request)));
        return await Task.WhenAll(validationTasks);
    }
}
