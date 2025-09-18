namespace Pokedex.Domain.Interfaces.Translations;

public interface ITranslationService
{
    Task<TranslatePokemonInformationResult> TranslatePokemonInformation(TranslatePokemonInformationCommand command);
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
/// <param name="Name">Pokemon name</param>
/// <param name="LanguageCodeToBeTranslated">Language code to be translated</param>
public record TranslatePokemonInformationCommand(
    string Name,
    string LanguageCodeToBeTranslated
);

#endregion