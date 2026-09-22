using CombatParser.Application.Queries.GetPreAuras;
using CombatParser.Application.Queries.GetUnitPreAuras;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CombatAnalysis.CombatParserAPI.Controllers.CombatPlayerData;

[Route("api/v1/[controller]")]
[ApiController]
public class UnitPreAuraController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;


    [HttpGet("getByCombatId/{combatId:int:min(1)}")]
    public async Task<IActionResult> GetByCombatId(int combatId, CancellationToken cancellationToken)
    {
        var preAuras = await _mediator.Send(new GetPreAurasQuery(combatId), cancellationToken);

        return Ok(preAuras);
    }

    [HttpGet("getByUnitId/{combatId:int:min(1)}")]
    public async Task<IActionResult> GetByUnitId(int combatId, string unitId, CancellationToken cancellationToken)
    {
        var preAuras = await _mediator.Send(new GetUnitPreAurasQuery(combatId, unitId), cancellationToken);

        return Ok(preAuras);
    }
}
