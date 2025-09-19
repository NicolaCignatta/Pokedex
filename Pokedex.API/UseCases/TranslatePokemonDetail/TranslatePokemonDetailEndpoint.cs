using Microsoft.AspNetCore.Mvc;
using Pokedex.API.UseCases.GetPokemonDetail;
using Pokedex.Domain.Interfaces.Queries;
using Pokedex.Shared.API.Response;

namespace Pokedex.API.UseCases.TranslatePokemonDetail;

public static class TranslatePokemonDetailEndpoint
{
    public static void MapTranslatePokemonDetailEndpoint(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/pokemon")
            .WithTags("Translate Endpoints");

        group.MapGet("/translate/{name}", async (string name, 
                [FromServices]ITranslatePokemonInformationQuery query, 
                [FromServices]ILogger<TranslatePokemonEndpointLogger> logger,
                CancellationToken cancellationToken = default) =>
            {
                var (isValid, errors) = TranslatePokemonDetailEndpointValidator.IsValid(name);
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
            .CacheOutput("TranslationEndpointsPolicy")
            .WithName("Translation API")
            .WithSummary("Translate Pokemon Details");
    }
}

public class TranslatePokemonEndpointLogger{}