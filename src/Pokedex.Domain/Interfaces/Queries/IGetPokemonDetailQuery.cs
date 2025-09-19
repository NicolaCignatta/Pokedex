using Pokedex.Domain.Aggregates;
using Pokedex.Domain.Shared;
using OneOf;
using OneOf.Types;

namespace Pokedex.Domain.Interfaces.Queries;

/// <summary>
/// Query interface for retrieving detailed information about a specific Pokemon by its name.
/// </summary>
public interface IGetPokemonDetailQuery
{
    Task<OneOf<Pokemon, NotFound, DomainError>> Execute(string name,CancellationToken cancellationToken = default);
}

