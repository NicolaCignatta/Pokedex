namespace Pokedex.Domain.Shared;

public record DomainError(string Message, string Code)
{
    public static DomainError GetPokemonDetailError() => new("Error in GetPokemonDetail API","GetPokemonDetailError");
}