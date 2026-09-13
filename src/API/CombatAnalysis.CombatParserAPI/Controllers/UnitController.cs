using CombatParser.Application.Queries.GetUnits;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CombatAnalysis.CombatParserAPI.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class UnitController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpGet("getByCombatId/{combatId:int:min(0)}")]
    public async Task<IActionResult> GetByGamePlayerId(int combatId, CancellationToken cancellationToken)
    {
        var units = await _mediator.Send(new GetUnitsQuery(combatId), cancellationToken);

        return Ok(units);
    }
}
