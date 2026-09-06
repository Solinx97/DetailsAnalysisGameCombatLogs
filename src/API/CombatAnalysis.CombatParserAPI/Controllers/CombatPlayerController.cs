using AutoMapper;
using CombatAnalysis.CombatParserAPI.Interfaces;
using CombatAnalysis.CombatParserAPI.Models.WoWMidnight;
using CombatAnalysis.CombatParserAPI.Models.WoWMoPClassic;
using CombatParser.Application.Queries.GetCombatPlayerById;
using CombatParser.Application.Queries.GetCombatPlayersByCombatId;
using CombatParser.Application.Queries.GetPlayerStats;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CombatAnalysis.CombatParserAPI.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class CombatPlayerController(IMediator mediator, IMapper mapper) : ControllerBase
{
    private readonly IMediator _mediator = mediator;
    private readonly IMapper _mapper = mapper;

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
}
