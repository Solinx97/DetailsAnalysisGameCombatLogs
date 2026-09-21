using CombatParser.Application.Queries.CombatPlayer.GetPlayerHealthesBeforeDied;
using CombatParser.Application.Queries.GetUnitsHealth;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CombatAnalysis.CombatParserAPI.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class UnitHealthController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpGet("getByCombatId/{combatId:int:min(1)}")]
    public async Task<IActionResult> GetByCombatId(int combatId, CancellationToken cancellationToken)
    {
        var unitsHealth = await _mediator.Send(new GetUnitsHealthQuery(combatId), cancellationToken);

        return Ok(unitsHealth);
    }

    [HttpGet("getBeforeDied/{unitId}")]
    public async Task<IActionResult> GetBeforeDied(string unitId, string whenDied, CancellationToken cancellationToken)
    {
        var unitsHealthBeforePlayerDied = await _mediator.Send(new GetPlayerHealthesBeforeDiedQuery(unitId, whenDied), cancellationToken);

        return Ok(unitsHealthBeforePlayerDied);
    }
}
