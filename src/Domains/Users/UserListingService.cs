using Ahva.Ceplan.Contracts.Users;
using Ahva.Ceplan.Data;
using Ahva.Ceplan.Data.Entities;
using Ahva.Ceplan.Domains.Mappers;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Ahva.Ceplan.Domains.Users;

public class UserListingService(ApplicationDbContext db)
{
    public async Task<IReadOnlyList<UserOutput>> GetPage(UserFilter filter)
    {
        var page = Math.Max(filter.Page, 1);
        var pageSize = Math.Clamp(filter.PageSize, 1, 100);

        var rows = await Filter(filter)
            .OrderBy(row => row.User.UserName)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return [.. rows.Select(row => row.User.ToOutput(row.Profile))];
    }

    public async Task<int> Count(UserFilter filter) => await Filter(filter).CountAsync();

    private IQueryable<UserRow> Filter(UserFilter filter)
    {
        var query =
            from user in db.Users
            join joined in db.UserProfiles on user.Id equals joined.UserId into profiles
            from profile in profiles.DefaultIfEmpty()
            select new UserRow { User = user, Profile = profile };

        if (!string.IsNullOrWhiteSpace(filter.DocumentType))
            query = query.Where(row => row.Profile != null && row.Profile.DocumentType == filter.DocumentType);

        if (!string.IsNullOrWhiteSpace(filter.DocumentNumber))
            query = query.Where(row => row.Profile != null && row.Profile.DocumentNumber == filter.DocumentNumber);

        if (!string.IsNullOrWhiteSpace(filter.Search))
            query = query.Where(row =>
                (row.User.Email != null && row.User.Email.Contains(filter.Search)) ||
                (row.Profile != null && row.Profile.FirstNames != null && row.Profile.FirstNames.Contains(filter.Search)) ||
                (row.Profile != null && row.Profile.LastNamePaternal != null && row.Profile.LastNamePaternal.Contains(filter.Search)) ||
                (row.Profile != null && row.Profile.LastNameMaternal != null && row.Profile.LastNameMaternal.Contains(filter.Search)) ||
                (row.Profile != null && row.Profile.DocumentNumber.Contains(filter.Search)));

        return query;
    }

    private class UserRow
    {
        public required IdentityUser User { get; init; }

        public UserProfile? Profile { get; init; }
    }
}
