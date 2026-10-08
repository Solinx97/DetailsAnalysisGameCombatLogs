using System.Net;

namespace CombatAnalysis.EnhancedWebApp.Server.Exceptions;

public class ApiResponseException(HttpStatusCode statusCode, Uri? requestUri) 
    : Exception($"API returned {(int)statusCode} ({statusCode})")
{
    public HttpStatusCode StatusCode { get; } = statusCode;

    public Uri? RequestUri { get; } = requestUri;
}
