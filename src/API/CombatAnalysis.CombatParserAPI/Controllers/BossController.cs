using CombatParser.Application.Queries.GetBoss;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CombatAnalysis.CombatParserAPI.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class BossController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpGet]
    public async Task<IActionResult> Get(int gameBossId, int difficult, int groupSize, CancellationToken cancellationToken)
    {
        var boss = await _mediator.Send(new GetBossQuery(gameBossId, difficult, groupSize), cancellationToken);

        return Ok(boss);
    }
}
