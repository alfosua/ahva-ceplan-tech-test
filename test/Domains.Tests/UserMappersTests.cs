using Ahva.Ceplan.Data.Entities;
using Ahva.Ceplan.Domains.Mappers;
using Microsoft.AspNetCore.Identity;

namespace Ahva.Ceplan.Domains.Tests;

public class UserMappersTests
{
    [Fact]
    public void ToFullName_ComposesLastNamesFirst()
    {
        var profile = new UserProfile
        {
            UserId = "user-1",
            DocumentType = "DNI",
            DocumentNumber = "07079879",
            FirstNames = "July Camila",
            LastNamePaternal = "Mendoza",
            LastNameMaternal = "Quispe",
        };

        Assert.Equal("Mendoza Quispe, July Camila", profile.ToFullName());
    }

    [Fact]
    public void ToFullName_WithoutLastNames_FallsBackToFirstNames()
    {
        var profile = new UserProfile
        {
            UserId = "user-1",
            DocumentType = "DNI",
            DocumentNumber = "07079879",
            FirstNames = "July Camila",
        };

        Assert.Equal("July Camila", profile.ToFullName());
    }

    [Fact]
    public void ToOutput_MapsIdentityAndProfileFields()
    {
        var user = new IdentityUser { Id = "user-1", Email = "test@minsa.gob.pe", UserName = "07079879" };
        var profile = new UserProfile
        {
            UserId = "user-1",
            DocumentType = "DNI",
            DocumentNumber = "07079879",
            FirstNames = "July Camila",
            LastNamePaternal = "Mendoza",
            LastNameMaternal = "Quispe",
            JobTitle = "Administrador de Recursos",
            Entity = "011 Ministerio de Salud",
            IsActive = true,
        };

        var output = user.ToOutput(profile);

        Assert.Equal("user-1", output.Id);
        Assert.Equal("test@minsa.gob.pe", output.Email);
        Assert.Equal("DNI", output.DocumentType);
        Assert.Equal("07079879", output.DocumentNumber);
        Assert.Equal("Mendoza Quispe, July Camila", output.FullName);
        Assert.Equal("Administrador de Recursos", output.JobTitle);
        Assert.True(output.IsActive);
    }
}
