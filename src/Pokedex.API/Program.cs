using Microsoft.OpenApi.Models;
using Pokedex.API.UseCases.GetPokemonDetail;
using Pokedex.API.UseCases.TranslatePokemonDetail;
using Pokedex.Domain;
using Pokedex.Infrastructure.Cache.Redis;
using Pokedex.Infrastructure.Http.PokemonAPI;
using Pokedex.Infrastructure.Translation.FunTranslation;

namespace Pokedex.API;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddOpenApi();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddDomainServices();
        builder.Services.AddPokemonApiHttpClientServices(builder.Configuration);
        builder.Services.AddTranslationHttpClientServices(builder.Configuration);
        builder.Services.AddOutputCaching(builder.Configuration);


        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Pokedex API",
                Version = "v1",
                Description = "A digital Pokédex API for modern trainers: fetch Pokémon details and catch ‘em all effortlessly!"
            });
        });

        builder.Logging.ClearProviders();
        builder.Logging.AddConsole();

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        
        app.Use(async (context, next) =>
        {
            if (context.Request.Path == "/")
            {
                context.Response.Redirect("/swagger");
                return;
            }
            await next();
        });

        app.UseOutputCache();
        app.UseHttpsRedirection();
        app.MapGetPokemonDetailEndpoint();
        app.MapTranslatePokemonDetailEndpoint();
        app.Run();
    }
}