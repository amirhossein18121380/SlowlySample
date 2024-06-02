

using Infrastructure.Caching;

namespace SlowlySimulate.ConfigurationOptions;

public class AppSettings
{
    public ConnectionStrings ConnectionStrings { get; set; }
    public Infrastructure.Logging.LoggingOptions Logging { get; set; }
    public CachingOptions Caching { get; set; }
    public EasyCachingRedisConfig EasyCaching { get; set; }
    public CheckDependency CheckDependency { get; set; }

    public string AllowedHosts { get; set; }

    public string CurrentUrl { get; set; }


    public CookiePolicyOptions CookiePolicyOptions { get; set; }

    public Dictionary<string, string> SecurityHeaders { get; set; } = new Dictionary<string, string>();

}
