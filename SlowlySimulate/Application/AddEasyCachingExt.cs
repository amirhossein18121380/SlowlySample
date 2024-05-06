using EasyCaching.Core.Configurations;

namespace SlowlySimulate.Application;

public static class AddEasyCachingExt
{
    public static IServiceCollection AddEasyCachingService(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddEasyCaching(option =>
        {
            //option.UseRedis(config =>
            //{
            //    config.DBConfig.Endpoints.Add(new ServerEndPoint("127.0.0.1", 6379));
            //}, "redis1").WithMessagePack("msgpack");

            option.UseRedis((options) =>
            {
                options.SerializerName = "msgpack";
                options.DBConfig.Endpoints.Add(new ServerEndPoint("localhost", 6379));
            }, "r1").WithMessagePack("msgpack");

            //"easycaching": {
            //    "redis": {
            //        "MaxRdSecond": 120,
            //        "EnableLogging": false,
            //        "LockMs": 5000,
            //        "SleepMs": 300,
            //        "SerializerName": "msgpack",
            //        "dbconfig": {
            //            "Password": null,
            //            "IsSsl": false,
            //            "SslHost": null,
            //            "ConnectionTimeout": 5000,
            //            "AllowAdmin": true,
            //            "Endpoints": [
            //            {
            //                "Host": "localhost",
            //                "Port": 6739
            //            }
            //            ],
            //            "Database": 0
            //        }
            //    }
            //}
        });


        return services;
    }
}