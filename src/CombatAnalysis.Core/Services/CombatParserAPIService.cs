using CombatAnalysis.Core.Consts;
using CombatAnalysis.Core.Enums;
using CombatAnalysis.Core.Interfaces;
using CombatAnalysis.Core.Models;
using CombatAnalysis.Core.Models.User;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System.Net.Http.Json;

namespace CombatAnalysis.Core.Services;

public class CombatParserAPIService
{
    private readonly IHttpClientHelper _httpClient;
    private readonly ILogger _logger;
    private readonly IMemoryCache _memoryCache;

    public CombatParserAPIService(IHttpClientHelper httpClient, ILogger logger, IMemoryCache memoryCache)
    {
        _httpClient = httpClient;
        _logger = logger;
        _memoryCache = memoryCache;
    }

    public void SetUpPort()
    {
        _httpClient.BaseAddress = Port.CombatParserApi;
    }

    public async Task<bool> SaveAsync(List<CombatModel> combats, CombatLogModel combatLog, LogType logType, Action<int, string, string> combatUploaded, CancellationToken cancellationToken)
    {
        try
        {
            var currentCombatNumber = 0;
            var combatsAreUploaded = false;
            if (logType == LogType.Public || logType == LogType.Private)
            {
                await SaveCombatLogByUserAsync(combatLog.Id, logType, cancellationToken);
            }

            await SetReadyForCombatLogAsync(combatLog, combats.Count, cancellationToken);

            foreach (var item in combats)
            {
                item.CombatLogId = combatLog.Id;

                var response = await _httpClient.PostAsync("Combat", JsonContent.Create(item), cancellationToken);
                response.EnsureSuccessStatusCode();

                currentCombatNumber++;
                combatUploaded(currentCombatNumber, item.DungeonName, item.Name);
            }

            combatsAreUploaded = true;

            return combatsAreUploaded;
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "HTTP request error: {Message}", ex.Message);

            return false;
        }
        catch (TaskCanceledException ex)
        {
            _logger.LogWarning(ex, "Task was canceled: {Message}", ex.Message);

            return false;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred: {Message}", ex.Message);

            return false;
        }
    }

    public async Task DeleteCombatLogByUserAsync(int id)
    {
        try
        {
            var response = await _httpClient.DeletAsync($"CombatLogByUser/{id}", CancellationToken.None);
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

    public async Task<IEnumerable<CombatLogModel>> LoadCombatLogsAsync()
    {
        try
        {
            var response = await _httpClient.GetAsync("CombatLog", CancellationToken.None);
            response.EnsureSuccessStatusCode();

            var combatLogs = await response.Content.ReadFromJsonAsync<IEnumerable<CombatLogModel>>();

            return combatLogs;
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "HTTP request error: {Message}", ex.Message);

            return null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred: {Message}", ex.Message);

            return null;
        }
    }

    public async Task<IEnumerable<CombatLogModel>> LoadCombatLogsAsync(List<int> combatLogsId)
    {
        try
        {
            var combatLogs = new List<CombatLogModel>();
            foreach (var item in combatLogsId)
            {
                var response = await _httpClient.GetAsync($"CombatLog/{item}", CancellationToken.None);
                response.EnsureSuccessStatusCode();

                var combatLog = await response.Content.ReadFromJsonAsync<CombatLogModel>();

                combatLogs.Add(combatLog);
            }

            return combatLogs;
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "HTTP request error: {Message}", ex.Message);

            return null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred: {Message}", ex.Message);

            return null;
        }
    }

    public async Task<IEnumerable<CombatLogByUserModel>> LoadCombatLogsByUserAsync()
    {
        try
        {
            var user = _memoryCache.Get<AppUserModel>(nameof(MemoryCacheValue.User));
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            var response = await _httpClient.GetAsync($"CombatLogByUser/getByUserId/{user.Id}", CancellationToken.None);
            response.EnsureSuccessStatusCode();

            var combatLogsByUser = await response.Content.ReadFromJsonAsync<IEnumerable<CombatLogByUserModel>>();

            return combatLogsByUser;
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "User is not logged in: {Message}", ex.Message);

            return null;
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "HTTP request error: {Message}", ex.Message);

            return null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred: {Message}", ex.Message);

            return null;
        }
    }

    public async Task<IEnumerable<CombatModel>> LoadCombatsAsync(int combatLogId)
    {
        try
        {
            var response = await _httpClient.GetAsync($"Combat/getByCombatLogId/{combatLogId}", CancellationToken.None);
            response.EnsureSuccessStatusCode();

            var combats = await response.Content.ReadFromJsonAsync<IEnumerable<CombatModel>>();

            return combats;
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "HTTP request error: {Message}", ex.Message);

            return null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred: {Message}", ex.Message);

            return null;
        }
    }

    public async Task<IEnumerable<CombatPlayerModel>> LoadCombatPlayersAsync(int combatId)
    {
        try
        {
            var response = await _httpClient.GetAsync($"CombatPlayer/getByCombatId/{combatId}", CancellationToken.None);
            response.EnsureSuccessStatusCode();

            var combatPlayers = await response.Content.ReadFromJsonAsync<IEnumerable<CombatPlayerModel>>();
            foreach (var item in combatPlayers)
            {
                var playerParseInfo = await GetPlayerParseInfoAsync(item.Id);
                item.PlayerParseInfo = playerParseInfo;
            }

            return combatPlayers;
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "HTTP request error: {Message}", ex.Message);

            return null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred: {Message}", ex.Message);

            return null;
        }
    }

    public async Task<int> LoadCountAsync(string address)
    {
        try
        {
            var response = await _httpClient.GetAsync(address, CancellationToken.None);
            response.EnsureSuccessStatusCode();

            var details = await response.Content.ReadFromJsonAsync<int>();

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

    public async Task<IEnumerable<T>> LoadCombatDetailsAsync<T>(string address)
        where T : class
    {
        try
        {
            var response = await _httpClient.GetAsync(address, CancellationToken.None);
            response.EnsureSuccessStatusCode();

            var details = await response.Content.ReadFromJsonAsync<IEnumerable<T>>();

            return details;
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "HTTP request error: {Message}", ex.Message);

            return null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred: {Message}", ex.Message);

            return null;
        }
    }

    public async Task<CombatLogModel> SaveCombatLogAsync(List<CombatModel> combats, CancellationToken cancellationToken)
    {
        try
        {
            var dungeonNames = combats
                 .GroupBy(group => group.DungeonName)
                 .Select(select => select.Key)
                 .Where(name => !string.IsNullOrEmpty(name))
                 .ToList();

            var response = await _httpClient.PostAsync("CombatLog", JsonContent.Create(dungeonNames), cancellationToken);
            response.EnsureSuccessStatusCode();

            var createdCombatLog = await response.Content.ReadFromJsonAsync<CombatLogModel>();

            return createdCombatLog;
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "HTTP request error: {Message}", ex.Message);

            return null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred: {Message}", ex.Message);

            return null;
        }
    }

    private async Task SaveCombatLogByUserAsync(int combatLogId, LogType logType, CancellationToken cancellationToken)
    {
        try
        {
            var user = _memoryCache.Get<AppUserModel>(nameof(MemoryCacheValue.User));
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            var combatLogByUser = new CombatLogByUserModel
            {
                AppUserId = user.Id,
                CombatLogId = combatLogId,
                PersonalLogType = (int)logType
            };

            var response = await _httpClient.PostAsync("CombatLogByUser", JsonContent.Create(combatLogByUser), cancellationToken);
            response.EnsureSuccessStatusCode();
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "User is not logged in: {Message}", ex.Message);
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "HTTP request error: {Message}", ex.Message);
        }
        catch (TaskCanceledException ex)
        {
            _logger.LogWarning(ex, "Task was canceled: {Message}", ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred: {Message}", ex.Message);
        }
    }

    private async Task SetReadyForCombatLogAsync(CombatLogModel combatLog, int numberCombats, CancellationToken cancellationToken)
    {
        try
        {
            combatLog.IsReady = true;
            combatLog.NumberReadyCombats = 0;
            combatLog.CombatsInQueue = numberCombats;

            var response = await _httpClient.PutAsync("CombatLog", JsonContent.Create(combatLog), cancellationToken);
            response.EnsureSuccessStatusCode();
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "HTTP request error: {Message}", ex.Message);
        }
        catch (TaskCanceledException ex)
        {
            _logger.LogWarning(ex, "Task was canceled: {Message}", ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred: {Message}", ex.Message);
        }
    }

    private async Task<PlayerParseInfoModel> GetPlayerParseInfoAsync(int combatPlayerId)
    {
        try
        {
            var response = await _httpClient.GetAsync($"PlayerParseInfo/getByCombatPlayerId/{combatPlayerId}", CancellationToken.None);
            response.EnsureSuccessStatusCode();

            var playerParseInfoCollection = await response.Content.ReadFromJsonAsync<IEnumerable<PlayerParseInfoModel>>();
            var playerInfo = playerParseInfoCollection.FirstOrDefault();

            return playerInfo;
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "HTTP request error: {Message}", ex.Message);

            return null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred: {Message}", ex.Message);

            return null;
        }
    }
}
