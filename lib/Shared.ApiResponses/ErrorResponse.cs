namespace Ahva.Ceplan.Shared.ApiResponses;

public class ErrorResponse : ApiResponse
{
    public ErrorResponse() => Ok = false;

    public required IReadOnlyList<ApiError> Errors { get; init; }

    public static ErrorResponse From(string code, string message) =>
        new() { Errors = [new ApiError { Code = code, Message = message }] };

    public static ErrorResponse From(IEnumerable<ApiError> errors) =>
        new() { Errors = [.. errors] };
}

public class ApiError
{
    public required string Code { get; init; }

    public required string Message { get; init; }

    public string? Detail { get; init; }
}
