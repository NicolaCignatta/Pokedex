using Pokedex.Domain.Interfaces.Aggregates;

namespace Pokedex.Domain.Aggregates;

public class Pokemon : IAggregateRoot<int>
{
    #region constructors

    private Pokemon(int id, string name, string description, string habitat, bool isLegendary)
    {
        Id = id;
        Name = name;
        Description = description;
        Habitat = habitat;
        IsLegendary = isLegendary;
    }

    #endregion

    #region factories

    public static Pokemon Materialize(int id, string name, string description, string habitat, bool isLegendary)
    {
        return new Pokemon(id, name, description, habitat, isLegendary);
    }

    #endregion

    #region props

    public int Id { get; private set; }
    public string Name { get; private set; }
    public string Description { get; private set; }
    public string Habitat { get; private set; }
    public bool IsLegendary { get; private set; }

    #endregion
}