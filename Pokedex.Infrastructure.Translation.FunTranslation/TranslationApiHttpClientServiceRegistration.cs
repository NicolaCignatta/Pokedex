using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Pokedex.Domain.Interfaces.Translations;
using Pokedex.Shared.Infrastructure.Http;


namespace Pokedex.Infrastructure.Translation.FunTranslation;

/// <summary>
/// Registration class for HTTP client services.
/// </summary>
public static class TranslationApiHttpClientServiceRegistration
{
    public static IServiceCollection AddTranslationHttpClientServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        var options = configuration.GetSection("FunTranslationHttpClientOptions").Get<HttpClientOptions>();
        if (options == null)
            throw new InvalidOperationException("HttpClientOptions section is missing in configuration.");

        services.AddHttpClient<ITranslationService, FunTranslationApiHttpClient>((_, client) =>
            {
                client.BaseAddress = new Uri(options.BaseAddress);
                client.Timeout = options.Timeout;
            })
            .AddPolicyHandler(HttpClientExtensions.GetRetryPolicy(options));

        return services;
    }
    
   
}