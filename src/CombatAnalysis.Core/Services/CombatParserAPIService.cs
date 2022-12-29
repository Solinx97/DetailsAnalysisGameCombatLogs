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

internal class CombatParserAPIService
{
    private readonly IHttpClientHelper _httpClient;
    private readonly ILogger _logger;
    private readonly IMemoryCache _memoryCache;

    private List<CombatModel> _combats;

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

    public async Task<bool> SaveAsync(List<CombatModel> combats, LogType logType)
    {
        _combats = combats;

        try
        {
            var createdCombatLogId = await SaveCombatLogAsync().ConfigureAwait(false);
            if (logType != LogType.NotIncludePlayer)
            {
                await SaveCombatLogByUser(createdCombatLogId, logType);
            }

            foreach (var item in combats)
            {
                await SaveCombatDataAsync(item, createdCombatLogId).ConfigureAwait(false);
            }

            await SetReadyForCombatLog(createdCombatLogId).ConfigureAwait(false);

            return true;
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, ex.Message);

            return false;
        }
    }

    public async Task DeleteCombatLogAsync(int id)
    {
        var combats = await LoadCombatsAsync(id).ConfigureAwait(false);
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
            if (responseMessage.StatusCode != System.Net.HttpStatusCode.OK)
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
        _httpClient.BaseAddress = Port.CombatParserApi;
        var responseMessage = await _httpClient.GetAsync($"Combat/FindByCombatLogId/{combatLogId}");
        var combats = await responseMessage.Content.ReadFromJsonAsync<IEnumerable<CombatModel>>();

        return combats;
    }

    public async Task<IEnumerable<CombatPlayerModel>> LoadCombatPlayersAsync(int combatId)
    {
        var responseMessage = await _httpClient.GetAsync($"CombatPlayer/FindByCombatId/{combatId}");
        var combatPlayers = await responseMessage.Content.ReadFromJsonAsync<IEnumerable<CombatPlayerModel>>();

        return combatPlayers;
    }

    public async Task<IEnumerable<HealDoneModel>> LoadHealDoneDetailsAsync(int combatPlayerId)
    {
        var responseMessage = await _httpClient.GetAsync($"HealDone/FindByCombatPlayerId/{combatPlayerId}");
        var healDones = await responseMessage.Content.ReadFromJsonAsync<IEnumerable<HealDoneModel>>();

        return healDones;
    }

    public async Task<IEnumerable<HealDoneGeneralModel>> LoadHealDoneGeneralAsync(int combatPlayerId)
    {
        var responseMessage = await _httpClient.GetAsync($"HealDoneGeneral/FindByCombatPlayerId/{combatPlayerId}");
        var healDoneGenerics = await responseMessage.Content.ReadFromJsonAsync<IEnumerable<HealDoneGeneralModel>>();

        return healDoneGenerics;
    }

    public async Task<IEnumerable<DamageDoneModel>> LoadDamageDoneDetailsAsync(int combatPlayerId)
    {
        var responseMessage = await _httpClient.GetAsync($"DamageDone/FindByCombatPlayerId/{combatPlayerId}");
        var damageDones = await responseMessage.Content.ReadFromJsonAsync<IEnumerable<DamageDoneModel>>();

        return damageDones;
    }

    public async Task<IEnumerable<DamageDoneGeneralModel>> LoadDamageDoneGeneralAsync(int combatPlayerId)
    {
        var responseMessage = await _httpClient.GetAsync($"DamageDoneGeneral/FindByCombatPlayerId/{combatPlayerId}");
        var damageDoneGenerics = await responseMessage.Content.ReadFromJsonAsync<IEnumerable<DamageDoneGeneralModel>>();

        return damageDoneGenerics;
    }

    public async Task<IEnumerable<DamageTakenModel>> LoadDamageTakenDetailsAsync(int combatPlayerId)
    {
        var responseMessage = await _httpClient.GetAsync($"DamageTaken/FindByCombatPlayerId/{combatPlayerId}");
        var damageTakens = await responseMessage.Content.ReadFromJsonAsync<IEnumerable<DamageTakenModel>>();

        return damageTakens;
    }

    public async Task<IEnumerable<DamageTakenGeneralModel>> LoadDamageTakenGeneralAsync(int combatPlayerId)
    {
        var responseMessage = await _httpClient.GetAsync($"DamageTakenGeneral/FindByCombatPlayerId/{combatPlayerId}");
        var damageTakenGenerals = await responseMessage.Content.ReadFromJsonAsync<IEnumerable<DamageTakenGeneralModel>>();

        return damageTakenGenerals;
    }

    public async Task<IEnumerable<ResourceRecoveryModel>> LoadResourceRecoveryDetailsAsync(int combatPlayerId)
    {
        var responseMessage = await _httpClient.GetAsync($"ResourceRecovery/FindByCombatPlayerId/{combatPlayerId}");
        var resourceRecoveryes = await responseMessage.Content.ReadFromJsonAsync<IEnumerable<ResourceRecoveryModel>>();

        return resourceRecoveryes;
    }

    public async Task<IEnumerable<ResourceRecoveryGeneralModel>> LoadResourceRecoveryGeneralAsync(int combatPlayerId)
    {
        var responseMessage = await _httpClient.GetAsync($"ResourceRecoveryGeneral/FindByCombatPlayerId/{combatPlayerId}");
        var ResourceRecoveryGenerals = await responseMessage.Content.ReadFromJsonAsync<IEnumerable<ResourceRecoveryGeneralModel>>();

        return ResourceRecoveryGenerals;
    }

    private async Task<int> SaveCombatLogAsync()
    {
        var dungeonNames = _combats
             .GroupBy(group => group.DungeonName)
             .Select(select => select.Key)
             .ToList();

        var combatLogResponse = await _httpClient.PostAsync("CombatLog", JsonContent.Create(dungeonNames));
        var createdCombatLog = await combatLogResponse.Content.ReadFromJsonAsync<CombatLogModel>();

        return createdCombatLog.Id;
    }

    private async Task SaveCombatLogByUser(int combatLogId, LogType logType)
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

    private async Task SaveCombatDataAsync(CombatModel combat, int createdCombatLogId)
    {
        combat.CombatLogId = createdCombatLogId;
        var combatDataResponse = await _httpClient.PostAsync("Combat", JsonContent.Create(combat));
        var createdCombat = await combatDataResponse.Content.ReadFromJsonAsync<CombatModel>();

        foreach (var item in combat.Players)
        {
            item.CombatId = createdCombat.Id;
        }

        await _httpClient.PostAsync("Combat/saveCombatPlayers", JsonContent.Create(combat.Players));
    }

    private async Task SetReadyForCombatLog(int createdCombatLogId)
    {
        var combatLogResponse = await _httpClient.GetAsync($"CombatLog/{createdCombatLogId}");
        var combatLog = await combatLogResponse.Content.ReadFromJsonAsync<CombatLogModel>();
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
