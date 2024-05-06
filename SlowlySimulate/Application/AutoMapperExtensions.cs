namespace SlowlySimulate.Application;

//public static class AutoMapperExtensions
//{
//    public static IServiceCollection AddAutoMapperProfiles(this IServiceCollection services, Assembly assembly)
//    {
//        // Get all types in the assembly that inherit from Profile
//        var profiles = assembly.GetTypes()
//            .Where(t => typeof(Profile).IsAssignableFrom(t) && !t.IsAbstract);

//        // Register each profile
//        foreach (var profile in profiles)
//        {
//            services.AddAutoMapper(profile);
//        }

//        return services;
//    }
//}