using AutoMapper;
using CombatAnalysis.BL.DTO;
using CombatAnalysis.BL.Interfaces;
using CombatAnalysis.BL.Interfaces.General;
using CombatAnalysis.CombatParserAPI.Interfaces;
using CombatAnalysis.CombatParserAPI.Models;
using CombatParser.Application.Commands.CreateCombat;
using CombatParser.Domain.EntityData;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CombatAnalysis.CombatParserAPI.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class CombatController(IBossService bossService, IQueryService<CombatDto> queryCombatService, IMapper mapper, ILogger<CombatController> logger, 
    ISpecializationScoreHelper scoreHelper, IMediator mediator) : ControllerBase
{
    private readonly IBossService _bossService = bossService;
    private readonly IQueryService<CombatDto> _queryCombatService = queryCombatService;
    private readonly ISpecializationScoreHelper _scoreHelper = scoreHelper;
    private readonly IMapper _mapper = mapper;
    private readonly ILogger<CombatController> _logger = logger;
    private readonly IMediator _mediator = mediator;

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var combats = await _queryCombatService.GetAllAsync(cancellationToken);

        return Ok(combats);
    }

    [HttpGet("{id:int:min(1)}")]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
    {
        var combat = await _queryCombatService.GetByIdAsync(id, cancellationToken);

        return Ok(combat);
    }

    [HttpGet("getByCombatLogId/{combatLogId:int:min(1)}")]
    public async Task<IActionResult> GetByCombatLogId(int combatLogId, CancellationToken cancellationToken)
    {
        var combats = await _queryCombatService.GetByParamAsync(nameof(CombatModel.CombatLogId), combatLogId, cancellationToken);
        var map = _mapper.Map<IEnumerable<CombatModel>>(combats);
        foreach (var item in map)
        {
            var boss = await _bossService.GetById(item.Boss.Id, cancellationToken);
            var bossMap = _mapper.Map<BossModel>(boss);

            item.UpdateBoss(bossMap);
        }

        return Ok(map);
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

            var combatPlayers = new List<CombatPlayerData>();
            var auras = _mapper.Map<List<CombatAuraData>>(combat.CombatAuras);

            await ExtractCombatPlayerDataAsync(combat, combatPlayers, auras, cancellationToken);

            var command = new CreateCombatCommand(combat.DungeonName, combat.BossHealthPercentage, combat.DamageDone, combat.HealDone, combat.DamageTaken, combat.ResourcesRecovery,
                 combat.IsWin, combat.StartDate, combat.FinishDate, combat.Boss.Id, combat.CombatLogId, combatPlayers, auras);

            var createdCombat = await _mediator.Send(command, cancellationToken);

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

    private async Task ExtractCombatPlayerDataAsync(CombatModel combat, List<CombatPlayerData> combatPlayers, List<CombatAuraData> auras, CancellationToken cancellationToken)
    {
        foreach (var combatPlayer in combat.CombatPlayers)
        {
            var statsMap = _mapper.Map<CombatPlayerStatsData>(combatPlayer.Stats);

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

            combatPlayers.Add(new CombatPlayerData(
                combatPlayer.AverageItemLevel,
                combatPlayer.ResourcesRecovery,
                combatPlayer.DamageDone,
                combatPlayer.HealDone,
                combatPlayer.DamageTaken,
                combatPlayer.PlayerId,
                combatPlayer.CombatId,
                statsMap,
                scoreMap,
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
            ));
        }
    }
}
