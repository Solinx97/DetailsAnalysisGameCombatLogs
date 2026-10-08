using CombatAnalysis.EnhancedWebApp.Server.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using System.Net;

namespace CombatAnalysis.EnhancedWebApp.Server.Handlers;

public class GlobalExternalAPIExceptionHandler(ILogger<GlobalExternalAPIExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext context, Exception exception, CancellationToken cancellationToken)
    {
        if (exception is not ExternalApiException apiException)
        {
            return false;
        }

        logger.LogError(
            exception,
            "External API error: {StatusCode} {Uri}",
            apiException.StatusCode,
            apiException.RequestUri);

        context.Response.StatusCode = apiException.StatusCode switch
        {
            HttpStatusCode.Unauthorized => StatusCodes.Status401Unauthorized,
            HttpStatusCode.NotFound => StatusCodes.Status404NotFound,
            HttpStatusCode.Forbidden => StatusCodes.Status403Forbidden,
            HttpStatusCode.TooManyRequests => StatusCodes.Status429TooManyRequests,
            _ => StatusCodes.Status502BadGateway
        };

        await context.Response.WriteAsJsonAsync(
            new
            {
                error = "External API request failed",
                statusCode = (int)apiException.StatusCode
            },
            cancellationToken);

        return true;
    }
}
