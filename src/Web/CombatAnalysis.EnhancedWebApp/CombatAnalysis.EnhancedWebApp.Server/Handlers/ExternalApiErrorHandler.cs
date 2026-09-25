using CombatAnalysis.EnhancedWebApp.Server.Exceptions;

namespace CombatAnalysis.EnhancedWebApp.Server.Handlers;

public class ExternalApiErrorHandler(ILogger<ExternalApiErrorHandler> logger) : DelegatingHandler
{
    private readonly ILogger<ExternalApiErrorHandler> _logger = logger;

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var response = await base.SendAsync(request, cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            var body = await response.Content.ReadAsStringAsync(
                cancellationToken);

            _logger.LogWarning("External API returned {StatusCode} for {Method} {Url}. Body: {Body}",
                (int)response.StatusCode,
                request.Method,
                request.RequestUri,
                body);

            throw new ExternalApiException(
                response.StatusCode,
                request.RequestUri,
                body);
        }

        return response;
    }
}
