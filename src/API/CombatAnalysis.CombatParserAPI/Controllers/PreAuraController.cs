using CombatParser.Application.Queries.GetPreAuras;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CombatAnalysis.CombatParserAPI.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class PreAuraController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;


    [HttpGet("getByCombatId")]
    public async Task<IActionResult> GetByCombatId(int combatId, int combatPlayerId, CancellationToken cancellationToken)
    {
        var preAuras = await _mediator.Send(new GetPreAurasQuery(combatId, combatPlayerId), cancellationToken);

        return Ok(preAuras);
    }
}
