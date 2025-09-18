namespace Pokedex.API.UseCases.GetPokemonDetail;

public static class GetPokemonDetailEndpointValidator
{
    public static (bool, Dictionary<string, string[]>) IsValid(string name)
    {
        var errors = new Dictionary<string, string[]>();
        var isValid = IsValidName(errors, name);
        return (isValid, errors);
    }

    private static bool IsValidName(Dictionary<string, string[]> errors, string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            errors.Add(nameof(name), [$"{nameof(name)} is required"]);
            return false;
        }
        return true;
    }
}
