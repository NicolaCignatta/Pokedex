using Pokedex.Domain.Shared;
using OneOf;

namespace Pokedex.Domain.Interfaces.Translations;

public interface ITranslationService
{
    Task<OneOf<TranslatePokemonInformationResult, DomainError>> Translate(TranslatePokemonInformationCommand command,CancellationToken cancellationToken = default);
}

#region ITranslateService models results

/// <summary>
/// 
/// </summary>
/// <param name="Description"></param>
public record TranslatePokemonInformationResult(
    string Description
);

#endregion

#region ITranslateService commands

/// <summary>
/// 
/// </summary>
/// <param name="TextToTranslate">Pokemon name</param>
/// <param name="LanguageCodeToBeTranslated">Language code to be translated</param>
public record TranslatePokemonInformationCommand(
    string TextToTranslate,
    string LanguageCodeToBeTranslated
);

#endregion