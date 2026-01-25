using AutoMapper;
using CombatAnalysis.BL.DTO;
using CombatAnalysis.BL.Interfaces;
using CombatAnalysis.BL.Interfaces.General;
using CombatAnalysis.CombatParser.Details;
using CombatAnalysis.CombatParserAPI.Interfaces;
using CombatAnalysis.CombatParserAPI.Models;
using CombatAnalysis.DAL.Entities;
using CombatParser.Application.Commands.CreateCombat;
using CombatParser.Application.Commands.CreateCombatLog;
using CombatParser.Domain.EntityData;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CombatAnalysis.CombatParserAPI.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class CombatController(IBossService bossService, IQueryService<CombatDto> queryCombatService, IMutationService<CombatDto> mutationCombatService,
    ICombatPlayerService combatPlayerService, IMapper mapper, ILogger<CombatController> logger, 
    ISpecializationScoreHelper scoreHelper, ICombatDataHelper combatDataHelper, ICombatTransactionService combatTransactionService,
    IMediator mediator) : ControllerBase
{
    private readonly IBossService _bossService = bossService;
    private readonly IQueryService<CombatDto> _queryCombatService = queryCombatService;
    private readonly IMutationService<CombatDto> _mutationCombatService = mutationCombatService;
    private readonly ICombatPlayerService _combatPlayerService = combatPlayerService;
    private readonly ISpecializationScoreHelper _scoreHelper = scoreHelper;
    private readonly IMapper _mapper = mapper;
    private readonly ILogger<CombatController> _logger = logger;
    private readonly ICombatDataHelper _combatDataHelper = combatDataHelper;
    private readonly ICombatTransactionService _combatTransactionService = combatTransactionService;
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

            item.Boss = bossMap;
        }

        return Ok(map);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CombatModel combat, CancellationToken cancellationToken)
    {
        try
        {
            // A huge transaction for all action as ONE TRANSACTION was divided into a few small transactions.
            // Transactions split by logic: combat/combatPlayer/combatPlayerData and combatPlayerSpecScore/bestSpecScore

            // Transaction to create Combat and Combat Players
            //await _combatTransactionService.BeginTransactionAsync();
            //cancellationToken.ThrowIfCancellationRequested();

            var combatDetails = _combatDataHelper.CreateCombatDetails(combat);
            //var combatDetails = await CreateCombatPlayersAsync(combat, cancellationToken);

            //await _combatTransactionService.CommitTransactionAsync();

            //// Transaction to create Combat player data and specialization score:
            //// 1) damage, heal, damage taken, resources, etc
            //// 2) how combat player do mechanics, class rotation, assist other combat players, etc
            //await _combatTransactionService.BeginTransactionAsync();
            //cancellationToken.ThrowIfCancellationRequested();

            //var createdCombatPlayers = await _combatPlayerService.GetByCombatIdAsync(createdCombat.Id, cancellationToken);
            //await _combatDataHelper.CreateCombatPlayersDataAsync(combatDetails, [.. createdCombatPlayers], createdCombat.Id, cancellationToken);
            //await _combatDataHelper.UpdateSpecializationScoreAsync([.. createdCombatPlayers], combatDetails, combat.Boss.Id, cancellationToken);

            var combatPlayers = new List<CombatPlayerData>();

            foreach (var item in combat.CombatPlayers)
            {
                var statsMap = _mapper.Map<CombatPlayerStatsData>(item.Stats);
                var scoreMap = _mapper.Map<SpecializationScoreData>(item.Score);
                var damageDonesMap = _mapper.Map<List<DamageDoneData>>(combatDetails.DamageDone[item.Player.GameId].Select(x => x.Value));
                var damageDoneGeneralsMap = _mapper.Map<List<DamageDoneGeneralData>>(combatDetails.DamageDoneGeneral[item.Player.GameId]);
                var healDonesMap = _mapper.Map<List<HealDoneData>>(combatDetails.HealDone[item.Player.GameId].Select(x => x.Value));
                var healDoneGeneralsMap = _mapper.Map<List<HealDoneGeneralData>>(combatDetails.HealDoneGeneral[item.Player.GameId]);
                var damageTakenMap = _mapper.Map<List<DamageTakenData>>(combatDetails.DamageTaken[item.Player.GameId].Select(x => x.Value));
                var damageTakenGeneralsMap = _mapper.Map<List<DamageTakenGeneralData>>(combatDetails.DamageTakenGeneral[item.Player.GameId]);
                var resourceRecoveryMap = _mapper.Map<List<ResourceRecoveryData>>(combatDetails.ResourcesRecovery[item.Player.GameId].Select(x => x.Value));
                var resourceRecoveryGeneralMap = _mapper.Map<List<ResourceRecoveryGeneralData>>(combatDetails.ResourcesRecoveryGeneral[item.Player.GameId]);
                var deathsMap = _mapper.Map<List<CombatPlayerDeathData>>(combatDetails.PlayersDeath[item.Player.GameId].Select(x => x.Value));
                var positionsMap = _mapper.Map<List<CombatPlayerPositionData>>(combatDetails.Positions[item.Player.GameId].Select(x => x.Value));

                combatPlayers.Add(new CombatPlayerData(
                    item.AverageItemLevel,
                    item.ResourcesRecovery,
                    item.DamageDone,
                    item.HealDone,
                    item.DamageTaken,
                    item.PlayerId,
                    item.CombatId,
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

            var command = new CreateCombatCommand(combat.DungeonName, combat.BossHealthPercentage, combat.DamageDone, combat.HealDone, combat.DamageTaken, combat.ResourcesRecovery,
                 combat.IsWin, combat.StartDate, combat.FinishDate, combat.Boss.Id, combat.CombatLogId, combatPlayers);

            var createdCombat = await _mediator.Send(command, cancellationToken);

            ////createdCombat.IsReady = true;

            //await _mutationCombatService.UpdateAsync(createdCombat, cancellationToken);

            //await _combatTransactionService.CommitTransactionAsync();

            return Ok(createdCombat);
        }
        catch (OperationCanceledException ex)
        {
            //await _combatTransactionService.RollbackTransactionAsync();

            _logger.LogInformation(ex, "Operation was canceled by Client.");

            return StatusCode(499);
        }
        catch (DbUpdateException ex)
        {
            //await _combatTransactionService.RollbackTransactionAsync();

            _logger.LogError(ex, "Failed to create combat.");

            return StatusCode(500, "Internal server error.");
        }
    }

    private async Task<CombatDto> CreateCombatAsync(CombatModel model, CancellationToken cancellationToken)
    {
        var map = _mapper.Map<CombatDto>(model);
        var createdCombat = await _mutationCombatService.CreateAsync(map, cancellationToken);
        ArgumentNullException.ThrowIfNull(createdCombat, nameof(createdCombat));

        return createdCombat;
    }

    private async Task<CombatDetails> CreateCombatPlayersAsync(CombatModel combat, CancellationToken cancellationToken)
    {
        var combatDetails = _combatDataHelper.CreateCombatDetails(combat);

        combat.CombatPlayers = [.. combat.CombatPlayers.Select(cp =>
        {
            cp.CombatId = combat.Id;

            return cp;
        })];

        var map = _mapper.Map<IEnumerable<CombatPlayerDto>>(combat.CombatPlayers);
        foreach (var combatPlayer in map)
        {
            await _scoreHelper.CreateSpecializationScoreAsync(combatPlayer, combatDetails, cancellationToken);
        }
        
        await _combatPlayerService.CreateBatchAsync(map, cancellationToken);

        return combatDetails;
    }
}
