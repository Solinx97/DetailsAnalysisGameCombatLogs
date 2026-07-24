using CombatParser.Application.Queries.GetPlayerDeaths;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CombatAnalysis.CombatParserAPI.Controllers.CombatPlayerData;

[Route("api/v1/[controller]")]
[ApiController]
public class CombatPlayerDeathController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpGet("getByCombatPlayerId/{combatPlayerId:int:min(1)}")]
    public async Task<IActionResult> GetByCombatPlayerId(int combatPlayerId, CancellationToken cancellationToken)
    {
        var deaths = await _mediator.Send(new GetPlayerDeathsQuery(combatPlayerId), cancellationToken);

        return Ok(deaths);
    }
}
