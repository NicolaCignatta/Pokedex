namespace Pokedex.API.UseCases.GetPokemonDetail;

public record GetPokemonDetailResponse(int Id,string Name, string Description, string Habitat, bool IsLegendary);