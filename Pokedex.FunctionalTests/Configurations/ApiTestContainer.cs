using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Pokedex.API;
using Pokedex.Domain;
using Pokedex.Infrastructure.Http.PokemonAPI;

namespace Pokedex.FunctionalTests.Configurations
{
    public class ApiTestContainer : WebApplicationFactory<Program>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureAppConfiguration((context, config) =>
            {
                config.AddJsonFile("appsettings.FunctionalTest.json", optional: false);
            });
            builder.ConfigureServices(services =>
            { 
                var configuration = services.BuildServiceProvider().GetRequiredService<IConfiguration>();
                services.AddDomainServices();
                services.AddHttpClientServices(configuration);
            });
        }

        public new HttpClient CreateClient() => CreateClient(new WebApplicationFactoryClientOptions
        {
            BaseAddress = new Uri("http://localhost:5000")
        });
    }
}