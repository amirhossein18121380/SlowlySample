using CrossCuttingConcerns.Storage;
using Domain.Permissions;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Persistence.Repositories;
using System.Reflection;

namespace Persistence;

public static class PersistenceExtensions
{
#if DEBUG
    public static readonly ILoggerFactory factory = LoggerFactory.Create(builder => { builder.AddDebug(); });
#endif
    public static IServiceCollection AddPersistence(this IServiceCollection services, Microsoft.Extensions.Configuration.IConfiguration configuration, string? connectionString, string migrationsAssembly = "")
    {
        //var connectionString = configuration.GetConnectionString("DefaultConnection");

        services.AddDbContext<SlowlyDbContext>(options => options.UseSqlServer(connectionString, sql =>
        {
            if (!string.IsNullOrEmpty(migrationsAssembly))
            {
                sql.MigrationsAssembly(migrationsAssembly);
            }
        })).AddDbContextFactory<SlowlyDbContext>((Action<DbContextOptionsBuilder>)null, ServiceLifetime.Scoped)
            .AddRepositories();
        services.AddScoped<EntityPermissions>();
        services.AddScoped<ApplicationPersistenceManager>();
        services.AddTransient<IDatabaseInitializer, DatabaseInitializer>();

        return services;
    }

    private static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped(typeof(IRepository<,>), typeof(Repository<,>))
            .AddScoped(typeof(ISlowlyUserRepository), typeof(SlowlyUserRepository))
            .AddScoped(typeof(IMatchingPreferencesRepository), typeof(MatchingPreferencesRepository));

        services.AddScoped(typeof(IUnitOfWork), services =>
        {
            return services.GetRequiredService<SlowlyDbContext>();
        });

        return services;
    }

    public static IIdentityServerBuilder AddIdentityServerStores(this IIdentityServerBuilder builder, Microsoft.Extensions.Configuration.IConfiguration configuration, string connectionString)
    {
        return builder.AddConfigurationStore(options =>
                {
                    options.ConfigureDbContext = x => GetDbContextOptions<SlowlyDbContext>(x, configuration, connectionString);
                })
                .AddOperationalStore(options =>
                {
                    options.ConfigureDbContext = x => GetDbContextOptions<SlowlyDbContext>(x, configuration, connectionString);

                    // this enables automatic token cleanup. this is optional.
                    options.EnableTokenCleanup = true;

                    options.TokenCleanupInterval = 3600; //In Seconds 1 hour
                });
    }

    public static void GetDbContextOptions<T>(DbContextOptionsBuilder builder, Microsoft.Extensions.Configuration.IConfiguration configuration
    , string connectionString) where T : DbContext
    {
        builder.UseLoggerFactory(factory).EnableSensitiveDataLogging();
        var migrationsAssembly = typeof(T).GetTypeInfo().Assembly.GetName().Name;

        //var connectionString = configuration.GetConnectionString("DefaultConnection");

        if (string.IsNullOrEmpty(connectionString))
            throw new ArgumentNullException("The SlowlyConnection was not found.");

        if (!connectionString.ToLower().Contains("multipleactiveresultsets=true"))
            throw new ArgumentException("When Sql Server is in use the DefaultConnection must contain: MultipleActiveResultSets=true");

        builder.UseSqlServer(connectionString, options =>
        {
            options.CommandTimeout(60);
            options.MigrationsAssembly(migrationsAssembly);

        });

    }
}
