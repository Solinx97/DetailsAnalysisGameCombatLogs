using CombatAnalysis.EnhancedWebApp.Server.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using System.Net;

namespace CombatAnalysis.EnhancedWebApp.Server.Handlers;

public class GlobalAPIExceptionHandler(ILogger<GlobalExternalAPIExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext context, Exception exception, CancellationToken cancellationToken)
    {
        if (exception is not ApiResponseException apiException)
        {
            return false;
        }
        else if (apiException.StatusCode == HttpStatusCode.NotFound)
        {
            context.Response.StatusCode = StatusCodes.Status404NotFound;

            await context.Response.WriteAsJsonAsync(
                new
                {
                    error = "Requested resouce not found",
                    statusCode = (int)apiException.StatusCode
                },
                cancellationToken);

            return true;
        }

        logger.LogError(
            exception,
            "API error: {StatusCode} {Uri}",
            apiException.StatusCode,
            apiException.RequestUri);

        context.Response.StatusCode = apiException.StatusCode switch
        {
            HttpStatusCode.Unauthorized => StatusCodes.Status401Unauthorized,
            HttpStatusCode.Forbidden => StatusCodes.Status403Forbidden,
            HttpStatusCode.TooManyRequests => StatusCodes.Status429TooManyRequests,
            _ => StatusCodes.Status502BadGateway
        };

        await context.Response.WriteAsJsonAsync(
            new
            {
                error = "API request failed",
                statusCode = (int)apiException.StatusCode
            },
            cancellationToken);

        return true;
    }
}
