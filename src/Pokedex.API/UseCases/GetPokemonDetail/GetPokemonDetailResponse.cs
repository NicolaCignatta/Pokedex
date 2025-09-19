namespace Pokedex.API.UseCases.GetPokemonDetail;

/// <summary>
/// GetPokemonDetailResponse represents the response returned by the GetPokemonDetail endpoint.
/// </summary>
/// <param name="Id"></param>
/// <param name="Name"></param>
/// <param name="Description"></param>
/// <param name="Habitat"></param>
/// <param name="IsLegendary"></param>
public record GetPokemonDetailResponse(int Id,string Name, string Description, string Habitat, bool IsLegendary);