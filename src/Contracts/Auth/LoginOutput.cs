using Ahva.Ceplan.Contracts.Users;

namespace Ahva.Ceplan.Contracts.Auth;

public class LoginOutput
{
    public required TokenOutput Token { get; init; }

    public required UserOutput User { get; init; }
}
