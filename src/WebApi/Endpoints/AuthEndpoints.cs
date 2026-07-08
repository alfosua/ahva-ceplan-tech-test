using System.Security.Claims;
using Ahva.Ceplan.Contracts.Auth;
using Ahva.Ceplan.Domains.Auth;
using Ahva.Ceplan.Domains.Users;
using Ahva.Ceplan.Shared.ApiResponses;
using Ahva.Ceplan.WebApi.Auth;

namespace Ahva.Ceplan.WebApi.Endpoints;

public static class AuthEndpoints
{
    public static IEndpointRouteBuilder MapAuthEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/auth").WithTags("Auth");

        group.MapPost("/login", async (LoginInput input, AuthService auth, JwtTokenService tokens) =>
        {
            var user = await auth.ValidateCredentials(input);
            var output = new LoginOutput { Token = tokens.IssueToken(user), User = user };

            return SingleResponse.From(output);
        });

        group.MapPost("/extend", async (ClaimsPrincipal principal, UserCrudService users, JwtTokenService tokens) =>
        {
            var user = await users.GetOne(principal.GetUserId());

            return SingleResponse.From(tokens.IssueToken(user));
        }).RequireAuthorization();

        group.MapGet("/me", async (ClaimsPrincipal principal, UserCrudService users) =>
        {
            var user = await users.GetOne(principal.GetUserId());

            return SingleResponse.From(user);
        }).RequireAuthorization();

        return routes;
    }

    public static string GetUserId(this ClaimsPrincipal principal) =>
        principal.FindFirstValue(ClaimTypes.NameIdentifier)
        ?? throw new InvalidOperationException("The current principal has no user id claim.");
}
