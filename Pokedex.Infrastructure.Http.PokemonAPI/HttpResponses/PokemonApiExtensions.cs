namespace Pokedex.Infrastructure.Http.PokemonAPI.HttpResponses;

public static class PokemonApiExtensions
{
    public static T? GetLocalizedValue<T>(this List<T> nameEntries, string languageCode) where T : ILocalizedResource
    {
        return nameEntries.FirstOrDefault(n => n.Language.Name.Equals(languageCode, StringComparison.OrdinalIgnoreCase));
    }
}