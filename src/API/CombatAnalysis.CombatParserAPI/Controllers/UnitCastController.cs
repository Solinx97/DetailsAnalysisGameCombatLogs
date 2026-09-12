using CombatParser.Application.Queries.GetUnitCasts;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CombatAnalysis.CombatParserAPI.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class UnitCastController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpGet("getByCombatUnitId/{combatId}")]
    public async Task<IActionResult> GetByCombatId(string combatUnitId, CancellationToken cancellationToken)
    {
        var casts = await _mediator.Send(new GetUnitCastsQuery(combatUnitId), cancellationToken);

        return Ok(casts);
    }
}
