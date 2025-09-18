using Pokedex.Domain.Interfaces.Aggregates;

namespace Pokedex.Domain.Aggregates;

/// <summary>
/// Pokemon aggregate root representing a Pokemon entity.
/// </summary>
public class Pokemon : IAggregateRoot<int>
{
    #region constructors

    /// <summary>
    /// Pokemon constructor is private to enforce the use of the factory method.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="name"></param>
    /// <param name="description"></param>
    /// <param name="habitat"></param>
    /// <param name="isLegendary"></param>
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

    /// <summary>
    /// Materialize creates a new instance of the Pokemon aggregate from external source data.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="name"></param>
    /// <param name="description"></param>
    /// <param name="habitat"></param>
    /// <param name="isLegendary"></param>
    /// <returns></returns>
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