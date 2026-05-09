using CombatParser.Application.Queries.GetDamageTakenGenerals;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CombatAnalysis.CombatParserAPI.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class DamageTakenGeneralController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpGet("getByCombatPlayerId/{combatPlayerId:int:min(1)}")]
    public async Task<IActionResult> GetByCombatPlayerId(int combatPlayerId, CancellationToken cancellationToken)
    {
        var damageTakenGenerals = await _mediator.Send(new GetDamageTakenGeneralsQuery(combatPlayerId), cancellationToken);

        return Ok(damageTakenGenerals);
    }
}
