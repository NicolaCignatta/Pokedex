using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Pokedex.Domain.Interfaces.Repositories;
using Pokedex.Shared.Infrastructure.Http;

namespace Pokedex.Infrastructure.Http.PokemonAPI;

/// <summary>
/// Registration class for HTTP client services.
/// </summary>
public static class PokemonApiHttpClientServiceRegistration
{
    public static IServiceCollection AddPokemonApiHttpClientServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        var options = configuration.GetSection("PokemonApiHttpClientOptions").Get<HttpClientOptions>();
        if (options == null)
            throw new InvalidOperationException("HttpClientOptions section is missing in configuration.");

        services.AddHttpClient<IPokemonReadOnlyRepository, PokemonApiHttpClient>((_, client) =>
            {
                client.BaseAddress = new Uri(options.BaseAddress);
                client.Timeout = options.Timeout;
            })
            .AddPolicyHandler(HttpClientExtensions.GetRetryPolicy(options));

        return services;
    }
}