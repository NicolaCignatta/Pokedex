using Pokedex.Domain.Interfaces.Aggregates;
using Pokedex.Domain.ValueObjects;

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
    /// <param name="habitatName"></param>
    /// <param name="isLegendary"></param>
    private Pokemon(int id, string name, string description, string habitatName, bool isLegendary)
    {
        Id = id;
        Name = name;
        Description = description;
        HabitatName = habitatName;
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
    /// <param name="habitatName"></param>
    /// <param name="isLegendary"></param>
    /// <returns></returns>
    public static Pokemon? Materialize(int id, string name, string description, string habitatName, bool isLegendary)
    {
        if (id <= 0 || string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(description) ||
            string.IsNullOrWhiteSpace(habitatName))
            return null;
        return new Pokemon(id, name, description, habitatName, isLegendary);
    }

    #endregion

    #region props

    public int Id { get; private set; }
    public string Name { get; private set; }
    public string Description { get; private set; }
    public string HabitatName { get; private set; }
    public bool IsLegendary { get; private set; }
    public Language MyLanguage => DetectLanguage();

    #endregion

    #region commands

    public void TranslateDescription(string descriptionTranslated)
    {
        if (string.IsNullOrWhiteSpace(descriptionTranslated) ||
            descriptionTranslated.Equals(Description, StringComparison.OrdinalIgnoreCase))
            return;
        Description = descriptionTranslated;
    }

    #endregion

    #region language methods

    private Language DetectLanguage()
    {
        if (IsLegendary || HabitatName.Equals(Habitat.Cave.Name, StringComparison.OrdinalIgnoreCase))
        {
            return Language.Yoda;
        }

        return Language.Shakespeare;
    }

    #endregion
}