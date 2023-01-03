using AutoMapper;
using CombatAnalysis.BL.DTO;
using CombatAnalysis.BL.Interfaces;
using CombatAnalysis.CombatParser.Entities;
using CombatAnalysis.CombatParser.Extensions;
using CombatAnalysis.CombatParser.Patterns;
using CombatAnalysis.CombatParserAPI.Interfaces;
using CombatAnalysis.CombatParserAPI.Models;
using System.Text;

namespace CombatAnalysis.CombatParserAPI.Helpers;

public class SaveCombatDataHelper : ISaveCombatDataHelper
{
    private readonly IMapper _mapper;
    private readonly IHttpClientHelper _httpClient;
    private readonly ILogger _logger;
    private readonly IService<DamageDoneDto, int> _damageDoneService;
    private readonly IService<DamageDoneGeneralDto, int> _damageDoneGeneralService;
    private readonly IService<HealDoneDto, int> _healDoneService;
    private readonly IService<HealDoneGeneralDto, int> _healDoneGeneralService;
    private readonly IService<DamageTakenDto, int> _damageTakenService;
    private readonly IService<DamageTakenGeneralDto, int> _damageTakenGeneralService;
    private readonly IService<ResourceRecoveryDto, int> _resourceRecoveryService;
    private readonly IService<ResourceRecoveryGeneralDto, int> _resourceRecoveryGeneralService;

    public SaveCombatDataHelper(IMapper mapper, IHttpClientHelper httpClient, ILogger logger,
        IService<DamageDoneDto, int> damageDoneService, IService<DamageDoneGeneralDto, int> damageDoneGeneralService, IService<HealDoneDto, int> healDoneService,
        IService<HealDoneGeneralDto, int> healDoneGeneralService, IService<DamageTakenDto, int> damageTakenService, IService<DamageTakenGeneralDto, int> damageTakenGeneralService,
        IService<ResourceRecoveryDto, int> resourceRecoveryService, IService<ResourceRecoveryGeneralDto, int> resourceRecoveryGeneralService)
    {
        _mapper = mapper;
        _httpClient = httpClient;
        _logger = logger;
        _damageDoneService = damageDoneService;
        _damageDoneGeneralService = damageDoneGeneralService;
        _healDoneService = healDoneService;
        _healDoneGeneralService = healDoneGeneralService;
        _damageTakenService = damageTakenService;
        _damageTakenGeneralService = damageTakenGeneralService;
        _resourceRecoveryService = resourceRecoveryService;
        _resourceRecoveryGeneralService = resourceRecoveryGeneralService;
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

        SaveData<DamageDone, DamageDoneDto>(damageDoneDetails.DamageDone, (temp) => _damageDoneService.CreateAsync(temp).GetAwaiter().GetResult(), combatPlayer.Id);

        var damageDoneGeneralData = damageDoneDetails.GetDamageDoneGeneral(damageDoneDetails.DamageDone, combat);
        SaveData<DamageDoneGeneral, DamageDoneGeneralDto>(damageDoneGeneralData.ToList(), (temp) => _damageDoneGeneralService.CreateAsync(temp).GetAwaiter().GetResult(), combatPlayer.Id);

        var healDoneDetails = new CombatDetailsHealDone(_logger);
        healDoneDetails.GetData(combatPlayer.UserName, CombatData);
        SaveData<HealDone, HealDoneDto>(healDoneDetails.HealDone, (temp) => _healDoneService.CreateAsync(temp).GetAwaiter().GetResult(), combatPlayer.Id);

        var healDoneGeneralData = healDoneDetails.GetHealDoneGeneral(healDoneDetails.HealDone, combat);
        SaveData<HealDoneGeneral, HealDoneGeneralDto>(healDoneGeneralData.ToList(), (temp) => _healDoneGeneralService.CreateAsync(temp).GetAwaiter().GetResult(), combatPlayer.Id);

        var damageTakenDetails = new CombatDetailsDamageTaken(_logger);
        damageTakenDetails.GetData(combatPlayer.UserName, CombatData);
        SaveData<DamageTaken, DamageTakenDto>(damageTakenDetails.DamageTaken, (temp) => _damageTakenService.CreateAsync(temp).GetAwaiter().GetResult(), combatPlayer.Id);

        var damageTakenGeneralData = damageTakenDetails.GetDamageTakenGeneral(damageTakenDetails.DamageTaken, combat);
        SaveData<DamageTakenGeneral, DamageTakenGeneralDto>(damageTakenGeneralData.ToList(), (temp) => _damageTakenGeneralService.CreateAsync(temp).GetAwaiter().GetResult(), combatPlayer.Id);

        var resourceRecoveryDetails = new CombatDetailsResourceRecovery(_logger);
        resourceRecoveryDetails.GetData(combatPlayer.UserName, CombatData);
        SaveData<ResourceRecovery, ResourceRecoveryDto>(resourceRecoveryDetails.ResourceRecovery, (temp) => _resourceRecoveryService.CreateAsync(temp).GetAwaiter().GetResult(), combatPlayer.Id);

        var resourceRecoveryGeneralData = resourceRecoveryDetails.GetResourceRecoveryGeneral(resourceRecoveryDetails.ResourceRecovery, combat);
        SaveData<ResourceRecoveryGeneral, ResourceRecoveryGeneralDto>(resourceRecoveryGeneralData.ToList(), (temp) => _resourceRecoveryGeneralService.CreateAsync(temp).GetAwaiter().GetResult(), combatPlayer.Id);

        combatModel.IsReady = true;
        combatModel.Data = new List<string>();
        combatModel.Players = new List<CombatPlayerModel>();
        await _httpClient.PutAsync("Combat", JsonContent.Create(combatModel));
    }

    private void SaveData<T, T2>(List<T> data, Action<T2> temp, int combatPlayerId)
        where T : class
        where T2 : class
    {
        foreach (var item in data)
        {
            item.GetType().GetProperty("CombatPlayerId").SetValue(item, combatPlayerId);

            var map = _mapper.Map<T2>(item);
            temp?.Invoke(map);
        }
    }
}
