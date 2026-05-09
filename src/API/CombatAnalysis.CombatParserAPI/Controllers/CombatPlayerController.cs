using CombatParser.Application.Queries.GetCombatPlayerById;
using CombatParser.Application.Queries.GetCombatPlayersByCombatId;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CombatAnalysis.CombatParserAPI.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class CombatPlayerController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

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
}
