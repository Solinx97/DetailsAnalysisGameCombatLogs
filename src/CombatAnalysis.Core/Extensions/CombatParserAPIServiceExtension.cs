using CombatAnalysis.Core.Interfaces;
using Microsoft.Extensions.Logging;
using System.Net.Http.Json;

namespace CombatAnalysis.Core.Extensions;

internal static class CombatParserAPIServiceExtension
{
    public static async Task<IEnumerable<T>> LoadCombatDetailsAsync<T>(this ICombatParserAPIService _, IHttpClientHelper httpClient, ILogger logger, string address)
        where T : class
    {
        try
        {
            var response = await httpClient.GetAsync(address, CancellationToken.None);
            response.EnsureSuccessStatusCode();

            var details = await response.Content.ReadFromJsonAsync<IEnumerable<T>>();
            if (details == null)
            {
                throw new ArgumentNullException(nameof(details));
            }

            return details;
        }
        catch (ArgumentNullException ex)
        {
            logger.LogError(ex, ex.Message);

            return new List<T>();
        }
        catch (HttpRequestException ex)
        {
            logger.LogError(ex, "HTTP request error: {Message}", ex.Message);

            return new List<T>();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An unexpected error occurred: {Message}", ex.Message);

            return new List<T>();
        }
    }
}
