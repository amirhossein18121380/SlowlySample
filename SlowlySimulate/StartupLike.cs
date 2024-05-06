using SlowlySimulate.CrossCuttingConcerns.Storage;

namespace SlowlySimulate;

public static class StartupLike
{
    public static IServiceCollection ConfigureServices(this IServiceCollection services, IWebHostEnvironment environment)
    {
        return services;
    }

    public static IApplicationBuilder Configure(this IApplicationBuilder app)
    {
        //// just for the first time
        using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
        {
            var databaseInitializer = serviceScope.ServiceProvider.GetService<IDatabaseInitializer>();
            databaseInitializer.SeedAsync().Wait();
        }

        return app;
    }
}