using Application;
using Domain.AuthorizationDefinitions;
using Domain.Constants;
using Domain.Identity;
using Domain.Models;
using Domain.Permissions;
using IdentityModel;
using Infrastructure.Caching;
using Infrastructure.DateTimes;
using Infrastructure.Web.ExceptionHandlers;
using Infrastructure.Web.Middleware;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Persistence;
using Serilog;
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



//services.AddHangfire(x =>
//{
//    x.UseSqlServerStorage(configuration.GetConnectionString("DefaultConnection"));
//});
//services.AddHangfireServer();
//services.AddScoped<IBirthdayNotificationService, BirthdayNotificationService>();
//services.AddSingleton<BirthdayJobScheduler>();
services.AddEasyCachingService(configuration);
services.AddSignalR();
services.AddDateTimeProvider();
services.AddMvc();
services.AddControllersWithViews();
services.AddScoped<IUserClaimsPrincipalFactory<User>, AdditionalUserClaimsPrincipalFactory>();
services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
services.AddScoped<IUserIdProvider, UserIdProvider>();
services.AddScoped<ICurrentUser, CurrentWebUser>();
services.AddTransient<IEmailSender, AuthMessageSender>();
services.AddTransient<ISmsSender, AuthMessageSender>();
services.AddScoped<EmailConfiguration>();
services.AddTransient<IEmailFactory, EmailFactory>();
services.AddTransient<IAccountManager, AccountManager>();
services.AddTransient<IEmailManager, EmailManager>();
services.AddSingleton<IUserIdProvider, UserIdProvider>();

#region Logging
builder.Host.InjectSerilog(configuration);
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

//var connectionString = configuration.GetConnectionString("DefaultConnection");
services.AddPersistence(configuration)
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
    .AddIdentityServerStores(builder.Configuration)
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
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error/Index");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

//app.UseHangfireDashboard();

//var job = app.Services.GetRequiredService<BirthdayJobScheduler>();
//job.ScheduleBirthdayCheckJob();


app.UseExceptionHandler();
app.UseDebuggingMiddleware();
app.UseIpFiltering();
app.UseDebuggingMiddleware();
app.UseSecurityHeaders(appSettings.SecurityHeaders);

app.UseSerilogRequestLogging();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseUnauthorizedRedirect();
app.UseAuthorization();
app.Configure();
app.MapHub<HubSample>("/chatHub");
//app.MapHub<BirthdayHub>("/birthdayHub");
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();