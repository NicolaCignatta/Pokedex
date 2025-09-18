namespace Pokedex.Infrastructure.Cache.Redis;

public class RedisOutputCacheSettings
{
    public string ConnectionString { get; set; } = string.Empty;
    public string InstanceName { get; set; } = "Pokedex";
    public int DefaultExpirationMinutes { get; set; } = 5;
    public int ConnectRetry { get; set; } = 3;
    public int ConnectTimeout { get; set; } = 5000;
    public bool AbortOnConnectFail { get; set; } = false;
    public int DefaultDatabase { get; set; } = 0;
    public int Port { get; set; } = 6379;
    public string Host { get; set; } = "localhost";
}