namespace Ahva.Ceplan.Shared.ApiResponses;

public class ListedResponse<T> : ApiResponse
{
    public required IReadOnlyList<T> Data { get; init; }

    public int Count => Data.Count;
}

public static class ListedResponse
{
    public static ListedResponse<T> From<T>(IReadOnlyList<T> data) => new() { Data = data };
}
