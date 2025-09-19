using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Pokedex.API;
using Pokedex.Domain;
using Pokedex.Infrastructure.Http.PokemonAPI;
using Pokedex.Infrastructure.Translation.FunTranslation;

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
                config.AddJsonFile("appsettings.json", optional: true);
                config.AddJsonFile("appsettings.FunctionalTest.json", optional: false);
                
                var configValues = new Dictionary<string, string>
                {
                    ["RedisOutputCache:ConnectionString"] = _redisContainer.ConnectionString,
                    ["RedisOutputCache:Host"] = "localhost",
                    ["RedisOutputCache:Port"] = _redisContainer.Port.ToString(),
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

                services.AddStackExchangeRedisCache(options =>
                {
                    options.Configuration = _redisContainer.ConnectionString;
                    options.InstanceName = "Pokedex:FunctionalTest:";
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