using CombatParser.Application.Queries.GetPlayerDeaths;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CombatAnalysis.CombatParserAPI.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class PlayerDeathController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpGet("getByCombatPlayerId/{combatPlayerId:int:min(1)}")]
    public async Task<IActionResult> Find(int combatPlayerId, CancellationToken cancellationToken)
    {
        var deaths = await _mediator.Send(new GetPlayerDeathsQuery(combatPlayerId), cancellationToken);

        return Ok(deaths);
    }
}
