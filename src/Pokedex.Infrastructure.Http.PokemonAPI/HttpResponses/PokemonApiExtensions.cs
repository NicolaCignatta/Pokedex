namespace Pokedex.Infrastructure.Http.PokemonAPI.HttpResponses;

/// <summary>
/// All extension methods for Pokemon API HTTP responses.
/// </summary>
public static class PokemonApiExtensions
{
    /// <summary>
    /// Represents a method to get a localized value from a list of name entries based on the specified language code.
    /// </summary>
    /// <param name="nameEntries"></param>
    /// <param name="languageCode"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static T? GetLocalizedValue<T>(this List<T> nameEntries, string languageCode) where T : ILocalizedResource
    {
        return nameEntries.FirstOrDefault(n => n.Language.Name.Equals(languageCode, StringComparison.OrdinalIgnoreCase));
    }
}