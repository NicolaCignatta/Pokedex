using Pokedex.Domain.Shared;

namespace Pokedex.Infrastructure.Http.PokemonAPI.HttpResponses;

using System.Collections.Generic;
using System.Text.Json.Serialization;

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

    public string? DefaultName => Names?.GetLocalizedValue(Language.DefaultCulture)?.Name;
    public string? HabitatName => Habitat?.Name;
    public string? DefaultFlavorText => FlavorTextEntries.GetLocalizedValue(Language.DefaultCulture)?.FlavorText;
}

public class FlavorTextEntry: ILocalizedResource
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