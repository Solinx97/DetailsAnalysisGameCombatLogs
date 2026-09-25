using System.Net;

namespace CombatAnalysis.EnhancedWebApp.Server.Exceptions;

public class ExternalApiException(HttpStatusCode statusCode, Uri? requestUri, string? responseBody) 
    : Exception($"External API returned {(int)statusCode} ({statusCode}).")
{
    public HttpStatusCode StatusCode { get; } = statusCode;

    public Uri? RequestUri { get; } = requestUri;

    public string? ResponseBody { get; } = responseBody;
}
