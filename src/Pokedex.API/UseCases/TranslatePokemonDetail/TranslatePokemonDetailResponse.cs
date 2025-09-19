namespace Pokedex.API.UseCases.TranslatePokemonDetail;

/// <summary>
/// TranslatePokemonDetail represents the response returned by the Translate Pokemon endpoint.
/// </summary>
/// <param name="Id"></param>
/// <param name="Name"></param>
/// <param name="Description"></param>
/// <param name="Habitat"></param>
/// <param name="IsLegendary"></param>
public record TranslatePokemonDetailResponse(int Id,string Name, string Description, string Habitat, bool IsLegendary);