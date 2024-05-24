using CrossCuttingConcerns.Storage;
using Grpc.Core;
using Grpc.Core.Interceptors;

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
public class GrpcExceptionInterceptor : Interceptor
{
    public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(
        TRequest request,
        ServerCallContext context,
        UnaryServerMethod<TRequest, TResponse> continuation)
    {
        try
        {
            return await continuation(request, context);
        }
        catch (System.Exception exception)
        {
            throw new RpcException(new Status(StatusCode.Internal, exception.Message));
        }
    }
}