namespace Pokedex.Infrastructure.Http.PokemonAPI;

/// <summary>
/// List of all endpoints for the Pokemon API.
/// </summary>
public static class PokemonApiEndpoints
{
    /// <summary>
    /// Pokemon detail endpoint.
    /// </summary>
    /// <param name="pokemonName"></param>
    /// <returns></returns>
    public static string? GetPokemonDetail(string? pokemonName)
    {
        return string.IsNullOrWhiteSpace(pokemonName) ? null : $"pokemon-species/{pokemonName}";
    }
}