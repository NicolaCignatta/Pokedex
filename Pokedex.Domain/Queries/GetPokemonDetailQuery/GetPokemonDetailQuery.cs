using Pokedex.Domain.Aggregates;
using Pokedex.Domain.Interfaces.Queries;
using Pokedex.Domain.Interfaces.Repositories;
using Pokedex.Domain.Shared;
using OneOf;
using OneOf.Types;

namespace Pokedex.Domain.Queries.GetPokemonDetailQuery;

/// <summary>
/// Pokemon detail query implementation to retrieve detailed information about a specific Pokemon by its name.
/// Implements all query logic and interacts with all other infrastructures components(e.g., repositories) to fulfill the query request.
/// </summary>
public class GetPokemonDetailQuery : IGetPokemonDetailQuery
{
    private readonly IPokemonReadOnlyRepository _pokemonReadOnlyRepository;

    public GetPokemonDetailQuery(IPokemonReadOnlyRepository pokemonReadOnlyRepository)
    {
        _pokemonReadOnlyRepository = pokemonReadOnlyRepository;
    }

    public async Task<OneOf<Pokemon, NotFound, DomainError>> Execute(string name,CancellationToken cancellationToken = default)
    {
        return await _pokemonReadOnlyRepository.GetPokemonDetail(name,cancellationToken);
    }
}