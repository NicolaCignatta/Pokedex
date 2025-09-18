using System.Net;
using Microsoft.Extensions.Logging;
using Pokedex.Domain.Aggregates;
using Pokedex.Domain.Interfaces.Repositories;
using Pokedex.Domain.Shared;
using Pokedex.Shared.Infrastructure.Http;
using OneOf;
using OneOf.Types;
using Pokedex.Infrastructure.Http.PokemonAPI.HttpResponses;

namespace Pokedex.Infrastructure.Http.PokemonAPI;

/// <summary>
/// Pokemon API HTTP client implementation for accessing Pokemon data from an external API.
/// </summary>
public class PokemonApiHttpClient(HttpClient httpClient, ILogger<HttpClientService> logger)
    : HttpClientService(httpClient, logger), IPokemonReadOnlyRepository
{
    /// <summary>
    /// Method to get detailed information about a specific Pokemon by its name.
    /// </summary>
    /// <param name="name"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<OneOf<Pokemon, NotFound, DomainError>> GetPokemonDetail(string name,CancellationToken cancellationToken = default)
    {
        var endpoint = PokemonApiEndpoints.GetPokemonDetail(name);
        if (endpoint == null)
        {
            logger.LogError("GetPokemonDetail: Invalid endpoint for Pokemon name: {PokemonName}", name);
            return DomainError.GetPokemonDetailError();
        }

        var httpResponse = await GetAsync<GetPokemonDetailHttpResponse>(endpoint,cancellationToken);
        return httpResponse.Match<OneOf<Pokemon, NotFound, DomainError>>(
            pokemon =>
            {
                if (pokemon == null)
                {
                    logger.LogError("GetPokemonDetail: Null response for Pokemon name: {PokemonName}", name);
                    return DomainError.GetPokemonDetailError();
                }
                logger.LogInformation("GetPokemonDetail: Successfully retrieved details for Pokemon name: {PokemonName}", name);
                return Pokemon.Materialize(pokemon.Id, pokemon.DefaultName, pokemon.DefaultFlavorText,
                    pokemon.HabitatName, pokemon.IsLegendary);
            },
            error =>
            {
                logger.LogError("GetPokemonDetail: Error {StatusCode} for Pokemon name: {PokemonName}", error.StatusCode, name);
                return error.StatusCode switch
                {
                    (int)HttpStatusCode.NotFound => new NotFound(),
                    _ => DomainError.GetPokemonDetailError()
                };
            });
    }
}