using AutoMapper;
using CombatAnalysis.CombatParserAPI.Interfaces;
using CombatAnalysis.CombatParserAPI.Models;
using CombatParser.Application.Commands.CreateCombat;
using CombatParser.Application.Queries.Dashboards.GetDamageSpells;
using CombatParser.Application.Queries.Dashboards.GetDashboard;
using CombatParser.Application.Queries.Dashboards.GetHealSpells;
using CombatParser.Application.Queries.Dashboards.GetPotions;
using CombatParser.Application.Queries.GetByIdCombat;
using CombatParser.Application.Queries.GetCombatsByCombatLogId;
using CombatParser.Domain.EntityData;
using CombatParser.Domain.EntityData.WoWMidnight;
using CombatParser.Domain.EntityData.WoWMoPClassic;
using CombatParser.Domain.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CombatAnalysis.CombatParserAPI.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class CombatController(IMapper mapper, ILogger<CombatController> logger, 
    ISpecializationScoreHelper scoreHelper, IMediator mediator) : ControllerBase
{
    private readonly ISpecializationScoreHelper _scoreHelper = scoreHelper;
    private readonly IMapper _mapper = mapper;
    private readonly ILogger<CombatController> _logger = logger;
    private readonly IMediator _mediator = mediator;

    [HttpGet("{id:int:min(1)}")]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
    {
        var combat = await _mediator.Send(new GetByIdCombatQuery(id), cancellationToken);

        return Ok(combat);
    }

    [HttpGet("getByCombatLogId/{combatLogId:int:min(1)}")]
    public async Task<IActionResult> GetByCombatLogId(int combatLogId, CancellationToken cancellationToken)
    {
        var combats = await _mediator.Send(new GetCombatsByCombatLogIdQuery(combatLogId), cancellationToken);

        return Ok(combats);
    }

    [HttpGet("getDashboards/{combatLogId:int:min(1)}")]
    public async Task<IActionResult> GetDashboards(int combatLogId, CancellationToken cancellationToken)
    {
        var dashboards = await _mediator.Send(new GetDashboardQuery(combatLogId), cancellationToken);

        return Ok(dashboards);
    }

    [HttpGet("getDamageSpells/{combatLogId:int:min(1)}")]
    public async Task<IActionResult> GetDamageSpells(int combatLogId, CancellationToken cancellationToken)
    {
        var spells = await _mediator.Send(new GetDamageSpellsQuery(combatLogId), cancellationToken);

        return Ok(spells);
    }

    [HttpGet("getHealSpells/{combatLogId:int:min(1)}")]
    public async Task<IActionResult> GetHealSpells(int combatLogId, CancellationToken cancellationToken)
    {
        var spells = await _mediator.Send(new GetHealSpellsQuery(combatLogId), cancellationToken);

        return Ok(spells);
    }

    [HttpGet("getPotions/{combatLogId:int:min(1)}")]
    public async Task<IActionResult> GetPotions(int combatLogId, CancellationToken cancellationToken)
    {
        var potions = await _mediator.Send(new GetPotionsQuery(combatLogId), cancellationToken);

        return Ok(potions);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateCombatModel combat, CancellationToken cancellationToken)
    {
        try
        {
            var combatPlayersData = new List<CombatParser.Domain.EntityData.CombatPlayerData>();
            foreach (var player in combat.CombatPlayers)
            {
                var unit = combat.Units.FirstOrDefault(x => x.GameId == player.Player.GameId);
                if (unit != null)
                {
                    var playerData = await ExtractCombatPlayerDataAsync(combat.GameVersion, unit, player, cancellationToken);
                    combatPlayersData.Add(playerData);
                }
            }

            var unitsData = new List<UnitData>();
            foreach (var item in combat.Units)
            {
                var unit = ExtractUnitDataAsync(item);
                unitsData.Add(unit);
            }

            var command = new CreateCombatCommand(combat.DungeonName, combat.BossHealthPercentage, combat.DamageDone, combat.HealDone, combat.DamageTaken, combat.ResourcesRecovery,
                 combat.IsWin, combat.StartDate, combat.FinishDate, combat.Boss.Id, combat.CombatLogId, combatPlayersData, unitsData);

            var combatId = await _mediator.Send(command, cancellationToken);

            return Ok(combatId);
        }
        catch (OperationCanceledException ex)
        {
            _logger.LogInformation(ex, "Operation was canceled by Client.");

            return StatusCode(499);
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Failed to create combat.");

            return StatusCode(500, "Internal server error.");
        }
    }

    private async Task<CombatParser.Domain.EntityData.CombatPlayerData> ExtractCombatPlayerDataAsync(int gameVersion, UnitModel unit, CreateCombatPlayerModel combatPlayer, CancellationToken cancellationToken)
    {
        IPlayerStatsData statsMap = gameVersion switch
        {
            0 => _mapper.Map<WoWMoPClassicPlayerStatsData>(combatPlayer.Stats),
            1 => _mapper.Map<WoWMidnightPlayerStatsData>(combatPlayer.Stats),
            _ => throw new ArgumentOutOfRangeException(nameof(gameVersion))
        };

        var spellIds = unit.UnitInfo.DamageDone > unit.UnitInfo.HealDone
            ? unit.DamageDones.Select(d => d.GameSpellId).ToArray()
            : [.. unit.HealDones.Select(d => d.GameSpellId)];

        await _scoreHelper.CreateSpecializationScoreAsync(combatPlayer, unit.UnitInfo, spellIds, cancellationToken);
        var scoreMap = _mapper.Map<SpecializationScoreData>(combatPlayer.Score);

        var playerData = new CombatParser.Domain.EntityData.CombatPlayerData(
            combatPlayer.AverageItemLevel,
            combatPlayer.Player.Id,
            statsMap,
            scoreMap,
            combatPlayer.UnitGameId
        );

        return playerData;
    }

    private UnitData ExtractUnitDataAsync(UnitModel unit)
    {
        var unitInfoMap = _mapper.Map<UnitInfoData>(unit.UnitInfo);
        var unitHealthesMap = _mapper.Map<List<UnitHealthData>>(unit.UnitHealthes);
        var unitCastsMap = _mapper.Map<List<UnitCastData>>(unit.UnitCasts);
        var unitPositionsMap = _mapper.Map<List<UnitPositionData>>(unit.UnitPositions);
        var preAurasMap = _mapper.Map<List<UnitPreAuraData>>(unit.PreAuras);
        var aurasMap = _mapper.Map<List<UnitAuraData>>(unit.Auras);
        var damageDonesMap = _mapper.Map<List<DamageDoneData>>(unit.DamageDones);
        var damageDoneGeneralsMap = _mapper.Map<List<DamageDoneGeneralData>>(unit.DamageDoneGenerals);
        var healDonesMap = _mapper.Map<List<HealDoneData>>(unit.HealDones);
        var healDoneGeneralsMap = _mapper.Map<List<HealDoneGeneralData>>(unit.HealDoneGenerals);
        var resourceRecoveryMap = _mapper.Map<List<ResourceRecoveryData>>(unit.ResourceRecoveries);
        var resourceRecoveryGeneralMap = _mapper.Map<List<ResourceRecoveryGeneralData>>(unit.ResourceRecoveryGenerals);

        var unitData = new UnitData(
            unit.GameId,
            unit.Name,
            unit.UnitHash,
            unit.Type,
            unit.CreatorGameId,
            unit.CombatId,
            unitInfoMap,
            unitHealthesMap,
            unitCastsMap,
            unitPositionsMap,
            preAurasMap,
            aurasMap,
            damageDonesMap,
            damageDoneGeneralsMap,
            healDonesMap,
            healDoneGeneralsMap,
            resourceRecoveryMap,
            resourceRecoveryGeneralMap
        );

        return unitData;
    }
}
