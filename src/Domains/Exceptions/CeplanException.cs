namespace Ahva.Ceplan.Domains.Exceptions;

public class CeplanException(string code, string message) : Exception(message)
{
    public string Code { get; } = code;
}

public class NotFoundException(string message) : CeplanException("not_found", message);

public class ValidationException(string message) : CeplanException("validation_error", message)
{
    public ValidationException(IReadOnlyList<string> details) : this("One or more validation errors occurred.")
    {
        Details = details;
    }

    public IReadOnlyList<string> Details { get; } = [];
}

public class InvalidCredentialsException() : CeplanException("invalid_credentials", "The user or password is incorrect.");

public class AccountLockedException() : CeplanException(
    "account_locked",
    "Has excedido el número máximo de intentos fallidos (5). Por seguridad, tu cuenta ha sido bloqueada durante 15 minutos. Intenta nuevamente más tarde.");
