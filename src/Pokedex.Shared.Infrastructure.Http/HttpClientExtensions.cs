using Polly;
using Polly.Contrib.WaitAndRetry;
using Polly.Extensions.Http;

namespace Pokedex.Shared.Infrastructure.Http;

public static class HttpClientExtensions
{
    public static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy(HttpClientOptions options)
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