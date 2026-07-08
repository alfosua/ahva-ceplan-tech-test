namespace Ahva.Ceplan.Contracts.Auth;

public class LoginInput
{
    public required string DocumentType { get; init; }

    public required string DocumentNumber { get; init; }

    public required string Password { get; init; }
}
