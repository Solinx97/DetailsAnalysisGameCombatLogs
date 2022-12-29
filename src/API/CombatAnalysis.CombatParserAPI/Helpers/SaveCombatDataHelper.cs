using AutoMapper;
using CombatAnalysis.CombatParser.Entities;
using CombatAnalysis.CombatParser.Extensions;
using CombatAnalysis.CombatParser.Patterns;
using CombatAnalysis.CombatParserAPI.Interfaces;
using CombatAnalysis.CombatParserAPI.Models;
using System.Text;

namespace CombatAnalysis.CombatParserAPI.Helpers;

public class SaveCombatDataHelper
{
    private readonly IMapper _mapper;
    private readonly IHttpClientHelper _httpClient;
    private readonly ILogger _logger;

    public SaveCombatDataHelper(IMapper mapper, IHttpClientHelper httpClient, ILogger logger)
    {
        _mapper = mapper;
        _httpClient = httpClient;
        _logger = logger;
    }

    public static List<string> CombatData { get; set; }

    public CombatLogModel CreateCombatLog(List<string> dungeonNames)
    {
        var combatLogDungeonName = new StringBuilder();
        foreach (var item in dungeonNames)
        {
            var dungeonName = item.Trim('"');
            combatLogDungeonName.Append($"{dungeonName}/");
        }

        combatLogDungeonName.Remove(combatLogDungeonName.Length - 1, 1);

        var combatLog = new CombatLogModel
        {
            Name = combatLogDungeonName.ToString(),
            Date = DateTimeOffset.Now
        };

        return combatLog;
    }

    public async Task SaveCombatPlayerDataAsync(int combatId, CombatPlayerModel combatPlayer)
    {
        var combatResponse = await _httpClient.GetAsync($"Combat/{combatId}");
        var combatModel = await combatResponse.Content.ReadFromJsonAsync<CombatModel>();
        var combat = _mapper.Map<Combat>(combatModel);

        var damageDoneDetails = new CombatDetailsDamageDone(_logger);
        damageDoneDetails.GetData(combatPlayer.UserName, CombatData);
        await SaveData(damageDoneDetails.DamageDone, nameof(DamageDone), combatPlayer.Id);

        var damageDoneGeneralData = damageDoneDetails.GetDamageDoneGeneral(damageDoneDetails.DamageDone, combat);
        await SaveData(damageDoneGeneralData.ToList(), nameof(DamageDoneGeneral), combatPlayer.Id);

        var healDoneDetails = new CombatDetailsHealDone(_logger);
        healDoneDetails.GetData(combatPlayer.UserName, CombatData);
        await SaveData(healDoneDetails.HealDone, nameof(DamageDone), combatPlayer.Id);

        var healDoneGeneralData = healDoneDetails.GetHealDoneGeneral(healDoneDetails.HealDone, combat);
        await SaveData(healDoneGeneralData.ToList(), nameof(HealDoneGeneral), combatPlayer.Id);

        var damageTakenDetails = new CombatDetailsDamageTaken(_logger);
        damageTakenDetails.GetData(combatPlayer.UserName, CombatData);
        await SaveData(damageTakenDetails.DamageTaken, nameof(DamageTaken), combatPlayer.Id);

        var damageTakenGeneralData = damageTakenDetails.GetDamageTakenGeneral(damageTakenDetails.DamageTaken, combat);
        await SaveData(damageTakenGeneralData.ToList(), nameof(DamageTakenGeneral), combatPlayer.Id);

        var resourceRecoveryDetails = new CombatDetailsResourceRecovery(_logger);
        resourceRecoveryDetails.GetData(combatPlayer.UserName, CombatData);
        await SaveData(resourceRecoveryDetails.ResourceRecovery, nameof(ResourceRecovery), combatPlayer.Id);

        var resourceRecoveryGeneralData = resourceRecoveryDetails.GetResourceRecoveryGeneral(resourceRecoveryDetails.ResourceRecovery, combat);
        await SaveData(resourceRecoveryGeneralData.ToList(), nameof(ResourceRecoveryGeneral), combatPlayer.Id);

        combatModel.IsReady = true;
        combatModel.Data = new List<string>();
        await _httpClient.PutAsync("Combat", JsonContent.Create(combatModel));
    }

    private async Task SaveData<T>(List<T> data, string dataName, int combatPlayerId)
        where T : class
    {
        foreach (var item in data)
        {
            item.GetType().GetProperty("CombatPlayerId").SetValue(item, combatPlayerId);

            await _httpClient.PostAsync(dataName, JsonContent.Create(item));
        }
    }
}
