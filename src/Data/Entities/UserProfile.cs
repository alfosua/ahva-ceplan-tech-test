using Microsoft.AspNetCore.Identity;

namespace Ahva.Ceplan.Data.Entities;

public class UserProfile
{
    public int Id { get; set; }

    public required string UserId { get; set; }

    public IdentityUser? User { get; set; }

    public string? FirstNames { get; set; }

    public string? LastNamePaternal { get; set; }

    public string? LastNameMaternal { get; set; }

    public required string DocumentType { get; set; }

    public required string DocumentNumber { get; set; }

    public DateOnly? BirthDate { get; set; }

    public string? Nationality { get; set; }

    public string? Sex { get; set; }

    public string? SecondaryEmail { get; set; }

    public string? MobilePhone { get; set; }

    public string? SecondaryPhoneType { get; set; }

    public string? SecondaryPhone { get; set; }

    public string? ContractType { get; set; }

    public DateOnly? HireDate { get; set; }

    public string? JobTitle { get; set; }

    public string? Entity { get; set; }

    public string? PideValidationStatus { get; set; }

    public bool IsActive { get; set; } = true;

    public DateTime CreatedAt { get; set; }
}
