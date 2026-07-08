using Ahva.Ceplan.Contracts.Users;
using Ahva.Ceplan.Domains.Users;
using Ahva.Ceplan.Shared.ApiResponses;

namespace Ahva.Ceplan.WebApi.Endpoints;

public static class UserEndpoints
{
    public static IEndpointRouteBuilder MapUserEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/users").WithTags("Users").RequireAuthorization();

        group.MapGet("/", async ([AsParameters] UserFilter filter, UserListingService listing) =>
        {
            var users = await listing.GetPage(filter);
            var total = await listing.Count(filter);

            return new PagedResponse<UserOutput>
            {
                Data = users,
                Page = filter.Page,
                PageSize = filter.PageSize,
                TotalCount = total,
            };
        });

        group.MapGet("/{id}", async (string id, UserCrudService users) =>
            SingleResponse.From(await users.GetOne(id)));

        group.MapPost("/", async (UserInput input, UserCrudService users) =>
            SingleResponse.From(await users.CreateOne(input)));

        group.MapPut("/{id}", async (string id, UserInput input, UserCrudService users) =>
            SingleResponse.From(await users.UpdateOne(id, input)));

        group.MapDelete("/{id}", async (string id, UserCrudService users) =>
            SingleResponse.From(await users.DeleteOne(id)));

        return routes;
    }
}
