using Pokedex.Domain.Shared;
using OneOf;

namespace Pokedex.Domain.Interfaces.Translations;

/// <summary>
/// Interface for translation services.
/// </summary>
public interface ITranslationService
{
    Task<OneOf<TranslatePokemonInformationResult, DomainError>> Translate(TranslatePokemonInformationCommand command,CancellationToken cancellationToken = default);
}

#region ITranslateService models results

/// <summary>
/// TranslatePokemonInformationResult represents the result of translating Pokemon information.
/// </summary>
/// <param name="Description"></param>
public record TranslatePokemonInformationResult(
    string Description
);

#endregion

#region ITranslateService commands

/// <summary>
/// TranslatePokemonInformationCommand represents the command to translate Pokemon information.
/// </summary>
/// <param name="TextToTranslate">Pokemon name</param>
/// <param name="LanguageCodeToBeTranslated">Language code to be translated</param>
public record TranslatePokemonInformationCommand(
    string TextToTranslate,
    string LanguageCodeToBeTranslated
);

#endregion