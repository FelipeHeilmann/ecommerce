using Domain.Shared;

namespace API.Extensions;

public static class ResultExtension
{
    public static IResult ToProblemDetail(this Result result)
    {
        return Results.Problem(
            statusCode: GetStatusCode(result.Error.Type),
            title: GetTitle(result.Error.Type),
            extensions: new Dictionary<string, object?>
            {
            { "error", new[] {result.Error} }
            });
    }

    static int GetStatusCode(ErrorType errorType) =>
        errorType switch
        {
            ErrorType.Validation => 400,
            ErrorType.NotFound => 404,
            ErrorType.Conflict => 409,
            _ => 500
        };

    static string GetTitle(ErrorType errorType) =>
        errorType switch
        {
            ErrorType.Validation => "Validation Error",
            ErrorType.NotFound => "Not found",
            ErrorType.Conflict => "Conflict",
            _ => "Internal error"
        };
}
