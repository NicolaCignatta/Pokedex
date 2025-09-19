using OneOf.Types;
using Pokedex.Domain.Aggregates;
using Pokedex.Domain.Shared;
using OneOf;

namespace Pokedex.Domain.Queries.TranslatePokemonInformationQuery;

public interface ITranslatePokemonInformationQuery
{
    Task<OneOf<Pokemon, NotFound, DomainError>> Execute(string name,CancellationToken cancellationToken = default);
}