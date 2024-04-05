using CombatAnalysis.Core.Consts;
using CombatAnalysis.Core.Enums;
using CombatAnalysis.Core.Interfaces;
using CombatAnalysis.Core.Models;
using CombatAnalysis.Core.Models.User;
using CombatAnalysis.Core.Modelsl;
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

    public async Task<List<bool>> SaveAsync(List<CombatModel> combats, CombatLogModel combatLog, LogType logType, Action<int, string, string> combatUploaded)
    {
        try
        {
            var currentCombatNumber = 0;
            var combatsAreUploaded = new List<bool>();
            if (logType == LogType.Public || logType == LogType.Private)
            {
                await SaveCombatLogByUserAsync(combatLog.Id, logType);
            }

            await Parallel.ForEachAsync(combats, async (item, token) =>
            {
                item.CombatLogId = combatLog.Id;

                var response = await _httpClient.PostAsync("Combat", JsonContent.Create(item));
                combatsAreUploaded.Add(response.IsSuccessStatusCode);

                currentCombatNumber++;
                combatUploaded(currentCombatNumber, item.DungeonName, item.Name);
            });

            await SetReadyForCombatLogAsync(combatLog);

            return combatsAreUploaded;
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, ex.Message);

            return null;
        }
    }

    public async Task DeleteCombatLogByUserAsync(int id)
    {
        try
        {
            await _httpClient.DeletAsync($"CombatLogByUser/{id}");
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, ex.Message);
        }
    }

    public async Task<IEnumerable<CombatLogModel>> LoadCombatLogsAsync()
    {
        try
        {
            var responseMessage = await _httpClient.GetAsync("CombatLog");
            if (!responseMessage.IsSuccessStatusCode)
            {
                return null;
            }

            var combatLogs = await responseMessage.Content.ReadFromJsonAsync<IEnumerable<CombatLogModel>>();

            return combatLogs;
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, ex.Message);

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
                var responseMessage = await _httpClient.GetAsync($"CombatLog/{item}");
                if (!responseMessage.IsSuccessStatusCode)
                {
                    return null;
                }

                var combatLog = await responseMessage.Content.ReadFromJsonAsync<CombatLogModel>();

                combatLogs.Add(combatLog);
            }

            return combatLogs;
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, ex.Message);

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
                return null;
            }

            var responseMessage = await _httpClient.GetAsync($"CombatLogByUser/byUserId/{user.Id}");
            if (!responseMessage.IsSuccessStatusCode)
            {
                return null;
            }

            var combatLogsByUser = await responseMessage.Content.ReadFromJsonAsync<IEnumerable<CombatLogByUserModel>>();

            return combatLogsByUser;
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, ex.Message);

            return null;
        }
    }

    public async Task<IEnumerable<CombatModel>> LoadCombatsAsync(int combatLogId)
    {
        try
        {
            _httpClient.BaseAddress = Port.CombatParserApi;

            var responseMessage = await _httpClient.GetAsync($"Combat/FindByCombatLogId/{combatLogId}");
            var combats = await responseMessage.Content.ReadFromJsonAsync<IEnumerable<CombatModel>>();

            return combats;
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, ex.Message);

            return null;
        }
    }

    public async Task<IEnumerable<CombatPlayerModel>> LoadCombatPlayersAsync(int combatId)
    {
        try
        {
            var responseMessage = await _httpClient.GetAsync($"CombatPlayer/FindByCombatId/{combatId}");
            var combatPlayers = await responseMessage.Content.ReadFromJsonAsync<IEnumerable<CombatPlayerModel>>();
            foreach (var item in combatPlayers)
            {
                var playerParseInfo = await GetPlayerParseInfoAsync(item.Id);
                item.PlayerParseInfo = playerParseInfo;
            }

            return combatPlayers;
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, ex.Message);

            return null;
        }
    }

    public async Task<IEnumerable<T>> LoadCombatDetailsByCombatPlayerId<T>(string address, int combatPlayerId)
        where T : class
    {
        try
        {
            var responseMessage = await _httpClient.GetAsync($"{address}/{combatPlayerId}");
            var details = await responseMessage.Content.ReadFromJsonAsync<IEnumerable<T>>();

            return details;
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, ex.Message);

            return null;
        }
    }

    public async Task<CombatLogModel> SaveCombatLogAsync(List<CombatModel> combats)
    {
        try
        {
            var dungeonNames = combats
                 .GroupBy(group => group.DungeonName)
                 .Select(select => select.Key)
                 .ToList();

            var combatLogResponse = await _httpClient.PostAsync("CombatLog", JsonContent.Create(dungeonNames));
            if (!combatLogResponse.IsSuccessStatusCode)
            {
                return null;
            }

            var createdCombatLog = await combatLogResponse.Content.ReadFromJsonAsync<CombatLogModel>();

            return createdCombatLog;
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, ex.Message);

            return null;
        }
    }

    private async Task SaveCombatLogByUserAsync(int combatLogId, LogType logType)
    {
        try
        {
            var user = _memoryCache.Get<AppUserModel>(nameof(MemoryCacheValue.User));
            if (user == null)
            {
                return;
            }

            var combatLogByUser = new CombatLogByUserModel
            {
                UserId = user.Id,
                CombatLogId = combatLogId,
                PersonalLogType = (int)logType
            };

            await _httpClient.PostAsync("CombatLogByUser", JsonContent.Create(combatLogByUser));
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, ex.Message);
        }
    }

    private async Task SetReadyForCombatLogAsync(CombatLogModel combatLog)
    {
        try
        {
            combatLog.IsReady = true;

            await _httpClient.PutAsync("CombatLog", JsonContent.Create(combatLog));
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, ex.Message);
        }
    }

    private async Task<PlayerParseInfoModel> GetPlayerParseInfoAsync(int combatPlayerId)
    {
        var responseMessage = await _httpClient.GetAsync($"PlayerParseInfo/FindByCombatPlayerId/{combatPlayerId}");
        var playerParseInfo = await responseMessage.Content.ReadFromJsonAsync<IEnumerable<PlayerParseInfoModel>>();

        return playerParseInfo.FirstOrDefault();
    }
}
