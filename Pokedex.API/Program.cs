using Microsoft.OpenApi.Models;
using Pokedex.API.UseCases.GetPokemonDetail;
using Pokedex.Domain;
using Pokedex.Infrastructure.Http.PokemonAPI;

namespace Pokedex.API;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddOpenApi();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddDomainServices();
        builder.Services.AddHttpClientServices(builder.Configuration);

        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Pokedex API",
                Version = "v1"
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

        app.UseHttpsRedirection();
        app.MapGetPokemonDetailEndpoint();
        app.Run();
    }
}