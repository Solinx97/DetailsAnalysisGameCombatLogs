using CombatParser.Application.Queries.GetDamageGenerals.GetDamageTakenGenerals;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CombatAnalysis.CombatParserAPI.Controllers.CombatPlayerData;

[Route("api/v1/[controller]")]
[ApiController]
public class DamageTakenGeneralController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpGet("getByUnitId/{unitId}")]
    public async Task<IActionResult> GetByUnitId(string unitId, int combatId, CancellationToken cancellationToken)
    {
        var damageGenerals = await _mediator.Send(new GetDamageTakenGeneralsQuery(unitId, combatId), cancellationToken);

        return Ok(damageGenerals);
    }
}
