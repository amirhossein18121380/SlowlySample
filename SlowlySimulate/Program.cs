using Application;
using Domain.AuthorizationDefinitions;
using Domain.Constants;
using Domain.Identity;
using Domain.Models;
using Domain.Permissions;
using FluentValidation;
using IdentityModel;
using Infrastructure.Caching;
using Infrastructure.DateTimes;
using Infrastructure.HealthChecks;
using Infrastructure.Logging;
using Infrastructure.Validation;
using Infrastructure.Web.ExceptionHandlers;
using Infrastructure.Web.Middleware;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Persistence;
using SlowlySimulate;
using SlowlySimulate.Api.Authorization;
using SlowlySimulate.Api.Factories;
using SlowlySimulate.Api.Hubs;
using SlowlySimulate.Api.Manager;
using SlowlySimulate.Api.Models;
using SlowlySimulate.Api.Providers;
using SlowlySimulate.ConfigurationOptions;
using SlowlySimulate.Manager;
using SlowlySimulate.Middleware;
using SlowlySimulate.Services;


var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;

var appSettings = new AppSettings();
configuration.Bind(appSettings);
services.Configure<AppSettings>(configuration);

if (appSettings.CheckDependency.Enabled)
{
    NetworkPortCheck.Wait(appSettings.CheckDependency.Host, 3);
}

services.AddSignalR();
services.AddDateTimeProvider();
services.AddMvc();
services.AddControllersWithViews();
services.AddScoped<IUserClaimsPrincipalFactory<User>, AdditionalUserClaimsPrincipalFactory>();
services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
services.AddSingleton<IUserIdProvider, UserIdProvider>();
services.AddScoped<ICurrentUser, CurrentWebUser>();
services.AddTransient<IEmailSender, AuthMessageSender>();
services.AddTransient<ISmsSender, AuthMessageSender>();
services.AddScoped<EmailConfiguration>();
services.AddTransient<IEmailFactory, EmailFactory>();
services.AddTransient<IAccountManager, AccountManager>();
services.AddTransient<IEmailManager, EmailManager>();



services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(Program).Assembly);
});

services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
services.AddValidatorsFromAssembly(typeof(Program).Assembly);

#region Caching
//services.AddCaches(appSettings.Caching);
services.AddEasyCachingService(appSettings.EasyCaching);
#endregion

#region HealthCheck

var healthChecksBuilder = services.AddHealthChecks()
    .AddSqlServer(connectionString: appSettings.ConnectionStrings.SlowlyConnection,
        healthQuery: "SELECT 1;",
        name: "Sql Server",
        failureStatus: HealthStatus.Degraded);

services.AddHealthChecksUI(setupSettings: setup =>
{
    setup.SetEvaluationTimeInSeconds(10); // set the interval for health check updates
    setup.MaximumHistoryEntriesPerEndpoint(50); // set the maximum number of entries to keep in the UI
    setup.AddHealthCheckEndpoint("Basic Health Check", $"{appSettings.CurrentUrl}/health");
    setup.DisableDatabaseMigrations();
}).AddInMemoryStorage();


#endregion

#region Logging
builder.WebHost.UseLogger(configuration =>
{
    return appSettings.Logging;
});

#endregion

#region GlobalExceptionHandling
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();
#endregion

#region Authorization
//Add Policies / Claims / Authorization - https://identityserver4.readthedocs.io/en/latest/topics/add_apis.html#advanced 
services.AddScoped<EntityPermissions>();
services.AddSingleton<IAuthorizationPolicyProvider, AuthorizationPolicyProvider>();
services.AddTransient<IAuthorizationHandler, DomainRequirementHandler>();
services.AddTransient<IAuthorizationHandler, EmailVerifiedHandler>();
services.AddTransient<IAuthorizationHandler, PermissionRequirementHandler>();
//services.AddTransient<IAuthorizationPolicyProvider, SharedAuthorizationPolicyProvider>();
services.AddAuthorizationCore();
#endregion

#region All

services.AddPersistence(configuration, appSettings.ConnectionStrings.SlowlyConnection)
    .AddApplicationServices(configuration)
    .AddMessageHandlers();

services.AddIdentity<User, Role>()
    .AddRoles<Role>()
    .AddSignInManager()
    .AddEntityFrameworkStores<SlowlyDbContext>()
    .AddDefaultTokenProviders();

var identityServerBuilder = services.AddIdentityServer(options =>
    {
        options.IssuerUri = "authAuthority";
        options.Events.RaiseErrorEvents = true;
        options.Events.RaiseInformationEvents = true;
        options.Events.RaiseFailureEvents = true;
        options.Events.RaiseSuccessEvents = true;
        options.UserInteraction.ErrorUrl = "/identityserver/error";
    })
    .AddIdentityServerStores(builder.Configuration, appSettings.ConnectionStrings.SlowlyConnection)
    .AddAspNetIdentity<User>();

services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        //options.Authority = authAuthority;
        options.RequireHttpsMetadata = builder.Environment.IsProduction();
        options.Audience = IdentityServerConfig.LocalApiName;
        options.TokenValidationParameters.ValidTypes = new[] { JwtClaimTypes.JwtTypes.AccessToken };

        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                var accessToken = context.Request.Query["access_token"];

                // If the request is for our hub...
                var path = context.HttpContext.Request.Path;
                if (!string.IsNullOrEmpty(accessToken) &&
                    path.StartsWithSegments(HubPaths.Chat))
                {
                    // Read the token out of the query string
                    context.Token = accessToken;
                }
                return Task.CompletedTask;
            }
        };
    });
#endregion


var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error/Index");
    app.UseHsts();
}

app.UseRouting();
app.UseExceptionHandler();
app.UseDebuggingMiddleware();
app.UseIpFiltering();
app.UseDebuggingMiddleware();
app.UseSecurityHeaders(appSettings.SecurityHeaders);
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseUnauthorizedRedirect();
app.UseAuthorization();
app.Configure();
app.UseHealthChecks("/health", new HealthCheckOptions
{
    Predicate = _ => true,
    //ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse,
    ResponseWriter = StartupLike.WriteResponse,
    ResultStatusCodes =
    {
        [HealthStatus.Healthy] = StatusCodes.Status200OK,
        [HealthStatus.Degraded] = StatusCodes.Status500InternalServerError,
        [HealthStatus.Unhealthy] = StatusCodes.Status503ServiceUnavailable,
    },
});
app.UseHealthChecksUI(options =>
{
    options.UIPath = "/health-ui";
    options.ApiPath = "/health-ui-api";
});

app.MapHub<HubSample>("/chatHub");
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();