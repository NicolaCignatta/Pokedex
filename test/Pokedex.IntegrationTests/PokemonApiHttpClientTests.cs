using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;
using OneOf.Types;
using Pokedex.Infrastructure.Http.PokemonAPI;
using Pokedex.Shared.Infrastructure.Http;

namespace Pokedex.IntegrationTests;

public class PokemonApiHttpClientTests
{
    [Fact]
    public async Task GetPokemonDetail_ReturnsPokemon_FromRealApi()
    {
        // Arrange
        var httpClient = new HttpClient { BaseAddress = new Uri("https://pokeapi.co/api/v2/") };
        var logger = NullLogger<HttpClientService>.Instance;
        var repo = new PokemonApiHttpClient(httpClient, logger);

        // Act
        var result = await repo.GetPokemonDetail("pikachu");

        // Assert
        result.AsT0.Description.Should().NotBeNullOrWhiteSpace();
        result.AsT0.HabitatName.Should().NotBeNull();
        result.AsT0.Id.Should().BeGreaterThan(0);
        result.AsT0.Name.Should().NotBeNullOrWhiteSpace();
    }

    [Fact]
    public async Task GetPokemonDetailWithNoExistingPokemon_NoReturnPokemon_FromRealApi()
    {
        // Arrange
        var httpClient = new HttpClient { BaseAddress = new Uri("https://pokeapi.co/api/v2/") };
        var logger = NullLogger<HttpClientService>.Instance;
        var repo = new PokemonApiHttpClient(httpClient, logger);

        // Act
        var result = await repo.GetPokemonDetail("notexistpokemon");

        // Assert

        // Assert
        result.AsT1.Should().BeOfType<NotFound>();
    }
}