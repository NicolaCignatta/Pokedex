using System.Text.Json;
using Microsoft.Extensions.Logging;
using OneOf;

namespace Pokedex.Shared.Infrastructure.Http;

/// <summary>
/// Base HTTP client service for making requests and handling responses.
/// </summary>
public class HttpClientService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<HttpClientService> _logger;
    private readonly JsonSerializerOptions _jsonOptions;

    public HttpClientService(HttpClient httpClient, ILogger<HttpClientService> logger)
    {
        ArgumentNullException.ThrowIfNull(httpClient, nameof(httpClient));
        ArgumentNullException.ThrowIfNull(logger, nameof(logger));

        _httpClient = httpClient;
        _logger = logger;

        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
    }

    public async Task<OneOf<TResponse, HttpError>> GetAsync<TResponse>(
        string endpoint,
        CancellationToken cancellationToken = default)
        where TResponse : class
    {
        _logger.LogDebug("Starting GET request to {Endpoint}", endpoint);
        var rawResult = await GetRawAsync(endpoint, cancellationToken);

        return await rawResult.Match<Task<OneOf<TResponse, HttpError>>>(
            async response =>
            {
                if (!response.IsSuccessStatusCode)
                {
                    var errorBody = await response.Content.ReadAsStringAsync(cancellationToken);
                    _logger.LogError("GET request to {Endpoint} failed with status {StatusCode}", endpoint,
                        response.StatusCode);
                    return new HttpError(
                        $"HTTP request failed with status {response.StatusCode}",
                        (int)response.StatusCode,
                        errorBody);
                }

                var content = await response.Content.ReadAsStringAsync(cancellationToken);
                if (string.IsNullOrWhiteSpace(content))
                {
                    _logger.LogError("GET request to {Endpoint} returned empty content", endpoint);
                    return new HttpError("Response was empty", (int)response.StatusCode);
                }

                try
                {
                    var obj = JsonSerializer.Deserialize<TResponse>(content, _jsonOptions);
                    return obj!;
                }
                catch (JsonException ex)
                {
                    _logger.LogError(ex, "Failed to deserialize response from {Endpoint}", endpoint);
                    return new HttpError("Failed to deserialize response");
                }
            },
            error => Task.FromResult<OneOf<TResponse, HttpError>>(error)
        );
    }

    private async Task<OneOf<HttpResponseMessage, HttpError>> GetRawAsync(
        string endpoint,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(endpoint))
            return new HttpError("Endpoint cannot be null or empty");

        try
        {
            _logger.LogDebug("GET {Endpoint}", endpoint);
            var response = await _httpClient.GetAsync(endpoint, cancellationToken);
            _logger.LogDebug("Response {Status} from {Endpoint}", response.StatusCode, endpoint);
            return response;
        }
        catch (TaskCanceledException ex) when (ex.InnerException is TimeoutException)
        {
            _logger.LogError(ex, "Request to {Endpoint} timed out", endpoint);
            return new HttpError($"Request to {endpoint} timed out");
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "HTTP request exception to {Endpoint}", endpoint);
            return new HttpError($"HTTP request failed for {endpoint}: {ex.Message}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error for {Endpoint}", endpoint);
            return new HttpError("Unexpected error: " + ex.Message);
        }
    }
}