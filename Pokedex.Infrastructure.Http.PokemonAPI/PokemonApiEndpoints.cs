namespace Pokedex.Infrastructure.Http.PokemonAPI;

public static class PokemonApiEndpoints
{
    public static string? GetPokemonDetail(string? pokemonName)
    {
        return string.IsNullOrWhiteSpace(pokemonName) ? null : $"pokemon-species/{pokemonName}";
    }
}