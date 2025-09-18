using Pokedex.Domain.Aggregates;
using Pokedex.Domain.Shared;
using OneOf;
using OneOf.Types;

namespace Pokedex.Domain.Interfaces.Queries;

public interface IGetPokemonDetailQuery
{
    Task<OneOf<Pokemon, NotFound, DomainError>> Execute(string name);
}

