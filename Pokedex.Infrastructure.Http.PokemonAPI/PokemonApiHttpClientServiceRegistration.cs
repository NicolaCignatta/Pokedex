using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Pokedex.Domain.Interfaces.Repositories;
using Pokedex.Shared.Infrastructure.Http;
using Polly;
using Polly.Contrib.WaitAndRetry;
using Polly.Extensions.Http;

namespace Pokedex.Infrastructure.Http.PokemonAPI;

/// <summary>
/// Registration class for HTTP client services.
/// </summary>
public static class HttpClientServiceRegistration
{
    public static IServiceCollection AddHttpClientServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        var options = configuration.GetSection("HttpClientOptions").Get<HttpClientOptions>();
        if (options == null)
            throw new InvalidOperationException("HttpClientOptions section is missing in configuration.");

        services.AddHttpClient<IPokemonReadOnlyRepository, PokemonApiHttpClient>((_, client) =>
            {
                client.BaseAddress = new Uri(options.BaseAddress);
                client.Timeout = options.Timeout;
            })
            .AddPolicyHandler(GetRetryPolicy(options));

        return services;
    }
    
    private static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy(HttpClientOptions options)
    {
        var delays = Backoff.DecorrelatedJitterBackoffV2(
            medianFirstRetryDelay: options.RetryDelay,
            retryCount: options.MaxRetryAttempts);

        return HttpPolicyExtensions
            .HandleTransientHttpError()
            .Or<TaskCanceledException>()
            .WaitAndRetryAsync(delays, (outcome, timespan, retryCount, _) =>
            {
                Console.WriteLine(
                    $"Retry #{retryCount} in {timespan.TotalMilliseconds}ms. Error: {outcome.Exception?.Message ?? outcome.Result.StatusCode.ToString()}");
            });
    }
}