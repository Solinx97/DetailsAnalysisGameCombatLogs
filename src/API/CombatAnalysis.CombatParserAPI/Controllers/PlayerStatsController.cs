using CombatParser.Application.Queries.GetCombatPlayerStat;
using CombatParser.Application.Queries.GetCombatPlayerStatById;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CombatAnalysis.CombatParserAPI.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class PlayerStatsController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpGet("getByCombatPlayerId/{combatPlayerId:int:min(1)}")]
    public async Task<IActionResult> GetByCombatPlayerId(int combatPlayerId, CancellationToken cancellationToken)
    {
        var playerStats = await _mediator.Send(new GetCombatPlayerStatQuery(combatPlayerId), cancellationToken);

        return Ok(playerStats);
    }

    [HttpGet("{id:int:min(1)}")]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
    {
        var playerStats = await _mediator.Send(new GetCombatPlayerStatByIdQuery(id), cancellationToken);

        return Ok(playerStats);
    }
}
