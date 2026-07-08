namespace Ahva.Ceplan.Shared.ApiResponses;

public class LimitedResponse<T> : ApiResponse
{
    public required IReadOnlyList<T> Data { get; init; }

    public required int Limit { get; init; }

    public int Offset { get; init; }

    public int Count => Data.Count;
}
