namespace Pokedex.Shared.Infrastructure.Http;

/// <summary>
/// Http client configuration. Now they are got from appsettings.json.
/// </summary>
public class HttpClientOptions
{
    public string BaseAddress { get; set; }
    public TimeSpan Timeout { get; set; } = TimeSpan.FromSeconds(30);
    public int MaxRetryAttempts { get; set; } = 3;
    public TimeSpan RetryDelay { get; set; } = TimeSpan.FromSeconds(1);
}