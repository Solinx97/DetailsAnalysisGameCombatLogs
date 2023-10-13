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

public class CombatDataHelper : ICombatDataHelper
{
    private readonly IMapper _mapper;
    private readonly ILogger _logger;
    private readonly IService<DamageDoneDto, int> _damageDoneService;
    private readonly IService<DamageDoneGeneralDto, int> _damageDoneGeneralService;
    private readonly IService<HealDoneDto, int> _healDoneService;
    private readonly IService<HealDoneGeneralDto, int> _healDoneGeneralService;
    private readonly IService<DamageTakenDto, int> _damageTakenService;
    private readonly IService<DamageTakenGeneralDto, int> _damageTakenGeneralService;
    private readonly IService<ResourceRecoveryDto, int> _resourceRecoveryService;
    private readonly IService<ResourceRecoveryGeneralDto, int> _resourceRecoveryGeneralService;

    public CombatDataHelper(IMapper mapper, ILogger logger, IService<DamageDoneDto, int> damageDoneService, IService<DamageDoneGeneralDto, int> damageDoneGeneralService, 
        IService<HealDoneDto, int> healDoneService, IService<HealDoneGeneralDto, int> healDoneGeneralService, IService<DamageTakenDto, int> damageTakenService, 
        IService<DamageTakenGeneralDto, int> damageTakenGeneralService, IService<ResourceRecoveryDto, int> resourceRecoveryService, IService<ResourceRecoveryGeneralDto, int> resourceRecoveryGeneralService)
    {
        _mapper = mapper;
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

    public async Task SaveCombatPlayerDataAsync(CombatDto combat, CombatPlayerDto combatPlayer, List<string> combatData)
    {
        var parsedCombat = _mapper.Map<Combat>(combat);

        var damageDoneDetails = new CombatDetailsDamageDone(_logger);
        damageDoneDetails.GetData(combatPlayer.Username, combatData);

        var damageDoneGeneralData = damageDoneDetails.GetDamageDoneGeneral(damageDoneDetails.DamageDone, parsedCombat);

        await UploadDataAsync(damageDoneDetails.DamageDone, _damageDoneService, combatPlayer.Id);
        await UploadDataAsync(damageDoneGeneralData.ToList(), _damageDoneGeneralService, combatPlayer.Id);

        var healDoneDetails = new CombatDetailsHealDone(_logger);
        healDoneDetails.GetData(combatPlayer.Username, combatData);

        var healDoneGeneralData = healDoneDetails.GetHealDoneGeneral(healDoneDetails.HealDone, parsedCombat);

        await UploadDataAsync(healDoneDetails.HealDone, _healDoneService, combatPlayer.Id);
        await UploadDataAsync(healDoneGeneralData.ToList(), _healDoneGeneralService, combatPlayer.Id);

        var damageTakenDetails = new CombatDetailsDamageTaken(_logger);
        damageTakenDetails.GetData(combatPlayer.Username, combatData);

        var damageTakenGeneralData = damageTakenDetails.GetDamageTakenGeneral(damageTakenDetails.DamageTaken, parsedCombat);

        await UploadDataAsync(damageTakenDetails.DamageTaken, _damageTakenService, combatPlayer.Id);
        await UploadDataAsync(damageTakenGeneralData.ToList(), _damageTakenGeneralService, combatPlayer.Id);

        var resourceRecoveryDetails = new CombatDetailsResourceRecovery(_logger);
        resourceRecoveryDetails.GetData(combatPlayer.Username, combatData);

        var resourceRecoveryGeneralData = resourceRecoveryDetails.GetResourceRecoveryGeneral(resourceRecoveryDetails.ResourceRecovery, parsedCombat);

        await UploadDataAsync(resourceRecoveryDetails.ResourceRecovery, _resourceRecoveryService, combatPlayer.Id);
        await UploadDataAsync(resourceRecoveryGeneralData.ToList(), _resourceRecoveryGeneralService, combatPlayer.Id);
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

    private async Task DeleteDataAsync<TServiceModel>(int combatPlayerId, IService<TServiceModel, int> service)
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
