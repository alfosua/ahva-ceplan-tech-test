using Ahva.Ceplan.Domains.Exceptions;
using Ahva.Ceplan.Shared.ApiResponses;

namespace Ahva.Ceplan.WebApi.Middleware;

public class ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
{
    public async Task Invoke(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception exception)
        {
            var (statusCode, response) = Map(exception);

            if (statusCode >= StatusCodes.Status500InternalServerError)
                logger.LogError(exception, "Unhandled exception while processing {Path}", context.Request.Path);

            context.Response.StatusCode = statusCode;
            await context.Response.WriteAsJsonAsync(response);
        }
    }

    private static (int StatusCode, ErrorResponse Response) Map(Exception exception) => exception switch
    {
        NotFoundException notFound =>
            (StatusCodes.Status404NotFound, ErrorResponse.From(notFound.Code, notFound.Message)),

        InvalidCredentialsException invalidCredentials =>
            (StatusCodes.Status401Unauthorized, ErrorResponse.From(invalidCredentials.Code, invalidCredentials.Message)),

        AccountLockedException locked =>
            (StatusCodes.Status423Locked, ErrorResponse.From(locked.Code, locked.Message)),

        ValidationException validation =>
            (StatusCodes.Status400BadRequest, new ErrorResponse
            {
                Errors =
                [
                    new ApiError { Code = validation.Code, Message = validation.Message },
                    .. validation.Details.Select(detail => new ApiError { Code = validation.Code, Message = detail }),
                ],
            }),

        CeplanException ceplan =>
            (StatusCodes.Status400BadRequest, ErrorResponse.From(ceplan.Code, ceplan.Message)),

        _ => (StatusCodes.Status500InternalServerError,
            ErrorResponse.From("internal_error", "An unexpected error occurred. Please try again later.")),
    };
}
