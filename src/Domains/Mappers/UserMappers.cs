using Ahva.Ceplan.Contracts.Users;
using Ahva.Ceplan.Data.Entities;
using Microsoft.AspNetCore.Identity;

namespace Ahva.Ceplan.Domains.Mappers;

public static class UserMappers
{
    public static UserOutput ToOutput(this IdentityUser user, UserProfile? profile = null) => new()
    {
        Id = user.Id,
        Email = user.Email ?? string.Empty,
        UserName = user.UserName,
        DocumentType = profile?.DocumentType,
        DocumentNumber = profile?.DocumentNumber,
        FirstNames = profile?.FirstNames,
        LastNamePaternal = profile?.LastNamePaternal,
        LastNameMaternal = profile?.LastNameMaternal,
        FullName = profile?.ToFullName(),
        BirthDate = profile?.BirthDate,
        Nationality = profile?.Nationality,
        Sex = profile?.Sex,
        SecondaryEmail = profile?.SecondaryEmail,
        MobilePhone = profile?.MobilePhone,
        SecondaryPhoneType = profile?.SecondaryPhoneType,
        SecondaryPhone = profile?.SecondaryPhone,
        ContractType = profile?.ContractType,
        HireDate = profile?.HireDate,
        JobTitle = profile?.JobTitle,
        Entity = profile?.Entity,
        PideValidationStatus = profile?.PideValidationStatus,
        IsActive = profile?.IsActive ?? true,
        CreatedAt = profile?.CreatedAt,
    };

    public static string ToFullName(this UserProfile profile)
    {
        var lastNames = string.Join(' ', new[] { profile.LastNamePaternal, profile.LastNameMaternal }
            .Where(part => !string.IsNullOrWhiteSpace(part)));

        return string.IsNullOrWhiteSpace(lastNames)
            ? profile.FirstNames ?? string.Empty
            : $"{lastNames}, {profile.FirstNames}";
    }

    public static void ApplyInput(this UserProfile profile, UserInput input)
    {
        profile.DocumentType = input.DocumentType;
        profile.DocumentNumber = input.DocumentNumber;
        profile.FirstNames = input.FirstNames;
        profile.LastNamePaternal = input.LastNamePaternal;
        profile.LastNameMaternal = input.LastNameMaternal;
        profile.BirthDate = input.BirthDate;
        profile.Nationality = input.Nationality;
        profile.Sex = input.Sex;
        profile.SecondaryEmail = input.SecondaryEmail;
        profile.MobilePhone = input.MobilePhone;
        profile.SecondaryPhoneType = input.SecondaryPhoneType;
        profile.SecondaryPhone = input.SecondaryPhone;
        profile.ContractType = input.ContractType;
        profile.HireDate = input.HireDate;
        profile.JobTitle = input.JobTitle;
        profile.Entity = input.Entity;
        profile.PideValidationStatus = input.PideValidationStatus;
    }
}
