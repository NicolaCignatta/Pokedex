using Pokedex.Domain.Aggregates;
using Pokedex.Domain.Shared;
using OneOf;
using OneOf.Types;

namespace Pokedex.Domain.Interfaces.Repositories;

/// <summary>
/// Data repository interface for read-only operations related to Pokemon entities.
/// Now used by queries to fetch data from Pokemon API.
/// </summary>
public interface IPokemonReadOnlyRepository
{
    Task<OneOf<Pokemon, NotFound, DomainError>> GetPokemonDetail(string name, CancellationToken cancellationToken = default);
}