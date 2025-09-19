using System.Net;
using FluentAssertions;
using Pokedex.FunctionalTests.Configurations;

namespace Pokedex.FunctionalTests.Tests;

[Collection("ApiTestCollection")]
public class GetPokemonDetailEndpointTests : IClassFixture<FunctionalTestFixture>
{
    private readonly HttpClient _client;
    private readonly VerifySettings _settings;

    public GetPokemonDetailEndpointTests(FunctionalTestFixture fixture)
    {
        _client = fixture.HttpClient;
        _settings = new VerifySettings();
        _settings.UseDirectory("VerifyResults");
    }

    [Fact]
    public async Task GetPokemonDetail_WithValidName_ReturnsOkWithCorrectData()
    {
        // Arrange
        var pokemonName = "pikachu";

        // Act
        var response = await _client.GetAsync($"/pokemon/{pokemonName}");

        // Assert
        var content = await response.Content.ReadAsStringAsync();
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        await Verify(new
        {
            Content = content
        }, _settings);
    }

    [Fact]
    public async Task GetPokemonDetail_WithInvalidName_ReturnsNotFound()
    {
        // Arrange
        var invalidPokemonName = "nonexistentpokemon123";

        // Act
        var response = await _client.GetAsync($"/pokemon/{invalidPokemonName}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}