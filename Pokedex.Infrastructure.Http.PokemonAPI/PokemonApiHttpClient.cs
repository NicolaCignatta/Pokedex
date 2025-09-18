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

public class PokemonApiHttpClient : HttpClientService, IPokemonReadOnlyRepository
{
    public PokemonApiHttpClient(HttpClient httpClient, ILogger<HttpClientService> logger) : base(httpClient, logger)
    {
    }

    public async Task<OneOf<Pokemon, NotFound, DomainError>> GetPokemonDetail(string name)
    {
        var endpoint = PokemonApiEndpoints.GetPokemonDetail(name);
        if (endpoint == null)
            return DomainError.GetPokemonDetailError();
        var httpResponse = await GetAsync<GetPokemonDetailHttpResponse>(endpoint);
        return httpResponse.Match<OneOf<Pokemon, NotFound, DomainError>>(
            pokemon =>
            {
                if(pokemon == null)
                    return DomainError.GetPokemonDetailError();
                return Pokemon.Materialize(pokemon.Id, pokemon.DefaultName, pokemon.DefaultFlavorText,
                    pokemon.HabitatName, pokemon.IsLegendary);
            },
            error =>
            {
                return error.StatusCode switch
                {
                    (int)HttpStatusCode.NotFound => new NotFound(),
                    _ => DomainError.GetPokemonDetailError()
                };
            });
    }
}