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

    public async Task<List<bool>> SaveAsync(List<CombatModel> combats, CombatLogModel combatLog, LogType logType)
    {
        try
        {
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
            });

            await SetReadyForCombatLog(combatLog);

            return combatsAreUploaded;
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, ex.Message);

            return null;
        }
    }

    public async Task DeleteCombatLogAsync(int id)
    {
        var combats = await LoadCombatsAsync(id).ConfigureAwait(false);
        if (combats == null)
        {
            return;
        }

        foreach (var item in combats)
        {
            await DeleteCombatPlayersData(item.Id);
        }

        await _httpClient.DeletAsync($"Combat/DeleteByCombatLogId/{id}");
        await _httpClient.DeletAsync($"CombatLog/{id}");
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

    public async Task<IEnumerable<CombatLogModel>> LoadCombatLogsByUserAsync()
    {
        try
        {
            var user = _memoryCache.Get<AppUserModel>("account");
            if (user == null)
            {
                return new List<CombatLogModel>();
            }

            var combatLogs = new List<CombatLogModel>();
            var responseMessage = await _httpClient.GetAsync($"CombatLogByUser/byUserId/{user.Id}");
            if (responseMessage.StatusCode != System.Net.HttpStatusCode.OK)
            {
                return null;
            }

            var combatLogsByUser = await responseMessage.Content.ReadFromJsonAsync<IEnumerable<CombatLogByUserModel>>();

            foreach (var item in combatLogsByUser)
            {
                responseMessage = await _httpClient.GetAsync($"CombatLog/{item.CombatLogId}");
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
        var responseMessage = await _httpClient.GetAsync($"CombatPlayer/FindByCombatId/{combatId}");
        var combatPlayers = await responseMessage.Content.ReadFromJsonAsync<IEnumerable<CombatPlayerModel>>();

        return combatPlayers;
    }

    public async Task<IEnumerable<T>> LoadCombatDetailsByCombatPlayerId<T>(string address, int combatPlayerId)
        where T : class
    {
        var responseMessage = await _httpClient.GetAsync($"{address}/{combatPlayerId}");
        var healDones = await responseMessage.Content.ReadFromJsonAsync<IEnumerable<T>>();

        return healDones;
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
        var user = _memoryCache.Get<AppUserModel>("account");
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

    private async Task SetReadyForCombatLog(CombatLogModel combatLog)
    {
        combatLog.IsReady = true;

        await _httpClient.PutAsync("CombatLog", JsonContent.Create(combatLog));
    }

    private async Task DeleteCombatPlayersData(int combatId)
    {
        var players = await LoadCombatPlayersAsync(combatId);
        foreach (var item in players)
        {
            await _httpClient.DeletAsync($"HealDone/DeleteByCombatPlayerId/{item.Id}");
            await _httpClient.DeletAsync($"HealDoneGeneral/DeleteByCombatPlayerId/{item.Id}");
            await _httpClient.DeletAsync($"DamageDone/DeleteByCombatPlayerId/{item.Id}");
            await _httpClient.DeletAsync($"DamageDoneGeneral/DeleteByCombatPlayerId/{item.Id}");
            await _httpClient.DeletAsync($"DamageTaken/DeleteByCombatPlayerId/{item.Id}");
            await _httpClient.DeletAsync($"DamageTakenGeneral/DeleteByCombatPlayerId/{item.Id}");
            await _httpClient.DeletAsync($"ResourceRecovery/DeleteByCombatPlayerId/{item.Id}");
            await _httpClient.DeletAsync($"ResourceRecoveryGeneral/DeleteByCombatPlayerId/{item.Id}");
        }

        await _httpClient.DeletAsync($"CombatPlayer/DeleteByCombatId/{combatId}");
    }
}
