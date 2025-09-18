namespace Pokedex.Domain.Shared;

/// <summary>
/// Object representing a domain error with a message and a code.
/// Domain errors are used to represent unexpected errors that occur during domain operations.
/// </summary>
/// <param name="Message"></param>
/// <param name="Code"></param>
public record DomainError(string Message, string Code)
{
    /// <summary>
    /// Factory method to create a DomainError for GetPokemonDetail API errors.
    /// </summary>
    /// <returns></returns>
    public static DomainError GetPokemonDetailError() => new("Error in GetPokemonDetail API","GetPokemonDetailError");
}