using Ahva.Ceplan.Contracts.Auth;
using Ahva.Ceplan.Contracts.Users;
using Ahva.Ceplan.Data;
using Ahva.Ceplan.Domains.Exceptions;
using Ahva.Ceplan.Domains.Mappers;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Ahva.Ceplan.Domains.Auth;

public class AuthService(UserManager<IdentityUser> userManager, ApplicationDbContext db)
{
    public async Task<UserOutput> ValidateCredentials(LoginInput input)
    {
        var profile = await db.UserProfiles.SingleOrDefaultAsync(p =>
            p.DocumentType == input.DocumentType && p.DocumentNumber == input.DocumentNumber);

        var user = profile is null ? null : await userManager.FindByIdAsync(profile.UserId);
        if (user is null)
            throw new InvalidCredentialsException();

        if (await userManager.IsLockedOutAsync(user))
            throw new AccountLockedException();

        if (!await userManager.CheckPasswordAsync(user, input.Password))
        {
            await userManager.AccessFailedAsync(user);

            if (await userManager.IsLockedOutAsync(user))
                throw new AccountLockedException();

            throw new InvalidCredentialsException();
        }

        await userManager.ResetAccessFailedCountAsync(user);

        return user.ToOutput(profile);
    }
}
