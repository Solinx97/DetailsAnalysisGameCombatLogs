using CombatParser.Application.Queries.GetCombatPlayerPositionById;
using CombatParser.Application.Queries.GetCombatPlayerPositions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CombatAnalysis.CombatParserAPI.Controllers.CombatPlayerData;

[Route("api/v1/[controller]")]
[ApiController]
public class CombatPlayerPositionController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpGet("getByCombatPlayerId/{combatPlayerId:int:min(1)}")]
    public async Task<IActionResult> GetByCombatPlayerId(int combatPlayerId, CancellationToken cancellationToken)
    {
        var combatPlayerPositions = await _mediator.Send(new GetCombatPlayerPositionsQuery(combatPlayerId), cancellationToken);

        return Ok(combatPlayerPositions);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(string id, CancellationToken cancellationToken)
    {
        var combatPlayerPosition = await _mediator.Send(new GetCombatPlayerPositionByIdQuery(id), cancellationToken);

        return Ok(combatPlayerPosition);
    }
}
