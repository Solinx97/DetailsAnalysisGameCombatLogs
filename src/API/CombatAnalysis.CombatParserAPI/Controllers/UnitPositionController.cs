using CombatParser.Application.Queries.GetUnitPositionById;
using CombatParser.Application.Queries.GetUnitPositions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CombatAnalysis.CombatParserAPI.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class UnitPositionController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpGet("getByCombatId/{combatId:int:min(1)}")]
    public async Task<IActionResult> GetByCombatId(int combatId, CancellationToken cancellationToken)
    {
        var unitPositions = await _mediator.Send(new GetUnitPositionsQuery(combatId), cancellationToken);

        return Ok(unitPositions);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(string id, CancellationToken cancellationToken)
    {
        var unitPosition = await _mediator.Send(new GetUnitPositionByIdQuery(id), cancellationToken);

        return Ok(unitPosition);
    }
}
