namespace Ahva.Ceplan.Shared.ApiResponses;

public class SingleResponse<T> : ApiResponse
{
    public required T Data { get; init; }
}

public static class SingleResponse
{
    public static SingleResponse<T> From<T>(T data) => new() { Data = data };
}
