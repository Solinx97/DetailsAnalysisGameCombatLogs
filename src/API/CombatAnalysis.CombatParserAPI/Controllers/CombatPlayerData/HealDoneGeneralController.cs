using CombatParser.Application.Queries.GetHealGenerals;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CombatAnalysis.CombatParserAPI.Controllers.CombatPlayerData;

[Route("api/v1/[controller]")]
[ApiController]
public class HealDoneGeneralController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpGet("getByUnitId/{unitId}")]
    public async Task<IActionResult> GetByUnitId(string unitId, CancellationToken cancellationToken)
    {
        var healGenerals = await _mediator.Send(new GetHealGeneralsQuery(unitId), cancellationToken);

        return Ok(healGenerals);
    }
}
