using Application.Common;
using Application.Common.Queries;
using Application.Common.Services;
using Application.Roles.Service;
using Application.Topic.Services;
using Application.TopicOfInterest.Services;
using Application.Users.Services;
using Domain.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Application;

public static class ApplicationServicesExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration conf)
    {
        services.AddScoped(typeof(ICrudService<>), typeof(CrudService<>))
            .AddScoped<IUserService, UserService>()
            .AddScoped<IRoleService, RoleService>()
            .AddScoped<ITopicService, TopicService>()
            .AddScoped<ISubTopicService, SubTopicService>()
            .AddScoped<IFriendService, FriendService>()
            .AddScoped<ITopicOfInterestService, TopicOfInterestService>();

        // Add AutoMapper with all profiles in the assembly
        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        //services.AddAutoMapper(Assembly.GetExecutingAssembly());



        return services;
    }
    public static IApplicationBuilder ApplicationConfigure(this IApplicationBuilder app)
    {
        return app;
    }

    public static IServiceCollection AddMessageHandlers(this IServiceCollection services)
    {
        services.AddScoped<Dispatcher>();

        var assembly = Assembly.GetExecutingAssembly();

        var assemblyTypes = assembly.GetTypes();

        foreach (var type in assemblyTypes)
        {
            var handlerInterfaces = type.GetInterfaces()
               .Where(Utils.IsHandlerInterface)
               .ToList();

            if (!handlerInterfaces.Any())
            {
                continue;
            }

            var handlerFactory = new HandlerFactory(type);
            foreach (var interfaceType in handlerInterfaces)
            {
                services.AddTransient(interfaceType, provider => handlerFactory.Create(provider, interfaceType));
            }
        }

        var aggregateRootTypes = typeof(IAggregateRoot).Assembly.GetTypes().Where(x => x.IsSubclassOf(typeof(Entity<Guid>)) && x.GetInterfaces().Contains(typeof(IAggregateRoot))).ToList();

        var genericHandlerTypes = new[]
        {
            typeof(GetEntititesQueryHandler<>),
            typeof(GetEntityByIdQueryHandler<>),
            //typeof(AddOrUpdateEntityCommandHandler<,>),
            //typeof(UpdateEntityCommandHandler<,>),
            //typeof(DeleteEntityCommandHandler<,>),
        };

        foreach (var aggregateRootType in aggregateRootTypes)
        {
            foreach (var genericHandlerType in genericHandlerTypes)
            {
                var handlerType = genericHandlerType.MakeGenericType(aggregateRootType);

                var handlerInterfaces = handlerType.GetInterfaces();

                var handlerFactory = new HandlerFactory(handlerType);
                foreach (var interfaceType in handlerInterfaces)
                {
                    services.AddTransient(interfaceType, provider => handlerFactory.Create(provider, interfaceType));
                }
            }
        }

        Dispatcher.RegisterEventHandlers(assembly, services);

        return services;
    }
}
