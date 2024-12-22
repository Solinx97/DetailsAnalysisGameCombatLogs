using CombatAnalysis.Core.Consts;
using CombatAnalysis.Core.Enums;
using CombatAnalysis.Core.Interfaces;
using CombatAnalysis.Core.Models;
using CombatAnalysis.Core.Models.User;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System.Net.Http.Json;
using System.Text;

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

    public async Task<bool> SaveAsync(List<CombatModel> combats, CombatLogModel combatLog, Action<int, string, string> combatUploaded, CancellationToken cancellationToken)
    {
        try
        {
            var currentCombatNumber = 0;
            var combatsAreUploaded = false;

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
            if (combatLogs == null)
            {
                throw new ArgumentNullException(nameof(combatLogs));
            }

            return combatLogs;
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, ex.Message);

            return new List<CombatLogModel>();
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "HTTP request error: {Message}", ex.Message);

            return new List<CombatLogModel>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred: {Message}", ex.Message);

            return new List<CombatLogModel>();
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
                if (combatLog == null)
                {
                    throw new ArgumentNullException(nameof(combatLog));
                }

                combatLogs.Add(combatLog);
            }

            return combatLogs;
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, ex.Message);

            return new List<CombatLogModel>();
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "HTTP request error: {Message}", ex.Message);

            return new List<CombatLogModel>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred: {Message}", ex.Message);

            return new List<CombatLogModel>();
        }
    }

    public async Task<IEnumerable<CombatModel>> LoadCombatsAsync(int combatLogId)
    {
        try
        {
            var response = await _httpClient.GetAsync($"Combat/getByCombatLogId/{combatLogId}", CancellationToken.None);
            response.EnsureSuccessStatusCode();

            var combats = await response.Content.ReadFromJsonAsync<IEnumerable<CombatModel>>();
            if (combats == null)
            {
                throw new ArgumentNullException(nameof(combats));
            }

            return combats;
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, ex.Message);

            return new List<CombatModel>();
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "HTTP request error: {Message}", ex.Message);

            return new List<CombatModel>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred: {Message}", ex.Message);

            return new List<CombatModel>();
        }
    }

    public async Task<IEnumerable<CombatPlayerModel>> LoadCombatPlayersAsync(int combatId)
    {
        try
        {
            var response = await _httpClient.GetAsync($"CombatPlayer/getByCombatId/{combatId}", CancellationToken.None);
            response.EnsureSuccessStatusCode();

            var combatPlayers = await response.Content.ReadFromJsonAsync<IEnumerable<CombatPlayerModel>>();
            if (combatPlayers == null)
            {
                throw new ArgumentNullException(nameof(combatPlayers));
            }

            foreach (var item in combatPlayers)
            {
                var playerParseInfo = await GetPlayerParseInfoAsync(item.Id);
                item.PlayerParseInfo = playerParseInfo;
            }

            return combatPlayers;
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, ex.Message);

            return new List<CombatPlayerModel>();
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "HTTP request error: {Message}", ex.Message);

            return new List<CombatPlayerModel>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred: {Message}", ex.Message);

            return new List<CombatPlayerModel>();
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
            if (details == null)
            {
                throw new ArgumentNullException(nameof(details));
            }

            return details;
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, ex.Message);

            return new List<T>();
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "HTTP request error: {Message}", ex.Message);

            return new List<T>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred: {Message}", ex.Message);

            return new List<T>();
        }
    }

    public async Task<CombatLogModel> SaveCombatLogAsync(List<CombatModel> combats, LogType logType, CancellationToken cancellationToken)
    {
        try
        {
            var user = _memoryCache.Get<AppUserModel>(nameof(MemoryCacheValue.User));
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            var dungeonNames = combats
                 .GroupBy(group => group.DungeonName)
                 .Select(select => select.Key)
                 .Where(name => !string.IsNullOrEmpty(name))
                 .ToList();

            var name = CreateCombatLogName(dungeonNames);

            var combatLog = new CombatLogModel
            {
                Name = name,
                Date = DateTimeOffset.UtcNow,
                LogType = (int)logType,
                AppUserId = user.Id,
            };

            var response = await _httpClient.PostAsync("CombatLog", JsonContent.Create(combatLog), cancellationToken);
            response.EnsureSuccessStatusCode();

            var createdCombatLog = await response.Content.ReadFromJsonAsync<CombatLogModel>(cancellationToken: cancellationToken);
            if (createdCombatLog == null)
            {
                throw new ArgumentNullException(nameof(createdCombatLog));
            }

            return createdCombatLog;
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, ex.Message);

            return new CombatLogModel();
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "HTTP request error: {Message}", ex.Message);

            return new CombatLogModel();
        }
        catch (TaskCanceledException ex)
        {
            _logger.LogWarning(ex, "Task was canceled: {Message}", ex.Message);

            return new CombatLogModel();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred: {Message}", ex.Message);

            return new CombatLogModel();
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
            var playerInfo = playerParseInfoCollection?.FirstOrDefault();
            if (playerInfo == null)
            {
                throw new ArgumentNullException(nameof(playerInfo));
            }

            return playerInfo;
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, ex.Message);

            return new PlayerParseInfoModel();
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "HTTP request error: {Message}", ex.Message);

            return new PlayerParseInfoModel();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred: {Message}", ex.Message);

            return new PlayerParseInfoModel();
        }
    }

    private static string CreateCombatLogName(List<string> dungeonNames)
    {
        var combatLogDungeonName = new StringBuilder();
        foreach (var item in dungeonNames)
        {
            var dungeonName = item.Trim('"');
            combatLogDungeonName.Append($"{dungeonName}/");
        }

        combatLogDungeonName.Remove(combatLogDungeonName.Length - 1, 1);

        return combatLogDungeonName.ToString();
    }
}
