using OneOf;
using OneOf.Types;
using Pokedex.Domain.Aggregates;
using Pokedex.Domain.Interfaces.Queries;
using Pokedex.Domain.Interfaces.Repositories;
using Pokedex.Domain.Interfaces.Translations;
using Pokedex.Domain.Shared;

namespace Pokedex.Domain.Queries.TranslatePokemonInformationQuery;

public class TranslatePokemonInformationQuery : ITranslatePokemonInformationQuery
{
    private readonly IPokemonReadOnlyRepository _pokemonReadOnlyRepository;
    private readonly ITranslationService _translationService;

    public TranslatePokemonInformationQuery(IPokemonReadOnlyRepository pokemonReadOnlyRepository,
        ITranslationService translationService)
    {
        _pokemonReadOnlyRepository = pokemonReadOnlyRepository;
        _translationService = translationService;
    }

    public async Task<OneOf<Pokemon, NotFound, DomainError>> Execute(string name,
        CancellationToken cancellationToken = default)
    {
        var pokemonResult = await _pokemonReadOnlyRepository.GetPokemonDetail(name, cancellationToken);
        return await pokemonResult.Match<Task<OneOf<Pokemon, NotFound, DomainError>>>(
            async pokemon =>
            {
                var translationResult = await _translationService.Translate(
                    new TranslatePokemonInformationCommand(pokemon.Description, pokemon.MyLanguage.Code),
                    cancellationToken);

                return translationResult.Match<OneOf<Pokemon, NotFound, DomainError>>(
                    translated =>
                    {
                        pokemon.TranslateDescription(translated.Description);
                        return pokemon;
                    },
                    error => error
                );
            },
            notFound => Task.FromResult<OneOf<Pokemon, NotFound, DomainError>>(notFound),
            error => Task.FromResult<OneOf<Pokemon, NotFound, DomainError>>(error)
        );
    }
}