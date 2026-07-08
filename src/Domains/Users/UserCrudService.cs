using Ahva.Ceplan.Contracts.Users;
using Ahva.Ceplan.Data;
using Ahva.Ceplan.Data.Entities;
using Ahva.Ceplan.Domains.Exceptions;
using Ahva.Ceplan.Domains.Mappers;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Ahva.Ceplan.Domains.Users;

public class UserCrudService(UserManager<IdentityUser> userManager, ApplicationDbContext db)
{
    public async Task<UserOutput> CreateOne(UserInput input)
    {
        if (string.IsNullOrWhiteSpace(input.Password))
            throw new ValidationException("A password is required to create a user.");

        var user = new IdentityUser
        {
            UserName = input.DocumentNumber,
            Email = input.Email,
            EmailConfirmed = true,
        };

        var result = await userManager.CreateAsync(user, input.Password);
        if (!result.Succeeded)
            throw new ValidationException([.. result.Errors.Select(e => e.Description)]);

        var profile = new UserProfile
        {
            UserId = user.Id,
            DocumentType = input.DocumentType,
            DocumentNumber = input.DocumentNumber,
            CreatedAt = DateTime.UtcNow,
        };
        profile.ApplyInput(input);

        db.UserProfiles.Add(profile);
        await db.SaveChangesAsync();

        return user.ToOutput(profile);
    }

    public async Task<UserOutput> GetOne(string id)
    {
        var user = await userManager.FindByIdAsync(id)
            ?? throw new NotFoundException($"User '{id}' was not found.");

        var profile = await db.UserProfiles.SingleOrDefaultAsync(p => p.UserId == id);

        return user.ToOutput(profile);
    }

    public async Task<UserOutput> UpdateOne(string id, UserInput input)
    {
        var user = await userManager.FindByIdAsync(id)
            ?? throw new NotFoundException($"User '{id}' was not found.");

        user.Email = input.Email;
        user.UserName = input.DocumentNumber;

        var result = await userManager.UpdateAsync(user);
        if (!result.Succeeded)
            throw new ValidationException([.. result.Errors.Select(e => e.Description)]);

        var profile = await db.UserProfiles.SingleOrDefaultAsync(p => p.UserId == id);
        if (profile is null)
        {
            profile = new UserProfile
            {
                UserId = id,
                DocumentType = input.DocumentType,
                DocumentNumber = input.DocumentNumber,
                CreatedAt = DateTime.UtcNow,
            };
            db.UserProfiles.Add(profile);
        }

        profile.ApplyInput(input);
        await db.SaveChangesAsync();

        return user.ToOutput(profile);
    }

    public async Task<UserOutput> DeleteOne(string id)
    {
        var user = await userManager.FindByIdAsync(id)
            ?? throw new NotFoundException($"User '{id}' was not found.");

        var profile = await db.UserProfiles.SingleOrDefaultAsync(p => p.UserId == id);
        var output = user.ToOutput(profile);

        var result = await userManager.DeleteAsync(user);
        if (!result.Succeeded)
            throw new ValidationException([.. result.Errors.Select(e => e.Description)]);

        return output;
    }
}
