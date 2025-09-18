using Pokedex.Domain.Aggregates;
using Pokedex.Domain.Interfaces.Queries;
using Pokedex.Domain.Interfaces.Repositories;
using Pokedex.Domain.Shared;
using OneOf;
using OneOf.Types;

namespace Pokedex.Domain.Queries.GetPokemonDetailQuery;

public class GetPokemonDetailQuery : IGetPokemonDetailQuery
{
    private readonly IPokemonReadOnlyRepository _pokemonReadOnlyRepository;

    public GetPokemonDetailQuery(IPokemonReadOnlyRepository pokemonReadOnlyRepository)
    {
        _pokemonReadOnlyRepository = pokemonReadOnlyRepository;
    }

    public async Task<OneOf<Pokemon, NotFound, DomainError>> Execute(string name)
    {
        return await _pokemonReadOnlyRepository.GetPokemonDetail(name);
    }
}