namespace Ahva.Ceplan.Contracts.Users;

public class UserFilter
{
    public string? Search { get; init; }

    public string? DocumentType { get; init; }

    public string? DocumentNumber { get; init; }

    public int Page { get; init; } = 1;

    public int PageSize { get; init; } = 10;
}
