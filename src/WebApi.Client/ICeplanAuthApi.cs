using Ahva.Ceplan.Contracts.Auth;
using Ahva.Ceplan.Contracts.Users;
using Ahva.Ceplan.Shared.ApiResponses;
using Refit;

namespace Ahva.Ceplan.WebApi.Client;

public interface ICeplanAuthApi
{
    [Post("/api/auth/login")]
    Task<SingleResponse<LoginOutput>> Login(LoginInput input);

    [Post("/api/auth/extend")]
    Task<SingleResponse<TokenOutput>> Extend([Authorize] string token);

    [Get("/api/auth/me")]
    Task<SingleResponse<UserOutput>> Me([Authorize] string token);
}
