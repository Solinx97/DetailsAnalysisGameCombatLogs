using CombatParser.Application.Queries.GetAuraById;
using CombatParser.Application.Queries.GetAurasByCombatId;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CombatAnalysis.CombatParserAPI.Controllers.CombatPlayerData;

[Route("api/v1/[controller]")]
[ApiController]
public class UnitAuraController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpGet("getByCombatId")]
    public async Task<IActionResult> GetByCombatId(int combatId, string unitId, CancellationToken cancellationToken)
    {
        var auras = await _mediator.Send(new GetAurasByCombatIdQuery(combatId, unitId), cancellationToken);

        return Ok(auras);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(string id, CancellationToken cancellationToken)
    {
        var aura = await _mediator.Send(new GetAuraByIdQuery(id), cancellationToken);

        return Ok(aura);
    }
}
