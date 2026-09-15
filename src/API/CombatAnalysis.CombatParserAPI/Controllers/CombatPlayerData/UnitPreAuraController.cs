using CombatParser.Application.Queries.GetPreAuras;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CombatAnalysis.CombatParserAPI.Controllers.CombatPlayerData;

[Route("api/v1/[controller]")]
[ApiController]
public class UnitPreAuraController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;


    [HttpGet("getByCombatId/{combatId:int:min(1)}")]
    public async Task<IActionResult> GetByCombatId(int combatId, string unitId, CancellationToken cancellationToken)
    {
        var preAuras = await _mediator.Send(new GetPreAurasQuery(combatId, unitId), cancellationToken);

        return Ok(preAuras);
    }
}
