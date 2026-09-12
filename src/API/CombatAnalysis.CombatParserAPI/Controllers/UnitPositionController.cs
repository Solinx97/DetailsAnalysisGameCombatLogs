using CombatParser.Application.Queries.GetUnitPositions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CombatAnalysis.CombatParserAPI.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class UnitPositionController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpGet("getByCombatUnitId/{combatUnitId}")]
    public async Task<IActionResult> GetByCombatId(string combatUnitId, CancellationToken cancellationToken)
    {
        var unitPositions = await _mediator.Send(new GetUnitPositionsQuery(combatUnitId), cancellationToken);

        return Ok(unitPositions);
    }
}
