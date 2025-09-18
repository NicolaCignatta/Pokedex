namespace Pokedex.API.UseCases.GetPokemonDetail;

/// <summary>
/// GetPokemonDetailEndpointValidator is responsible for validating the input parameters for the GetPokemonDetail endpoint.
/// </summary>
public static class GetPokemonDetailEndpointValidator
{
    /// <summary>
    /// Validates all parameters and returns a tuple indicating if it's valid and any associated errors.
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public static (bool, Dictionary<string, string[]>) IsValid(string name)
    {
        var errors = new Dictionary<string, string[]>();
        var isValid = IsValidName(errors, name);
        return (isValid, errors);
    }

    /// <summary>
    /// Validates the 'name' parameter.
    /// </summary>
    /// <param name="errors"></param>
    /// <param name="name"></param>
    /// <returns></returns>
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
