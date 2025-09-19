namespace Pokedex.Domain.ValueObjects;

/// <summary>
/// Static class containing language-related constants.
/// </summary>
public class Language
{
    public string Name { get; private set; }
    public string Code { get; private set; }

    private Language(string name, string code)
    {
        Name = name;
        Code = code;
    }

    public static readonly Language English = new("English", "en");
    public static readonly Language Yoda = new("Yoda", "yoda");
    public static readonly Language Shakespeare = new("Shakespeare", "shakespeare");

   public static readonly string DefaultCulture = English.Code;
}