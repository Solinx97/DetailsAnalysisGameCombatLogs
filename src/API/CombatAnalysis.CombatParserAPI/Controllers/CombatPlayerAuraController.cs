using CombatParser.Application.Queries.GetAuraById;
using CombatParser.Application.Queries.GetAurasByCombatId;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CombatAnalysis.CombatParserAPI.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class CombatPlayerAuraController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpGet("getByCombatId")]
    public async Task<IActionResult> GetByCombatId(int combatId, int combatPlayerId, CancellationToken cancellationToken)
    {
        var auras = await _mediator.Send(new GetAurasByCombatIdQuery(combatId, combatPlayerId), cancellationToken);

        return Ok(auras);
    }

    [HttpGet("{id:int:min(1)}")]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
    {
        var aura = await _mediator.Send(new GetAuraByIdQuery(id), cancellationToken);

        return Ok(aura);
    }
}
