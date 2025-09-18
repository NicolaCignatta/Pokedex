namespace Pokedex.Shared.Infrastructure.Http;

public record HttpError(string Message, int? StatusCode = null, string? ResponseBody = null);