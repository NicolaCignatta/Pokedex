using Microsoft.Extensions.Logging;
using Pokedex.Domain.Interfaces.Translations;
using Pokedex.Domain.Shared;
using Pokedex.Infrastructure.Translation.FunTranslation.HttpResponses;
using Pokedex.Shared.Infrastructure.Http;
using OneOf;

namespace Pokedex.Infrastructure.Translation.FunTranslation;

/// <summary>
/// Pokemon API HTTP client implementation for accessing Pokemon data from an external API.
/// </summary>
public class FunTranslationApiHttpClient(HttpClient httpClient, ILogger<HttpClientService> logger)
    : HttpClientService(httpClient, logger), ITranslationService
{
    /// <summary>
    /// Method to translate Pokemon information using the FunTranslation API.
    /// </summary>
    /// <param name="command"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<OneOf<TranslatePokemonInformationResult, DomainError>> Translate(
        TranslatePokemonInformationCommand command, CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Starting translation for text: {TextToTranslate} to language: {LanguageCode}",
            command.TextToTranslate, command.LanguageCodeToBeTranslated);
        var endpoint =
            FunTranslationApiEndpoints.GetTranslationEndpointByLanguage(command.LanguageCodeToBeTranslated,
                command.TextToTranslate);
        if (endpoint == null)
        {
            logger.LogError("translate: Invalid endpoint for text: {TextToTranslate} and language: {LanguageCode}",
                command.TextToTranslate, command.LanguageCodeToBeTranslated);
            return DomainError.TranslationError();
        }
        logger.LogInformation("translate: Using endpoint: {Endpoint}", endpoint);
        
        var httpResponse = await GetAsync<TranslatePokemonDetailHttpResponse>(endpoint, cancellationToken);
        return httpResponse.Match<OneOf<TranslatePokemonInformationResult, DomainError>>(
            translation =>
            {
                logger.LogInformation(
                    "translate: Successfully retrieved translation for text: {TextToTranslate} and language: {LanguageCode}",
                    command.TextToTranslate, command.LanguageCodeToBeTranslated);
                return new TranslatePokemonInformationResult(translation?.Contents?.Translated ?? string.Empty);
            },
            error =>
            {
                logger.LogError(
                    "translate: Error {StatusCode} for text: {TextToTranslate} and language: {LanguageCode}",
                    error.StatusCode, command.TextToTranslate, command.LanguageCodeToBeTranslated);
                return DomainError.TranslationError();
            });
    }
}