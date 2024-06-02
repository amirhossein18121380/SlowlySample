namespace Infrastructure.Caching;

public class EasyCachingRedisConfig
{
    public RedisConfig Redis { get; set; }
}

public class RedisConfig
{
    public int MaxRdSecond { get; set; }
    public bool EnableLogging { get; set; }
    public int LockMs { get; set; }
    public int SleepMs { get; set; }
    public string SerializerName { get; set; }
    public DbConfig DbConfig { get; set; }
}

public class DbConfig
{
    public string Password { get; set; }
    public bool IsSsl { get; set; }
    public string SslHost { get; set; }
    public int ConnectionTimeout { get; set; }
    public bool AllowAdmin { get; set; }
    public List<EndpointConfig> Endpoints { get; set; }
    public int Database { get; set; }
}

public class EndpointConfig
{
    public string Host { get; set; }
    public int Port { get; set; }
}
