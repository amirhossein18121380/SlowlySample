using CrossCuttingConcerns.Caching;
using EasyCaching.Core.Configurations;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Caching;

public static class AddEasyCachingExt
{
    public static IServiceCollection AddEasyCachingService(this IServiceCollection services, EasyCachingRedisConfig config)
    {
        services.AddTransient<ICache, EasyCachingProvider>();
        services.AddEasyCaching(option =>
        {
            option.UseRedis((options) =>
            {
                options.SerializerName = config.Redis.SerializerName;
                //if (config?.Redis.DbConfig is not null)
                //{
                //    config.Redis.DbConfig.Endpoints.ForEach(x =>
                //        options.DBConfig.Endpoints.Add(new ServerEndPoint(x.Host, x.Port)));
                //}

                options.DBConfig.Endpoints.Add(new ServerEndPoint("localhost", 6379));
            }, "r1")

                .WithMessagePack(config.Redis.SerializerName);
        });


        return services;
    }
}