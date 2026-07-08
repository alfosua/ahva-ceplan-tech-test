namespace Ahva.Ceplan.Contracts.Users;

public class UserOutput
{
    public required string Id { get; init; }

    public required string Email { get; init; }

    public string? UserName { get; init; }

    public string? DocumentType { get; init; }

    public string? DocumentNumber { get; init; }

    public string? FirstNames { get; init; }

    public string? LastNamePaternal { get; init; }

    public string? LastNameMaternal { get; init; }

    public string? FullName { get; init; }

    public DateOnly? BirthDate { get; init; }

    public string? Nationality { get; init; }

    public string? Sex { get; init; }

    public string? SecondaryEmail { get; init; }

    public string? MobilePhone { get; init; }

    public string? SecondaryPhoneType { get; init; }

    public string? SecondaryPhone { get; init; }

    public string? ContractType { get; init; }

    public DateOnly? HireDate { get; init; }

    public string? JobTitle { get; init; }

    public string? Entity { get; init; }

    public string? PideValidationStatus { get; init; }

    public bool IsActive { get; init; }

    public DateTime? CreatedAt { get; init; }
}
