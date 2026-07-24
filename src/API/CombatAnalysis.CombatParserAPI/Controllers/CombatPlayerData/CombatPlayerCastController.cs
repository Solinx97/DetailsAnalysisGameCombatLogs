using CombatParser.Application.Queries.GetCombatPlayerCasts;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CombatAnalysis.CombatParserAPI.Controllers.CombatPlayerData;

[Route("api/v1/[controller]")]
[ApiController]
public class CombatPlayerCastController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpGet("getByCombatPlayerId/{combatPlayerId:int:min(1)}")]
    public async Task<IActionResult> GetByCombatPlayerId(int combatPlayerId, CancellationToken cancellationToken)
    {
        var casts = await _mediator.Send(new GetCombatPlayerCastsQuery(combatPlayerId), cancellationToken);

        return Ok(casts);
    }
}
