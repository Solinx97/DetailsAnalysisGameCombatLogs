using CombatAnalysis.Core.Consts;
using CombatAnalysis.Core.Interfaces;
using CombatAnalysis.Core.Models.GameLogs;
using Microsoft.Extensions.Logging;
using System.Net.Http.Json;

namespace CombatAnalysis.Core.Services;

internal class CombatParserAPIService : ICombatParserAPIService
{
    private readonly IHttpClientHelper _httpClient;
    private readonly ILogger _logger;

    public CombatParserAPIService(IHttpClientHelper httpClient, ILogger logger)
    {
        _httpClient = httpClient;
        _logger = logger;

        _httpClient.BaseAddress = API.CombatParserApi;
    }

    public async Task DeleteCombatLogByUserAsync(int id, CancellationToken cancellationToken)
    {
        try
        {
            var response = await _httpClient.DeletAsync($"CombatLog/{id}", cancellationToken);
            response.EnsureSuccessStatusCode();
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "HTTP request error: {Message}", ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred: {Message}", ex.Message);
        }
    }

    public async Task<IEnumerable<CombatLogModel>> LoadCombatLogsAsync(CancellationToken cancellationToken)
    {
        try
        {
            var response = await _httpClient.GetAsync("CombatLog", cancellationToken);
            response.EnsureSuccessStatusCode();

            var combatLogs = await response.Content.ReadFromJsonAsync<IEnumerable<CombatLogModel>>();
            ArgumentNullException.ThrowIfNull(combatLogs, nameof(combatLogs));

            return combatLogs;
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, ex.Message);

            return [];
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "HTTP request error: {Message}", ex.Message);

            return [];
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred: {Message}", ex.Message);

            return [];
        }
    }

    public async Task<IEnumerable<CombatModel>> LoadCombatsAsync(int combatLogId, CancellationToken cancellationToken)
    {
        try
        {
            var response = await _httpClient.GetAsync($"Combat/getByCombatLogId/{combatLogId}", cancellationToken);
            response.EnsureSuccessStatusCode();

            var combats = await response.Content.ReadFromJsonAsync<IEnumerable<CombatModel>>(cancellationToken);
            ArgumentNullException.ThrowIfNull(combats, nameof(combats));

            return combats;
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, ex.Message);

            return [];
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "HTTP request error: {Message}", ex.Message);

            return [];
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred: {Message}", ex.Message);

            return [];
        }
    }

    public async Task<IEnumerable<CombatPlayerModel>> LoadCombatPlayersAsync(int combatId, CancellationToken cancellationToke)
    {
        try
        {
            var response = await _httpClient.GetAsync($"CombatPlayer/getByCombatId/{combatId}", cancellationToke);
            response.EnsureSuccessStatusCode();

            var combatPlayers = await response.Content.ReadFromJsonAsync<IEnumerable<CombatPlayerModel>>(cancellationToke);
            ArgumentNullException.ThrowIfNull(combatPlayers, nameof(combatPlayers));

            return combatPlayers;
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "Some arguments is null: {Message}", ex.Message);

            return [];
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "HTTP request error: {Message}", ex.Message);

            return [];
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred: {Message}", ex.Message);

            return [];
        }
    }

    public async Task<int> LoadCountAsync(string address, CancellationToken cancellationToken)
    {
        try
        {
            var response = await _httpClient.GetAsync(address, cancellationToken);
            response.EnsureSuccessStatusCode();

            var details = await response.Content.ReadFromJsonAsync<int>(cancellationToken);

            return details;
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "HTTP request error: {Message}", ex.Message);

            return 0;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred: {Message}", ex.Message);

            return 0;
        }
    }
}
