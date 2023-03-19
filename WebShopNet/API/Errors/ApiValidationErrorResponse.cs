namespace API.Errors;

public class ApiValidationErrorResponse : ApiResponse
{
    public ApiValidationErrorResponse(int statusCode, string message = null) : base(statusCode, message)
    {
    }

    public IEnumerable<string> Errors { get; set; }
}