using CombatParser.Application.Queries.GetDamageGenerals;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CombatAnalysis.CombatParserAPI.Controllers.CombatPlayerData;

[Route("api/v1/[controller]")]
[ApiController]
public class DamageDoneGeneralController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpGet("getByUnitId/{unitId}")]
    public async Task<IActionResult> GetByUnitId(string unitId, int combatId, CancellationToken cancellationToken)
    {
        var damageGenerals = await _mediator.Send(new GetDamageGeneralsQuery(unitId, combatId), cancellationToken);

        return Ok(damageGenerals);
    }
}
