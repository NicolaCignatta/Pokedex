using Pokedex.Domain.Aggregates;
using Pokedex.Domain.Shared;
using OneOf;
using OneOf.Types;

namespace Pokedex.Domain.Interfaces.Repositories;

public interface IPokemonReadOnlyRepository
{
    Task<OneOf<Pokemon, NotFound, DomainError>> GetPokemonDetail(string name);
}