namespace Ahva.Ceplan.Contracts.Auth;

public class TokenOutput
{
    public required string AccessToken { get; init; }

    public required string TokenType { get; init; }

    public required int ExpiresIn { get; init; }
}
