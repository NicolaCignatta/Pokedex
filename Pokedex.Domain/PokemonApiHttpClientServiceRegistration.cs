using Microsoft.Extensions.DependencyInjection;
using Pokedex.Domain.Interfaces.Queries;
using Pokedex.Domain.Queries.GetPokemonDetailQuery;

namespace Pokedex.Domain;

public static class DomainRegistration
{
    public static IServiceCollection AddDomainServices(this IServiceCollection services)
    {
        services.AddScoped<IGetPokemonDetailQuery, GetPokemonDetailQuery>();

        return services;
    }
}