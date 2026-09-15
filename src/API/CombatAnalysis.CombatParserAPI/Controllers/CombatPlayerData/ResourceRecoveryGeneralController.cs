using CombatParser.Application.Queries.GetResourcesGenerals;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CombatAnalysis.CombatParserAPI.Controllers.CombatPlayerData;

[Route("api/v1/[controller]")]
[ApiController]
public class ResourceRecoveryGeneralController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpGet("getByUnitId/{unitId}")]
    public async Task<IActionResult> GetByUnitId(string unitId, int combatId, CancellationToken cancellationToken)
    {
        var resourcesGenerals = await _mediator.Send(new GetResourcesGeneralsQuery(unitId, combatId), cancellationToken);

        return Ok(resourcesGenerals);
    }
}
