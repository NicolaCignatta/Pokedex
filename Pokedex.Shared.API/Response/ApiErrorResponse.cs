namespace Pokedex.Shared.API.Response;

public class ApiErrorResponse
{
    private ApiErrorResponse(string message, string code, Dictionary<string, string[]>? fields = null)
    {
        Message = message;
        Code = code;
        Fields = fields ?? new Dictionary<string, string[]>();
    }
    
    public static ApiErrorResponse CreateGenericResponse(string message, string code)
    {
        return new ApiErrorResponse(message, code);
    }
    
    public static ApiErrorResponse CreateValidationResponse(string message, string code, Dictionary<string, string[]> fields)
    {
        return new ApiErrorResponse(message, code, fields);
    
    }
    
    public string Message { get; private set; }
    public string Code { get; private set; }
    public Dictionary<string, string[]> Fields { get; private set; }
   
}