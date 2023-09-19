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
    private readonly ILogger _logger;
    private readonly IService<DamageDoneDto, int> _damageDoneService;
    private readonly IService<DamageDoneGeneralDto, int> _damageDoneGeneralService;
    private readonly IService<HealDoneDto, int> _healDoneService;
    private readonly IService<HealDoneGeneralDto, int> _healDoneGeneralService;
    private readonly IService<DamageTakenDto, int> _damageTakenService;
    private readonly IService<DamageTakenGeneralDto, int> _damageTakenGeneralService;
    private readonly IService<ResourceRecoveryDto, int> _resourceRecoveryService;
    private readonly IService<ResourceRecoveryGeneralDto, int> _resourceRecoveryGeneralService;

    public SaveCombatDataHelper(IMapper mapper, ILogger logger, IService<DamageDoneDto, int> damageDoneService, IService<DamageDoneGeneralDto, int> damageDoneGeneralService, 
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
        damageDoneDetails.GetData(combatPlayer.UserName, combatData);

        var damageDoneGeneralData = damageDoneDetails.GetDamageDoneGeneral(damageDoneDetails.DamageDone, parsedCombat);

        await UploadDamageDoneAsync(damageDoneDetails.DamageDone, combatPlayer.Id);
        await UploadDamageDoneGeneralAsync(damageDoneGeneralData.ToList(), combatPlayer.Id);

        var healDoneDetails = new CombatDetailsHealDone(_logger);
        healDoneDetails.GetData(combatPlayer.UserName, combatData);

        var healDoneGeneralData = healDoneDetails.GetHealDoneGeneral(healDoneDetails.HealDone, parsedCombat);

        await UploadHealDoneAsync(healDoneDetails.HealDone, combatPlayer.Id);
        await UploadHealDoneGeneralAsync(healDoneGeneralData.ToList(), combatPlayer.Id);

        var damageTakenDetails = new CombatDetailsDamageTaken(_logger);
        damageTakenDetails.GetData(combatPlayer.UserName, combatData);

        var damageTakenGeneralData = damageTakenDetails.GetDamageTakenGeneral(damageTakenDetails.DamageTaken, parsedCombat);

        await UploadDamageTakenAsync(damageTakenDetails.DamageTaken, combatPlayer.Id);
        await UploadDamageTakenGeneralAsync(damageTakenGeneralData.ToList(), combatPlayer.Id);

        var resourceRecoveryDetails = new CombatDetailsResourceRecovery(_logger);
        resourceRecoveryDetails.GetData(combatPlayer.UserName, combatData);

        var resourceRecoveryGeneralData = resourceRecoveryDetails.GetResourceRecoveryGeneral(resourceRecoveryDetails.ResourceRecovery, parsedCombat);

        await UploadResorceRecoveryAsync(resourceRecoveryDetails.ResourceRecovery, combatPlayer.Id);
        await UploadResourceRecoveryGeneralAsync(resourceRecoveryGeneralData.ToList(), combatPlayer.Id);
    }

    private async Task UploadDamageDoneAsync(List<DamageDone> damageDoneList, int combatPlayerId)
    {
        foreach (var item in damageDoneList)
        {
            var map = _mapper.Map<DamageDoneDto>(item);
            map.CombatPlayerId = combatPlayerId;

            var createdItem = await _damageDoneService.CreateAsync(map);
            if (createdItem == null)
            {
                throw new ArgumentException("Damage done did not created");
            }
        }
    }

    private async Task UploadDamageDoneGeneralAsync(List<DamageDoneGeneral> damageDoneGeneralList, int combatPlayerId)
    {
        foreach (var item in damageDoneGeneralList)
        {
            var map = _mapper.Map<DamageDoneGeneralDto>(item);
            map.CombatPlayerId = combatPlayerId;

            var createdItem = await _damageDoneGeneralService.CreateAsync(map);
            if (createdItem == null)
            {
                throw new ArgumentException("Damage done general did not created");
            }
        }
    }

    private async Task UploadHealDoneAsync(List<HealDone> healDoneList, int combatPlayerId)
    {
        foreach (var item in healDoneList)
        {
            var map = _mapper.Map<HealDoneDto>(item);
            map.CombatPlayerId = combatPlayerId;

            var createdItem = await _healDoneService.CreateAsync(map);
            if (createdItem == null)
            {
                throw new ArgumentException("Heal done did not created");
            }
        }
    }

    private async Task UploadHealDoneGeneralAsync(List<HealDoneGeneral> healDoneGeneralList, int combatPlayerId)
    {
        foreach (var item in healDoneGeneralList)
        {
            var map = _mapper.Map<HealDoneGeneralDto>(item);
            map.CombatPlayerId = combatPlayerId;

            var createdItem = await _healDoneGeneralService.CreateAsync(map);
            if (createdItem == null)
            {
                throw new ArgumentException("Heal done general did not created");
            }
        }
    }

    private async Task UploadDamageTakenAsync(List<DamageTaken> damageTakenList, int combatPlayerId)
    {
        foreach (var item in damageTakenList)
        {
            var map = _mapper.Map<DamageTakenDto>(item);
            map.CombatPlayerId = combatPlayerId;

            var createdItem = await _damageTakenService.CreateAsync(map);
            if (createdItem == null)
            {
                throw new ArgumentException("Damage taken did not created");
            }
        }
    }

    private async Task UploadDamageTakenGeneralAsync(List<DamageTakenGeneral> damageTakenGeneralList, int combatPlayerId)
    {
        foreach (var item in damageTakenGeneralList)
        {
            var map = _mapper.Map<DamageTakenGeneralDto>(item);
            map.CombatPlayerId = combatPlayerId;

            var createdItem = await _damageTakenGeneralService.CreateAsync(map);
            if (createdItem == null)
            {
                throw new ArgumentException("Damage taken general did not created");
            }
        }
    }

    private async Task UploadResorceRecoveryAsync(List<ResourceRecovery> resourceRecoveryList, int combatPlayerId)
    {
        foreach (var item in resourceRecoveryList)
        {
            var map = _mapper.Map<ResourceRecoveryDto>(item);
            map.CombatPlayerId = combatPlayerId;

            var createdItem = await _resourceRecoveryService.CreateAsync(map);
            if (createdItem == null)
            {
                throw new ArgumentException("Resource recovery did not created");
            }
        }
    }

    private async Task UploadResourceRecoveryGeneralAsync(List<ResourceRecoveryGeneral> resourceRecoveryGeneralList, int combatPlayerId)
    {
        foreach (var item in resourceRecoveryGeneralList)
        {
            var map = _mapper.Map<ResourceRecoveryGeneralDto>(item);
            map.CombatPlayerId = combatPlayerId;

            var createdItem = await _resourceRecoveryGeneralService.CreateAsync(map);
            if (createdItem == null)
            {
                throw new ArgumentException("Resource recovery general did not created");
            }
        }
    }
}
