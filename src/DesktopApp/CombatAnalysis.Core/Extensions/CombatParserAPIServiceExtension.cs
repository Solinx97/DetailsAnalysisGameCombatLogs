using CombatAnalysis.Core.Interfaces;
using Microsoft.Extensions.Logging;
using System.Net.Http.Json;

namespace CombatAnalysis.Core.Extensions;

internal static class CombatParserAPIServiceExtension
{
    public static async Task<IEnumerable<object>> LoadCombatDetailsAsync(this ICombatParserAPIService _, Type type, IHttpClientHelper httpClient, ILogger logger, string address, CancellationToken token)
    {
        try
        {
            var response = await httpClient.GetAsync(address, token);
            response.EnsureSuccessStatusCode();

            var details = await response.Content.ReadFromJsonAsync(type, cancellationToken: token);
            ArgumentNullException.ThrowIfNull(details, nameof(details));

            return (IEnumerable<object>)details;
        }
        catch (ArgumentNullException ex)
        {
            logger.LogError(ex, ex.Message);

            return [];
        }
        catch (HttpRequestException ex)
        {
            logger.LogError(ex, "HTTP request error: {Message}", ex.Message);

            return [];
        }
        catch (OperationCanceledException ex)
        {
            logger.LogError(ex, "Request was canceled by client: {Message}", ex.Message);

            return [];
        }
    }
}
