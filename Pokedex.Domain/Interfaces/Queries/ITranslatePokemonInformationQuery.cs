using OneOf;
using OneOf.Types;
using Pokedex.Domain.Aggregates;
using Pokedex.Domain.Shared;

namespace Pokedex.Domain.Interfaces.Queries;

/// <summary>
/// Query interface for translating Pokemon information with all integration of external services.
/// </summary>
public interface ITranslatePokemonInformationQuery
{
    Task<OneOf<Pokemon, NotFound, DomainError>> Execute(string name,CancellationToken cancellationToken = default);
}