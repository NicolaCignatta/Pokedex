namespace Pokedex.Domain.ValueObjects;

/// <summary>
/// Static class containing language-related constants.
/// </summary>
public class Habitat
{
    public static readonly Habitat Cave = new("cave", "CAVE");

    private Habitat(string name, string code)
    {
        Name = name;
        Code = code;
    }

    public string Name { get; private set; }
    public string Code { get; private set; }
}