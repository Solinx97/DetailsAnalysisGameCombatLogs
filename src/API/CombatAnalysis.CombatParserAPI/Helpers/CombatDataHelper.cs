using AutoMapper;
using CombatAnalysis.BL.DTO;
using CombatAnalysis.BL.Interfaces;
using CombatAnalysis.CombatParser.Extensions;
using CombatAnalysis.CombatParser.Patterns;
using CombatAnalysis.CombatParserAPI.Interfaces;
using CombatAnalysis.CombatParserAPI.Models;
using System.Text;

namespace CombatAnalysis.CombatParserAPI.Helpers;

public class CombatDataHelper : ICombatDataHelper
{
    private readonly IMapper _mapper;
    private readonly ILogger<CombatDataHelper> _logger;
    private readonly IPlayerParseInfoHelper _playerParseInfoHelper;
    private readonly IPlayerInfoCountService<DamageDoneDto, int> _damageDoneService;
    private readonly IPlayerInfoService<DamageDoneGeneralDto, int> _damageDoneGeneralService;
    private readonly IPlayerInfoCountService<HealDoneDto, int> _healDoneService;
    private readonly IPlayerInfoService<HealDoneGeneralDto, int> _healDoneGeneralService;
    private readonly IPlayerInfoCountService<DamageTakenDto, int> _damageTakenService;
    private readonly IPlayerInfoService<DamageTakenGeneralDto, int> _damageTakenGeneralService;
    private readonly IPlayerInfoCountService<ResourceRecoveryDto, int> _resourceRecoveryService;
    private readonly IPlayerInfoService<ResourceRecoveryGeneralDto, int> _resourceRecoveryGeneralService;

    public CombatDataHelper(IMapper mapper, ILogger<CombatDataHelper> logger, IPlayerParseInfoHelper playerParseInfoHelper,
        IPlayerInfoCountService<DamageDoneDto, int> damageDoneService, IPlayerInfoService<DamageDoneGeneralDto, int> damageDoneGeneralService,
        IPlayerInfoCountService<HealDoneDto, int> healDoneService, IPlayerInfoService<HealDoneGeneralDto, int> healDoneGeneralService, IPlayerInfoCountService<DamageTakenDto, int> damageTakenService,
        IPlayerInfoService<DamageTakenGeneralDto, int> damageTakenGeneralService, IPlayerInfoCountService<ResourceRecoveryDto, int> resourceRecoveryService,
        IPlayerInfoService<ResourceRecoveryGeneralDto, int> resourceRecoveryGeneralService)
    {
        _mapper = mapper;
        _logger = logger;
        _playerParseInfoHelper = playerParseInfoHelper;
        _damageDoneService = damageDoneService;
        _damageDoneGeneralService = damageDoneGeneralService;
        _healDoneService = healDoneService;
        _healDoneGeneralService = healDoneGeneralService;
        _damageTakenService = damageTakenService;
        _damageTakenGeneralService = damageTakenGeneralService;
        _resourceRecoveryService = resourceRecoveryService;
        _resourceRecoveryGeneralService = resourceRecoveryGeneralService;
    }

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

    public async Task SaveCombatPlayerAsync(CombatDto combat, Dictionary<string, List<string>> petsId, CombatPlayerDto combatPlayer, List<string> combatData)
    {
        var parsedCombat = _mapper.Map<CombatParser.Entities.Combat>(combat);

        var damageDoneDetails = new CombatDetailsDamageDone(_logger)
        {
            PetsId = petsId,
        };
        damageDoneDetails.GetData(combatPlayer.PlayerId, combatData);

        var damageDoneGeneralData = damageDoneDetails.GetDamageDoneGeneral(damageDoneDetails.DamageDone, parsedCombat);
        var damageDoneGeneralList = damageDoneGeneralData.ToList();

        await UploadDataAsync(damageDoneDetails.DamageDone, _damageDoneService, combatPlayer.Id);
        await UploadDataAsync(damageDoneGeneralList, _damageDoneGeneralService, combatPlayer.Id);

        var healDoneDetails = new CombatDetailsHealDone(_logger);
        healDoneDetails.GetData(combatPlayer.PlayerId, combatData);

        var healDoneGeneralData = healDoneDetails.GetHealDoneGeneral(healDoneDetails.HealDone, parsedCombat);
        var healDoneGeneralList = healDoneGeneralData.ToList();

        await UploadDataAsync(healDoneDetails.HealDone, _healDoneService, combatPlayer.Id);
        await UploadDataAsync(healDoneGeneralList, _healDoneGeneralService, combatPlayer.Id);

        var damageTakenDetails = new CombatDetailsDamageTaken(_logger);
        damageTakenDetails.GetData(combatPlayer.PlayerId, combatData);

        var damageTakenGeneralData = damageTakenDetails.GetDamageTakenGeneral(damageTakenDetails.DamageTaken, parsedCombat);

        await UploadDataAsync(damageTakenDetails.DamageTaken, _damageTakenService, combatPlayer.Id);
        await UploadDataAsync(damageTakenGeneralData.ToList(), _damageTakenGeneralService, combatPlayer.Id);

        var resourceRecoveryDetails = new CombatDetailsResourceRecovery(_logger);
        resourceRecoveryDetails.GetData(combatPlayer.PlayerId, combatData);

        var resourceRecoveryGeneralData = resourceRecoveryDetails.GetResourceRecoveryGeneral(resourceRecoveryDetails.ResourceRecovery, parsedCombat);

        await UploadDataAsync(resourceRecoveryDetails.ResourceRecovery, _resourceRecoveryService, combatPlayer.Id);
        await UploadDataAsync(resourceRecoveryGeneralData.ToList(), _resourceRecoveryGeneralService, combatPlayer.Id);

        if (combat.IsWin)
        {
            await _playerParseInfoHelper.UploadPlayerParseInfoAsync(combat, combatPlayer, damageDoneGeneralList, healDoneGeneralList);
        }
    }

    public async Task DeleteCombatPlayerDataAsync(CombatPlayerDto combatPlayer)
    {
        await DeleteDataAsync(combatPlayer.Id, _damageDoneService);
        await DeleteDataAsync(combatPlayer.Id, _damageDoneGeneralService);

        await DeleteDataAsync(combatPlayer.Id, _healDoneService);
        await DeleteDataAsync(combatPlayer.Id, _healDoneGeneralService);

        await DeleteDataAsync(combatPlayer.Id, _damageTakenService);
        await DeleteDataAsync(combatPlayer.Id, _damageTakenGeneralService);

        await DeleteDataAsync(combatPlayer.Id, _resourceRecoveryService);
        await DeleteDataAsync(combatPlayer.Id, _resourceRecoveryGeneralService);
    }

    private async Task UploadDataAsync<TModel, TModelMap>(List<TModel> dataforUpload, IService<TModelMap, int> service, int combatPlayerId)
        where TModel : class
        where TModelMap : class
    {
        foreach (var item in dataforUpload)
        {
            var map = _mapper.Map<TModelMap>(item);
            var property = map.GetType().GetProperty("CombatPlayerId");
            property.SetValue(map, combatPlayerId);

            var createdItem = await service.CreateAsync(map);
            if (createdItem == null)
            {
                throw new ArgumentException("Did not created");
            }
        }
    }

    private static async Task DeleteDataAsync<TServiceModel>(int combatPlayerId, IService<TServiceModel, int> service)
            where TServiceModel : class
    {
        var dataForRemove = await service.GetByParamAsync("CombatPlayerId", combatPlayerId);
        foreach (var item in dataForRemove)
        {
            var property = item.GetType().GetProperty("Id");
            var propertyValue = (int)property.GetValue(item);

            var rowsAffected = await service.DeleteAsync(propertyValue);
            if (rowsAffected == 0)
            {
                throw new ArgumentException($"Did not deleted");
            }
        }
    }
}
