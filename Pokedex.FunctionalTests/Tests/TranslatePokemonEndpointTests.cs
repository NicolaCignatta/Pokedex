using System.Net;
using FluentAssertions;
using Pokedex.FunctionalTests.Configurations;

namespace Pokedex.FunctionalTests.Tests;

[Collection("ApiTestCollection")]
public class TranslatePokemonDetailEndpointTests : IClassFixture<FunctionalTestFixture>
{
    private readonly HttpClient _client;
    private readonly VerifySettings _settings;

    public TranslatePokemonDetailEndpointTests(FunctionalTestFixture fixture)
    {
        _client = fixture.HttpClient;
        _settings = new VerifySettings();
        _settings.UseDirectory("VerifyResults");
    }

    [Fact]
    public async Task TranslatePokemonDetail_WithCaveHabitatAndIsLegendary_ReturnsYodaTranslatedDescription()
    {
        // Arrange
        var pokemonName = "registeel"; 

        // Act
        var response = await _client.GetAsync($"/pokemon/translate/{pokemonName}");

        // Assert
        var content = await response.Content.ReadAsStringAsync();
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        await Verify(new
        {
            Content = content
        }, _settings);
    }

    [Fact]
    public async Task TranslatePokemonDetail_WithNonExistingPokemon_ReturnsNotFound()
    {
        // Arrange
        var invalidPokemonName = "nonexistentpokemon123";

        // Act
        var response = await _client.GetAsync($"/pokemon/translate/{invalidPokemonName}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
    
    [Fact]
    public async Task TranslatePokemonDetail_LegendaryPokemon_ReturnsYodaTranslated()
    {
        // Arrange
        var pokemonName = "mewtwo"; 

        // Act
        var response = await _client.GetAsync($"/pokemon/translate/{pokemonName}");

        // Assert
        var apiResponse = await response.Content.ReadAsStringAsync();
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        await Verify(new
        {
            Content = apiResponse
        }, _settings);
    }

    [Fact]
    public async Task TranslatePokemonDetail_CavePokemon_ReturnsYodaTranslated()
    {
        // Arrange
        var pokemonName = "articuno"; 

        // Act
        var response = await _client.GetAsync($"/pokemon/translate/{pokemonName}");

        // Assert
        var apiResponse = await response.Content.ReadAsStringAsync();
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        await Verify(new
        {
            Response = apiResponse
        }, _settings);
    }

    [Fact]
    public async Task TranslatePokemonDetail_NormalPokemon_ReturnsShakespeareTranslated()
    {
        // Arrange
        var pokemonName = "pikachu"; 

        // Act
        var response = await _client.GetAsync($"/pokemon/translate/{pokemonName}");

        // Assert
        var apiResponse = await response.Content.ReadAsStringAsync();
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        await Verify(new
        {
            Content = apiResponse
        }, _settings);
    }
}
