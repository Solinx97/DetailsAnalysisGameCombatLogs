using AutoMapper;
using CombatAnalysis.CombatParserAPI.Interfaces;
using CombatAnalysis.CombatParserAPI.Models;
using CombatParser.Application.Commands.CreateCombat;
using CombatParser.Application.Queries.GetByIdCombat;
using CombatParser.Application.Queries.GetCombatsByCombatLogId;
using CombatParser.Domain.EntityData;
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

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CombatModel combat, CancellationToken cancellationToken)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid Combat create received: {@Combat}", combat);

                return ValidationProblem(ModelState);
            }

            var combatPlayersData = new List<CombatPlayerData>();
            foreach (var player in combat.CombatPlayers)
            {
                var playerData = await ExtractCombatPlayerDataAsync(player, cancellationToken);
                combatPlayersData.Add(playerData);
            }

            var command = new CreateCombatCommand(combat.DungeonName, combat.BossHealthPercentage, combat.DamageDone, combat.HealDone, combat.DamageTaken, combat.ResourcesRecovery,
                 combat.IsWin, combat.StartDate, combat.FinishDate, combat.Boss.Id, combat.CombatLogId, combatPlayersData);

            await _mediator.Send(command, cancellationToken);

            return Ok();
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

    private async Task<CombatPlayerData> ExtractCombatPlayerDataAsync(CombatPlayerModel combatPlayer, CancellationToken cancellationToken)
    {
        var statsMap = _mapper.Map<CombatPlayerStatsData>(combatPlayer.Stats);

        var aurasMap = _mapper.Map<List<CombatPlayerAuraData>>(combatPlayer.Auras);
        var damageDonesMap = _mapper.Map<List<DamageDoneData>>(combatPlayer.DamageDones);
        var damageDoneGeneralsMap = _mapper.Map<List<DamageDoneGeneralData>>(combatPlayer.DamageDoneGenerals);
        var healDonesMap = _mapper.Map<List<HealDoneData>>(combatPlayer.HealDones);
        var healDoneGeneralsMap = _mapper.Map<List<HealDoneGeneralData>>(combatPlayer.HealDoneGenerals);
        var damageTakenMap = _mapper.Map<List<DamageTakenData>>(combatPlayer.DamageTakens);
        var damageTakenGeneralsMap = _mapper.Map<List<DamageTakenGeneralData>>(combatPlayer.DamageTakenGenerals);
        var resourceRecoveryMap = _mapper.Map<List<ResourceRecoveryData>>(combatPlayer.ResourceRecoveries);
        var resourceRecoveryGeneralMap = _mapper.Map<List<ResourceRecoveryGeneralData>>(combatPlayer.ResourceRecoveryGenerals);
        var deathsMap = _mapper.Map<List<CombatPlayerDeathData>>(combatPlayer.CombatPlayerDeathes);
        var positionsMap = _mapper.Map<List<CombatPlayerPositionData>>(combatPlayer.CombatPlayerPositions);

        var spellIds = combatPlayer.DamageDone > combatPlayer.HealDone
            ? combatPlayer.DamageDones.Select(d => d.GameSpellId).ToArray()
            : [.. combatPlayer.HealDones.Select(d => d.GameSpellId)];

        await _scoreHelper.CreateSpecializationScoreAsync(combatPlayer, spellIds, cancellationToken);
        var scoreMap = _mapper.Map<SpecializationScoreData>(combatPlayer.Score);

        var playerData = new CombatPlayerData(
            combatPlayer.AverageItemLevel,
            combatPlayer.ResourcesRecovery,
            combatPlayer.DamageDone,
            combatPlayer.HealDone,
            combatPlayer.DamageTaken,
            combatPlayer.PlayerId,
            combatPlayer.CombatId,
            statsMap,
            scoreMap,
            aurasMap,
            damageDonesMap,
            damageDoneGeneralsMap,
            healDonesMap,
            healDoneGeneralsMap,
            damageTakenMap,
            damageTakenGeneralsMap,
            resourceRecoveryMap,
            resourceRecoveryGeneralMap,
            deathsMap,
            positionsMap
        );

        return playerData;
    }
}
