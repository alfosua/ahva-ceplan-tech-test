namespace Ahva.Ceplan.Shared.ApiResponses;

public class PagedResponse<T> : ApiResponse
{
    public required IReadOnlyList<T> Data { get; init; }

    public required int Page { get; init; }

    public required int PageSize { get; init; }

    public required int TotalCount { get; init; }

    public int TotalPages => PageSize > 0 ? (int)Math.Ceiling(TotalCount / (double)PageSize) : 0;
}
