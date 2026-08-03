using CombatAnalysis.CombatParserAPI.PartialModels;
using CombatParser.Application.Commands.CreateCombatLog;
using CombatParser.Application.Commands.DeleteCombatLog;
using CombatParser.Application.Commands.UpdateCombatLog;
using CombatParser.Application.Queries.GetByIdCombatLog;
using CombatParser.Application.Queries.GetCombatLogsByLogType;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CombatAnalysis.CombatParserAPI.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class CombatLogController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpGet("getByLogType")]
    public async Task<IActionResult> GetByLogType(int logType, string? appUserId, CancellationToken cancellationToken)
    {
        var combatLogs = await _mediator.Send(new GetCombatLogsByLogTypeQuery(logType, appUserId), cancellationToken);

        return Ok(combatLogs);
    }

    [HttpGet("{id:int:min(1)}")]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
    {
        var combatLog = await _mediator.Send(new GetByIdCombatLogQuery(id), cancellationToken);

        return Ok(combatLog);
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Create([FromBody] CreateCombatLogCommand command, CancellationToken cancellationToken)
    {
        var combatLog = await _mediator.Send(command, cancellationToken);

        return Ok(combatLog);
    }

    [HttpPatch("{id:int:min(1)}")]
    [Authorize]
    public async Task<IActionResult> PartialUpdate(int id, [FromBody] CombatLogPatch combatLog, CancellationToken cancellationToken)
    {
        if (id != combatLog.Id)
        {
            return BadRequest("Route ID and body ID do not match.");
        }

        var command = new UpdateCombatLogCommand(combatLog.Id, combatLog.Name);
        await _mediator.Send(command, cancellationToken);

        return NoContent();
    }

    [HttpDelete("{id:int:min(1)}")]
    [Authorize]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        await _mediator.Send(new DeleteCombatLogCommand(id), cancellationToken);

        return NoContent();
    }
}
