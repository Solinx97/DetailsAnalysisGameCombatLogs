using CombatParser.Application.Queries.GetCombatUnits;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CombatAnalysis.CombatParserAPI.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class CombatUnitController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpGet("getByCombatId/{combatId:int:min(1)}")]
    public async Task<IActionResult> GetByGamePlayerId(int combatId, CancellationToken cancellationToken)
    {
        var units = await _mediator.Send(new GetCombatUnitsQuery(combatId), cancellationToken);

        return Ok(units);
    }
}
