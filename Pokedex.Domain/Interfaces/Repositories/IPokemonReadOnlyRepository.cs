using Pokedex.Domain.Aggregates;

namespace Pokedex.Domain.Interfaces;

public interface IPokemonReadOnlyRepository
{
    Task<Pokemon?> GetPokemonByName(GetPokemonByNameQuery query);
}

#region IPokemonReadOnlyRepository queries

/// <summary>
/// 
/// </summary>
/// <param name="Name">Pokemon name</param>
public record GetPokemonByNameQuery(
    string Name
);

#endregion

