using Ahva.Ceplan.Contracts.Users;
using Ahva.Ceplan.Shared.ApiResponses;
using Refit;

namespace Ahva.Ceplan.WebApi.Client;

public interface ICeplanUsersApi
{
    [Get("/api/users")]
    Task<PagedResponse<UserOutput>> GetPage([Query] UserFilter filter, [Authorize] string token);

    [Get("/api/users/{id}")]
    Task<SingleResponse<UserOutput>> GetOne(string id, [Authorize] string token);

    [Post("/api/users")]
    Task<SingleResponse<UserOutput>> CreateOne(UserInput input, [Authorize] string token);

    [Put("/api/users/{id}")]
    Task<SingleResponse<UserOutput>> UpdateOne(string id, UserInput input, [Authorize] string token);

    [Delete("/api/users/{id}")]
    Task<SingleResponse<UserOutput>> DeleteOne(string id, [Authorize] string token);
}
