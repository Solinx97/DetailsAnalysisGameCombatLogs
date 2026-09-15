using CombatParser.Application.Queries.GetDamageGenerals;
using CombatParser.Application.Queries.GetDamageGenerals.GetDamageTakenGenerals;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CombatAnalysis.CombatParserAPI.Controllers.CombatPlayerData;

[Route("api/v1/[controller]")]
[ApiController]
public class DamageDoneGeneralController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpGet("getByUnitId/{unitId}")]
    public async Task<IActionResult> GetByUnitId(string unitId, CancellationToken cancellationToken)
    {
        var damageGenerals = await _mediator.Send(new GetDamageGeneralsQuery(unitId), cancellationToken);

        return Ok(damageGenerals);
    }

    [HttpGet("getDamageTakenByUnitId/{unitId}")]
    public async Task<IActionResult> GetDamageTakenByUnitId(string unitId, CancellationToken cancellationToken)
    {
        var damageTakenGenerals = await _mediator.Send(new GetDamageTakenGeneralsQuery(unitId), cancellationToken);

        return Ok(damageTakenGenerals);
    }
}
