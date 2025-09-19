using System.Net;
using Pokedex.Domain.ValueObjects;

namespace Pokedex.Infrastructure.Translation.FunTranslation;

/// <summary>
/// List of all endpoints for the FunTranslations API.
/// </summary>
public static class FunTranslationApiEndpoints
{
    /// <summary>
    /// Get Yoda translation endpoint with the text to translate.
    /// </summary>
    /// <param name="textToTranslate"></param>
    /// <returns></returns>
    public static string? GetTranslateYodaApi(string textToTranslate)
    {
        return string.IsNullOrWhiteSpace(textToTranslate) ? null : $"translate/yoda.json?text={textToTranslate}";
    }
    
    /// <summary>
    /// Get Shakespeare translation endpoint with the text to translate.
    /// </summary>
    /// <param name="textToTranslate"></param>
    /// <returns></returns>
    public static string? GetTranslateShakespeareApi(string textToTranslate)
    {
        return string.IsNullOrWhiteSpace(textToTranslate) ? null : $"translate/shakespeare.json?text={textToTranslate}";
    }
    
    /// <summary>
    /// Factory method to get the appropriate translation endpoint based on the language code.
    /// </summary>
    /// <param name="language"></param>
    /// <param name="textToTranslate"></param>
    /// <returns></returns>
    public static string? GetTranslationEndpointByLanguage(string language, string textToTranslate)
    {
        if(string.IsNullOrWhiteSpace(language) || string.IsNullOrWhiteSpace(textToTranslate))
        {
            return null;
        }
        
        var sanitized = textToTranslate.Replace("\r", "").Replace("\n", " ");
        var encodedTextToTranslate = WebUtility.UrlEncode(sanitized.Trim());

        return language switch
        {
            _ when language == Language.Yoda.Code => GetTranslateYodaApi(encodedTextToTranslate),
            _ when language == Language.Shakespeare.Code => GetTranslateShakespeareApi(encodedTextToTranslate),
            _ => null
        };
    }
}