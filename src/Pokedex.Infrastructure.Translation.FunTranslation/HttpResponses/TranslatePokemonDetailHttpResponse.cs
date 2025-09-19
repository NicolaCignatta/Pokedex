namespace Pokedex.Infrastructure.Translation.FunTranslation.HttpResponses;

/// <summary>
/// Represents the HTTP response structure for detailed information about a specific Pokemon.
/// </summary>
public record TranslatePokemonDetailHttpResponse
{
    public TranslationContents? Contents { get; set; }
    public record TranslationContents
    {
        public string? Translated { get; set; }
        public string? Text { get; set; }
        public string? Translation { get; set; }
    }
}