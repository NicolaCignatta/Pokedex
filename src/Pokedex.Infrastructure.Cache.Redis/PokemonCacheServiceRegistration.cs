using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace Pokedex.Infrastructure.Cache.Redis;

public static class PokemonCacheServiceRegistration
{
    public static IServiceCollection AddOutputCaching(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<RedisOutputCacheSettings>(configuration.GetSection("RedisOutputCache"));

        var settings = configuration.GetSection("RedisOutputCache").Get<RedisOutputCacheSettings>()
                       ?? new RedisOutputCacheSettings();

        services.AddOutputCache(options =>
        {
            options.DefaultExpirationTimeSpan = TimeSpan.FromMinutes(settings.DefaultExpirationMinutes);
            options.AddPolicy("PokemonEndpointsPolicy", policy => policy.Expire(TimeSpan.FromMinutes(1)));
            options.AddPolicy("TranslationEndpointsPolicy", policy => policy.Expire(TimeSpan.FromMinutes(5)));
            options.AddPolicy("NoCachePolicy", policy => policy.NoCache());
        });
        
        services.AddStackExchangeRedisOutputCache(options =>
        {
            options.ConfigurationOptions = new ConfigurationOptions
            {
                EndPoints = { settings.ConnectionString },
                DefaultDatabase = settings.DefaultDatabase,
                AbortOnConnectFail = settings.AbortOnConnectFail,
                ConnectRetry = settings.ConnectRetry,
                ConnectTimeout = settings.ConnectTimeout
            };
        });

        return services;
    }
}