namespace Pokedex.Shared.Infrastructure.Http;

/// <summary>
/// Error model return from HTTP client.
/// </summary>
/// <param name="Message"></param>
/// <param name="StatusCode"></param>
/// <param name="ResponseBody"></param>
public record HttpError(string Message, int? StatusCode = null, string? ResponseBody = null);