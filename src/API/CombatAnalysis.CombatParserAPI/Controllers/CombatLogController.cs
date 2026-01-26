using CombatAnalysis.CombatParserAPI.PartialModels;
using CombatParser.Application.Commands.CombatLogIsReady;
using CombatParser.Application.Commands.CreateCombatLog;
using CombatParser.Application.Commands.DeleteCombatLog;
using CombatParser.Application.Commands.UpdateCombatLog;
using CombatParser.Application.Queries.GetAllCombatLogs;
using CombatParser.Application.Queries.GetByIdCombatLog;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CombatAnalysis.CombatParserAPI.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class CombatLogController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var allCombatLogs = await _mediator.Send(new GetAllCombatLogsQuery(), cancellationToken);

        return Ok(allCombatLogs);
    }

    [HttpGet("{id:int:min(1)}")]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
    {
        var combatLog = await _mediator.Send(new GetByIdCombatLogQuery(id), cancellationToken);

        return Ok(combatLog);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateCombatLogCommand command, CancellationToken cancellationToken)
    {
        var combatLog = await _mediator.Send(command, cancellationToken);

        return Ok(combatLog);
    }

    [HttpPatch("{id:int:min(1)}")]
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

    [HttpPatch("combatLogIsReady/{id:int:min(1)}")]
    public async Task<IActionResult> CombatLogIsReady(int id, [FromBody] CombatLogIsReadyPatch combatLog, CancellationToken cancellationToken)
    {
        if (id != combatLog.Id)
        {
            return BadRequest("Route ID and body ID do not match.");
        }

        var command = new CombatLogIsReadyCommand(combatLog.Id, combatLog.NumberReadyCombats, combatLog.CombatsInQueue);
        await _mediator.Send(command, cancellationToken);

        return NoContent();
    }

    [HttpDelete("{id:int:min(1)}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        await _mediator.Send(new DeleteCombatLogCommand(id), cancellationToken);

        return NoContent();
    }
}
