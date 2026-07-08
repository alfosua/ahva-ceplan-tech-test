using Ahva.Ceplan.Contracts.Users;
using Ahva.Ceplan.Data;
using Ahva.Ceplan.Domains.Users;
using Microsoft.EntityFrameworkCore;

namespace Ahva.Ceplan.WebApi;

public static class SeedData
{
    public const string DefaultPassword = "Ceplan#2026";

    public static async Task ApplyMigrationsAndSeed(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();

        var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        await db.Database.MigrateAsync();

        var users = scope.ServiceProvider.GetRequiredService<UserCrudService>();

        await Upsert(db, users, new UserInput
        {
            Email = "test@minsa.gob.pe",
            Password = DefaultPassword,
            DocumentType = "DNI",
            DocumentNumber = "07079879",
            FirstNames = "July Camila",
            LastNamePaternal = "Mendoza",
            LastNameMaternal = "Quispe",
            BirthDate = new DateOnly(1944, 4, 15),
            Nationality = "Peruana",
            Sex = "Femenino",
            MobilePhone = "+51 999 999 999",
            ContractType = "CAS",
            HireDate = new DateOnly(2015, 3, 9),
            JobTitle = "Administrador de Recursos",
            Entity = "011 Ministerio de Salud",
            PideValidationStatus = "Validado",
        });

        await Upsert(db, users, new UserInput
        {
            Email = "adriana.osorio@ceplan.gob.pe",
            Password = DefaultPassword,
            DocumentType = "DNI",
            DocumentNumber = "46844596",
            FirstNames = "Adriana",
            LastNamePaternal = "Osorio",
            LastNameMaternal = "Montes",
            BirthDate = new DateOnly(1990, 8, 21),
            Nationality = "Peruana",
            Sex = "Femenino",
            SecondaryEmail = "adriana.osorio.montes@gmail.com",
            MobilePhone = "+51 988 888 888",
            SecondaryPhoneType = "Fijo",
            SecondaryPhone = "+51 1 234 5678",
            ContractType = "CAS",
            HireDate = new DateOnly(2018, 6, 4),
            JobTitle = "Operador",
            Entity = "Centro Nacional de Planeamiento Estratégico",
            PideValidationStatus = "Validado",
        });
    }

    private static async Task Upsert(ApplicationDbContext db, UserCrudService users, UserInput input)
    {
        var profile = await db.UserProfiles.SingleOrDefaultAsync(p =>
            p.DocumentType == input.DocumentType && p.DocumentNumber == input.DocumentNumber);

        if (profile is null)
            await users.CreateOne(input);
        else
            await users.UpdateOne(profile.UserId, input);
    }
}
