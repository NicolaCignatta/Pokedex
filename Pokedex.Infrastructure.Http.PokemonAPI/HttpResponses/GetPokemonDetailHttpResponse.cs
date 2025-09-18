using Pokedex.Domain.Shared;
using System.Text.Json.Serialization;

namespace Pokedex.Infrastructure.Http.PokemonAPI.HttpResponses;

/// <summary>
/// Represents the HTTP response structure for detailed information about a specific Pokemon.
/// </summary>
public class GetPokemonDetailHttpResponse
{
    public GetPokemonDetailHttpResponse()
    {
        Names = [];
    }

    [JsonPropertyName("id")] public int Id { get; set; }

    [JsonPropertyName("flavor_text_entries")]
    public List<FlavorTextEntry> FlavorTextEntries { get; set; }

    [JsonPropertyName("names")] public List<NameEntry> Names { get; set; }

    [JsonPropertyName("habitat")] public NamedApiResource Habitat { get; set; }

    [JsonPropertyName("is_legendary")] public bool IsLegendary { get; set; }

    /// <summary>
    /// Gets the default name of the Pokemon in the default language.
    /// </summary>
    public string? DefaultName => Names?.GetLocalizedValue(Language.DefaultCulture)?.Name;

    public string? HabitatName => Habitat?.Name;

    /// <summary>
    /// Gets the default flavor text of the Pokemon in the default language.
    /// </summary>
    public string? DefaultFlavorText => FlavorTextEntries.GetLocalizedValue(Language.DefaultCulture)?.FlavorText;
}

#region sub classes

public class FlavorTextEntry : ILocalizedResource
{
    [JsonPropertyName("flavor_text")] public string FlavorText { get; set; }

    [JsonPropertyName("language")] public NamedApiResource Language { get; set; }
}

public class NameEntry : ILocalizedResource
{
    [JsonPropertyName("name")] public string Name { get; set; }
    [JsonPropertyName("language")] public NamedApiResource Language { get; set; }
}

public class NamedApiResource
{
    [JsonPropertyName("name")] public string Name { get; set; }
}

public interface ILocalizedResource
{
    NamedApiResource Language { get; }
}

#endregion