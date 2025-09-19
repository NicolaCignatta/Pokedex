using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Pokedex.API;
using Pokedex.Domain;
using Pokedex.Infrastructure.Http.PokemonAPI;
using Pokedex.Infrastructure.Translation.FunTranslation;
using StackExchange.Redis;

namespace Pokedex.FunctionalTests.Configurations
{
    public class ApiTestContainer : WebApplicationFactory<Program>
    {
        private readonly RedisTestContainer _redisContainer;

        public ApiTestContainer(RedisTestContainer redisContainer)
        {
            _redisContainer = redisContainer;
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureAppConfiguration((context, config) =>
            {
                var configValues = new Dictionary<string, string>
                {
                    // Configurazione Redis per Output Cache
                    ["RedisOutputCache:ConnectionString"] = _redisContainer.ConnectionString,
                    ["RedisOutputCache:Host"] = _redisContainer.Host,
                    ["RedisOutputCache:Port"] = _redisContainer.Port.ToString(),
                    
                    // Configurazione per abilitare Redis come provider di Output Cache
                    ["OutputCache:DefaultProvider"] = "Redis",
                };
                
                config.AddInMemoryCollection(configValues);
            });

            builder.ConfigureServices(services =>
            {
                var serviceProvider = services.BuildServiceProvider();
                var configuration = serviceProvider.GetRequiredService<IConfiguration>();

                services.AddDomainServices();
                services.AddPokemonApiHttpClientServices(configuration);
                services.AddTranslationHttpClientServices(configuration);

                // Configurazione Redis per IDistributedCache (per compatibilità)
                services.AddStackExchangeRedisCache(options =>
                {
                    options.Configuration = _redisContainer.ConnectionString;
                    options.ConfigurationOptions = new ConfigurationOptions()
                    {
                        EndPoints = { $"{_redisContainer.Host}:{_redisContainer.Port}" },
                        AbortOnConnectFail = false,
                        ConnectTimeout = 5000,
                        SyncTimeout = 1000
                    };
                    options.InstanceName = "Pokedex:FunctionalTest:";
                });

                // Configurazione Output Cache con Redis
                services.AddOutputCache(options =>
                {
                    // Configurazione di base
                    options.DefaultExpirationTimeSpan = TimeSpan.FromMinutes(5);
                })
                .AddStackExchangeRedisOutputCache(options =>
                {
                    options.Configuration = _redisContainer.ConnectionString;
                    options.ConfigurationOptions = new ConfigurationOptions()
                    {
                        EndPoints = { $"{_redisContainer.Host}:{_redisContainer.Port}" },
                        AbortOnConnectFail = false,
                        ConnectTimeout = 5000,
                        SyncTimeout = 1000
                    };
                    options.InstanceName = "Pokedex:OutputCache:FunctionalTest:";
                });

                services.AddLogging(builder =>
                {
                    builder.ClearProviders();
                    builder.AddConsole();
                    builder.SetMinimumLevel(LogLevel.Warning);
                });
            });

            builder.UseEnvironment("FunctionalTest");
        }

        public new HttpClient CreateClient() => CreateClient(new WebApplicationFactoryClientOptions
        {
            BaseAddress = new Uri("http://localhost:5000")
        });
    }
}