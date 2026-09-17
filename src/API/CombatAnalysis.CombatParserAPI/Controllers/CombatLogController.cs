using CombatAnalysis.CombatParserAPI.Interfaces;
using CombatAnalysis.CombatParserAPI.PartialModels;
using CombatParser.Application.Commands.AddCombatLogStatus;
using CombatParser.Application.Commands.CreateCombatLog;
using CombatParser.Application.Commands.DeleteCombatLog;
using CombatParser.Application.Commands.UpdateCombatLog;
using CombatParser.Application.Queries.GetByIdCombatLog;
using CombatParser.Application.Queries.GetCombatLogsByLogType;
using CombatParser.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CombatAnalysis.CombatParserAPI.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class CombatLogController(IMediator mediator, IDeleteCombatLogQueue deleteCombatLogQueue) : ControllerBase
{
    private readonly IMediator _mediator = mediator;
    private readonly IDeleteCombatLogQueue _deleteCombatLogQueue = deleteCombatLogQueue;

    [HttpGet("getByLogType")]
    public async Task<IActionResult> GetByLogType(int logType, int gameVersion, string? appUserId, CancellationToken cancellationToken)
    {
        var combatLogs = await _mediator.Send(new GetCombatLogsByLogTypeQuery(logType, gameVersion, appUserId), cancellationToken);

        return Ok(combatLogs);
    }

    [HttpGet("{id:int:min(1)}")]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
    {
        var combatLog = await _mediator.Send(new GetByIdCombatLogQuery(id), cancellationToken);

        return Ok(combatLog);
    }

    [HttpPost("addStatus/{id:int:min(1)}")]
    public async Task<IActionResult> AddStatus(int id, int status, CancellationToken cancellationToken)
    {
        var command = new AddCombatLogStatusCommand(id, status);
        await _mediator.Send(command, cancellationToken);

        return NoContent();
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Create([FromBody] CreateCombatLogCommand command, CancellationToken cancellationToken)
    {
        var combatLogId = await _mediator.Send(command, cancellationToken);

        return Ok(combatLogId);
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


        await _deleteCombatLogQueue.EnqueueAsync(new DeleteCombatLogCommand(id), cancellationToken);

        return Accepted();
    }
}
