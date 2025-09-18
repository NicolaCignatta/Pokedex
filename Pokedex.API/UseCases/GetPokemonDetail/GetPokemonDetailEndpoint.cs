using Pokedex.Domain.Interfaces.Queries;
using Pokedex.Shared.API.Response;

namespace Pokedex.API.UseCases.GetPokemonDetail;

public static class GetPokemonDetailEndpoint
{
    public static void MapGetPokemonDetailEndpoint(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/pokemon")
            .WithTags("Pokemon");

        group.MapGet("/{name}", async (string name, IGetPokemonDetailQuery query) =>
            {
                var (isValid, errors) = GetPokemonDetailEndpointValidator.IsValid(name);
                if (isValid == false)
                {
                    return Results.BadRequest(ApiErrorResponse.CreateValidationResponse("Validation errors!","VALIDATION_ERROR", errors));
                }
                
                var result = await query.Execute(name);
                return result.Match<IResult>(
                    pokemon => Results.Ok(new GetPokemonDetailResponse(pokemon.Id,
                        pokemon.Name,
                        pokemon.Description,
                        pokemon.Habitat,
                        pokemon.IsLegendary
                    )),
                    notFound => Results.NotFound(),
                    error => Results.BadRequest(ApiErrorResponse.CreateGenericResponse(error.Message,error.Code)));
            })
            .WithName("GetPokemonDetail")
            .WithSummary("Get pokemon detail by name");
    }
}