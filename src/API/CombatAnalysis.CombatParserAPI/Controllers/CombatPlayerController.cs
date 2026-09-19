using AutoMapper;
using CombatAnalysis.CombatParserAPI.Interfaces;
using CombatAnalysis.CombatParserAPI.Models.WoWMidnight;
using CombatAnalysis.CombatParserAPI.Models.WoWMoPClassic;
using CombatParser.Application.Queries.GetCombatPlayerById;
using CombatParser.Application.Queries.GetCombatPlayersByCombatId;
using CombatParser.Application.Queries.GetPlayerDeath;
using CombatParser.Application.Queries.GetPlayerDeathCount;
using CombatParser.Application.Queries.GetPlayerStats;
using CombatParser.Application.Queries.GetUniquePlayerNames;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CombatAnalysis.CombatParserAPI.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class CombatPlayerController(IMediator mediator, IMapper mapper) : ControllerBase
{
    private readonly IMediator _mediator = mediator;
    private readonly IMapper _mapper = mapper;

    [HttpGet("getUniquePlayerNames/{combatLogId:int:min(1)}")]
    public async Task<IActionResult> GetUniquePlayerNames(int combatLogId, CancellationToken cancellationToken)
    {
        var combatPlayerNames = await _mediator.Send(new GetUniquePlayerNamesQuery(combatLogId), cancellationToken);

        return Ok(combatPlayerNames);
    }

    [HttpGet("getByCombatId/{combatId:int:min(1)}")]
    public async Task<IActionResult> GetByCombatId(int combatId, CancellationToken cancellationToken)
    {
        var combatPlayers = await _mediator.Send(new GetCombatPlayersByCombatIdQuery(combatId), cancellationToken);

        return Ok(combatPlayers);
    }

    [HttpGet("{id:int:min(1)}")]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
    {
        var combatPlayer = await _mediator.Send(new GetCombatPlayerByIdQuery(id), cancellationToken);

        return Ok(combatPlayer);
    }

    [HttpGet("getPlayerStats/{combatPlayerId:int:min(1)}")]
    public async Task<IActionResult> GetPlayerStats(int combatPlayerId, int gameVersion, CancellationToken cancellationToken)
    {
        var stats = await _mediator.Send(new GetPlayerStatsQuery(combatPlayerId, gameVersion), cancellationToken);
        IPlayerStatsModel result = gameVersion switch
        {
            0 => _mapper.Map<WoWMoPClassicPlayerStatsModel>(stats),
            1 => _mapper.Map<WoWMidnightPlayerStatsModel>(stats),
            _ => throw new ArgumentOutOfRangeException(nameof(gameVersion))
        };

        return Ok(result);
    }

    [HttpGet("getPlayerDeathCount/{unitId}")]
    public async Task<IActionResult> GetPlayerDeathCount(string unitId, CancellationToken cancellationToken)
    {
        var playerDeathCount = await _mediator.Send(new GetPlayerDeathCountQuery(unitId), cancellationToken);
       
        return Ok(playerDeathCount);
    }

    [HttpGet("getPlayerDeath/{unitId}")]
    public async Task<IActionResult> GetPlayerDeath(string unitId, int skipCount, CancellationToken cancellationToken)
    {
        var playerDeath = await _mediator.Send(new GetPlayerDeathQuery(unitId, skipCount), cancellationToken);

        return Ok(playerDeath);
    }
}
