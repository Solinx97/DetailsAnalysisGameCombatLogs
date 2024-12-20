using AutoMapper;
using CombatAnalysis.BL.DTO;
using CombatAnalysis.BL.Interfaces.General;
using CombatAnalysis.CombatParser.Details;
using CombatAnalysis.CombatParser.Entities;
using CombatAnalysis.CombatParser.Extensions;
using CombatAnalysis.CombatParser.Interfaces.Entities;
using CombatAnalysis.CombatParserAPI.Interfaces;
using CombatAnalysis.CombatParserAPI.Models;
using System.Text;

namespace CombatAnalysis.CombatParserAPI.Helpers;

public class CombatDataHelper : ICombatDataHelper
{
    private readonly IMapper _mapper;
    private readonly ILogger<CombatDataHelper> _logger;
    private readonly IPlayerParseInfoHelper _playerParseInfoHelper;
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public CombatDataHelper(IMapper mapper, ILogger<CombatDataHelper> logger, IPlayerParseInfoHelper playerParseInfoHelper, IServiceScopeFactory serviceScopeFactory)
    {
        _mapper = mapper;
        _logger = logger;
        _playerParseInfoHelper = playerParseInfoHelper;
        _serviceScopeFactory = serviceScopeFactory;
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

    public async Task SaveCombatPlayerAsync(CombatModel combat)
    {
        var parsedCombat = _mapper.Map<Combat>(combat);

        var playersId = combat.Players.Select(x => x.PlayerId).ToList();

        var combatDetails = new CombatDetails(_logger, combat.PetsId);
        combatDetails.Calculate(playersId, combat.Data, combat.StartDate, combat.FinishDate);
        combatDetails.CalculateGeneralData(playersId, combat.Duration);

        var uploadTasks = combat.Players.Select(item => UploadAsync(parsedCombat, item, combatDetails, combat.Id)).ToList();
        await Task.WhenAll(uploadTasks);

        var uploadCombatAuraTasks = combatDetails.Auras.Select(item => UploadCombatAuraData(item.Value, combat.Id)).ToList();
        await Task.WhenAll(uploadCombatAuraTasks);
    }

    //public async Task DeleteCombatPlayerDataAsync(CombatPlayerDto combatPlayer)
    //{
    //    await DeleteDataAsync(combatPlayer.Id, _damageDoneService);
    //    await DeleteDataAsync(combatPlayer.Id, _damageDoneGeneralService);

    //    await DeleteDataAsync(combatPlayer.Id, _healDoneService);
    //    await DeleteDataAsync(combatPlayer.Id, _healDoneGeneralService);

    //    await DeleteDataAsync(combatPlayer.Id, _damageTakenService);
    //    await DeleteDataAsync(combatPlayer.Id, _damageTakenGeneralService);

    //    await DeleteDataAsync(combatPlayer.Id, _resourceRecoveryService);
    //    await DeleteDataAsync(combatPlayer.Id, _resourceRecoveryGeneralService);
    //}

    private async Task UploadAsync(Combat combat, CombatPlayerModel combatPlayer, CombatDetails combatDetails, int combatId)
    {
        foreach (var item in combatDetails.PlayersDeath[combatPlayer.PlayerId])
        {
            var lastDamageTaken = combatDetails.DamageTaken[combatPlayer.PlayerId].LastOrDefault(x => x.Target == item.Username);
            if (lastDamageTaken != null)
            {
                item.LastHitValue = lastDamageTaken.Value;
                item.LastHitSpellOrItem = lastDamageTaken.Spell;
            }
        }

        var uploadTasks = new List<Task>
        {
            UploadCombatPlayerPositionData(combatDetails.Positions[combatPlayer.PlayerId], combatPlayer.Id, combatId),

            UploadPlayerInfoData<DamageDone, DamageDoneDto>(combatDetails.DamageDone[combatPlayer.PlayerId], combatPlayer.Id),
            UploadPlayerInfoData<DamageDoneGeneral, DamageDoneGeneralDto>(combatDetails.DamageDoneGeneral[combatPlayer.PlayerId], combatPlayer.Id),
            UploadPlayerInfoData<HealDone, HealDoneDto>(combatDetails.HealDone[combatPlayer.PlayerId], combatPlayer.Id),
            UploadPlayerInfoData<HealDoneGeneral, HealDoneGeneralDto>(combatDetails.HealDoneGeneral[combatPlayer.PlayerId], combatPlayer.Id),
            UploadPlayerInfoData<DamageTaken, DamageTakenDto>(combatDetails.DamageTaken[combatPlayer.PlayerId], combatPlayer.Id),
            UploadPlayerInfoData<DamageTakenGeneral, DamageTakenGeneralDto>(combatDetails.DamageTakenGeneral[combatPlayer.PlayerId], combatPlayer.Id),
            UploadPlayerInfoData<ResourceRecovery, ResourceRecoveryDto>(combatDetails.ResourcesRecovery[combatPlayer.PlayerId], combatPlayer.Id),
            UploadPlayerInfoData<ResourceRecoveryGeneral, ResourceRecoveryGeneralDto>(combatDetails.ResourcesRecoveryGeneral[combatPlayer.PlayerId], combatPlayer.Id),
            UploadPlayerInfoData<PlayerDeath, PlayerDeathDto>(combatDetails.PlayersDeath[combatPlayer.PlayerId], combatPlayer.Id),
        };

        await Task.WhenAll(uploadTasks);

        if (combat.IsWin)
        {
            await _playerParseInfoHelper.UploadPlayerParseInfoAsync(combat, combatPlayer, combatDetails.DamageDoneGeneral[combatPlayer.PlayerId], combatDetails.HealDoneGeneral[combatPlayer.PlayerId]);
        }
    }

    private async Task UploadPlayerInfoData<TModel, TModelMap>(List<TModel> dataforUpload, int combatPlayerId)
        where TModel :class,  ICombatPlayerEntity
        where TModelMap : class, BL.Interfaces.Entity.ICombatPlayerEntity
    {
        using var scope = _serviceScopeFactory.CreateScope();
        var scopedService = scope.ServiceProvider.GetRequiredService<IMutationService<TModelMap>>();

        foreach (var item in dataforUpload)
        {
            var map = _mapper.Map<TModelMap>(item);
            map.CombatPlayerId = combatPlayerId;

            var createdItem = await scopedService.CreateAsync(map);
            if (createdItem == null)
            {
                throw new ArgumentException("Did not created");
            }
        }
    }

    private async Task UploadCombatPlayerPositionData(List<CombatPlayerPosition> combatPlayerPositions, int combatPlayerId, int combatId)
    {
        using var scope = _serviceScopeFactory.CreateScope();
        var scopedService = scope.ServiceProvider.GetRequiredService<IMutationService<CombatPlayerPositionDto>>();

        foreach (var item in combatPlayerPositions)
        {
            var combatPlayerPositionMap = _mapper.Map<CombatPlayerPositionDto>(item);
            combatPlayerPositionMap.CombatId = combatId;
            combatPlayerPositionMap.CombatPlayerId = combatPlayerId;

            var createdItem = await scopedService.CreateAsync(combatPlayerPositionMap);
            if (createdItem == null)
            {
                throw new ArgumentException("Did not created");
            }
        }
    }

    private async Task UploadCombatAuraData(List<CombatAura> combatAuras, int combatId)
    {
        using var scope = _serviceScopeFactory.CreateScope();
        var scopedService = scope.ServiceProvider.GetRequiredService<IMutationService<CombatAuraDto>>();

        foreach (var item in combatAuras)
        {
            var combatAuraMap = _mapper.Map<CombatAuraDto>(item);
            combatAuraMap.CombatId = combatId;

            var createdItem = await scopedService.CreateAsync(combatAuraMap);
            if (createdItem == null)
            {
                throw new ArgumentException("Did not created");
            }
        }
    }

    //private static async Task DeleteDataAsync<TServiceModel>(int combatPlayerId, IMutationService<TServiceModel> service)
    //    where TServiceModel : class
    //{
    //    var dataForRemove = await service.GetByParamAsync("CombatPlayerId", combatPlayerId);
    //    foreach (var item in dataForRemove)
    //    {
    //        var property = item.GetType().GetProperty("Id");
    //        var propertyValue = (int)property.GetValue(item);

    //        var rowsAffected = await service.DeleteAsync(propertyValue);
    //        if (rowsAffected == 0)
    //        {
    //            throw new ArgumentException($"Did not deleted");
    //        }
    //    }
    //}
}
