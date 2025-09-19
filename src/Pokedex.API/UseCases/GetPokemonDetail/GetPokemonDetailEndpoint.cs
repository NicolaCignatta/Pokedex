using Microsoft.AspNetCore.Mvc;
using Pokedex.Domain.Interfaces.Queries;
using Pokedex.Shared.API.Response;

namespace Pokedex.API.UseCases.GetPokemonDetail;

public static class GetPokemonDetailEndpoint
{
    public static void MapGetPokemonDetailEndpoint(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/pokemon")
            .WithTags("Pokemon Endpoints");

        group.MapGet("/{name}", async (string name, 
                [FromServices]IGetPokemonDetailQuery query, 
                [FromServices]ILogger<GetPokemonDetailEndpointLogger> logger,
                CancellationToken cancellationToken = default) =>
            {
                var (isValid, errors) = GetPokemonDetailEndpointValidator.IsValid(name);
                if (isValid == false)
                {
                    logger.LogInformation("Validation errors: {Errors}", string.Join(", ", errors));
                    return Results.BadRequest(
                        ApiErrorResponse.CreateValidationResponse("Validation errors!", "VALIDATION_ERROR", errors));
                }

                logger.LogInformation("Fetching details for pokemon: {Name}", name);
                var result = await query.Execute(name,cancellationToken);
                return result.Match<IResult>(
                    pokemon =>
                    {
                        logger.LogInformation("Pokemon {Name} found with ID {Id}", pokemon.Name, pokemon.Id);
                        return Results.Ok(new GetPokemonDetailResponse(pokemon.Id,
                            pokemon.Name,
                            pokemon.Description,
                            pokemon.HabitatName,
                            pokemon.IsLegendary
                        ));
                    },
                    notFound =>
                    {
                        logger.LogInformation("Pokemon {Name} not found", name);
                        return Results.NotFound();
                    },
                    error =>
                    {
                        logger.LogInformation("Error fetching pokemon {Name}: {ErrorMessage}. Code: {Code}", name, error.Message, error.Code);
                        return Results.BadRequest(ApiErrorResponse.CreateGenericResponse(error.Message, error.Code));
                    });
            })
            .CacheOutput("PokemonEndpointsPolicy")
            .WithName("GetPokemonDetail API")
            .WithSummary("Get pokemon detail by name");
    }
}

public class GetPokemonDetailEndpointLogger{}