using CombatParser.Application.Queries.DamageTaken.CountDamageTakenByCreator;
using CombatParser.Application.Queries.DamageTaken.CountDamageTakenBySpell;
using CombatParser.Application.Queries.DamageTaken.GetDamageTakenCount;
using CombatParser.Application.Queries.DamageTaken.GetDamageTakens;
using CombatParser.Application.Queries.DamageTaken.GetDamageTakensByCreator;
using CombatParser.Application.Queries.DamageTaken.GetDamageTakensBySpell;
using CombatParser.Application.Queries.DamageTaken.GetUniqueDamageTakenCreators;
using CombatParser.Application.Queries.DamageTaken.GetUniqueDamageTakenSpells;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CombatAnalysis.CombatParserAPI.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class DamageTakenController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpGet("getByCombatPlayerId")]
    public async Task<IActionResult> GetByCombatPlayerId(int combatPlayerId, int page, int pageSize, CancellationToken cancellationToken)
    {
        var damageTakens = await _mediator.Send(new GetDamageTakensQuery(combatPlayerId, page, pageSize), cancellationToken);

        return Ok(damageTakens);
    }

    [HttpGet("count/{combatPlayerId}")]
    public async Task<IActionResult> Count(int combatPlayerId, CancellationToken cancellationToken)
    {
        var count = await _mediator.Send(new GetDamageTakenCountQuery(combatPlayerId), cancellationToken);

        return Ok(count);
    }

    [HttpGet("getUniqueCreators/{combatPlayerId}")]
    public async Task<IActionResult> GetUniqueCreators(int combatPlayerId, CancellationToken cancellationToken)
    {
        var uniqueCreators = await _mediator.Send(new GetUniqueDamageTakenCreatorsQuery(combatPlayerId), cancellationToken);

        return Ok(uniqueCreators);
    }

    [HttpGet("getByCreator")]
    public async Task<IActionResult> GetByCreator(int combatPlayerId, string creator, int page, int pageSize, CancellationToken cancellationToken)
    {
        var damageTakens = await _mediator.Send(new GetDamageTakensByCreatorQuery(combatPlayerId, creator, page, pageSize), cancellationToken); ;

        return Ok(damageTakens);
    }

    [HttpGet("countByCreator")]
    public async Task<IActionResult> CountByCreator(int combatPlayerId, string creator, CancellationToken cancellationToken)
    {
        var count = await _mediator.Send(new CountDamageTakenByCreatorQuery(combatPlayerId, creator), cancellationToken);

        return Ok(count);
    }

    [HttpGet("getUniqueSpells/{combatPlayerId}")]
    public async Task<IActionResult> GetUniqueSpells(int combatPlayerId, CancellationToken cancellationToken)
    {
        var uniqueSpells = await _mediator.Send(new GetUniqueDamageTakenSpellsQuery(combatPlayerId), cancellationToken);

        return Ok(uniqueSpells);
    }

    [HttpGet("getBySpell")]
    public async Task<IActionResult> GetBySpell(int combatPlayerId, string spell, int page, int pageSize, CancellationToken cancellationToken)
    {
        var damageTakens = await _mediator.Send(new GetDamageTakensBySpellQuery(combatPlayerId, spell, page, pageSize), cancellationToken);

        return Ok(damageTakens);
    }

    [HttpGet("countBySpell")]
    public async Task<IActionResult> CountBySpell(int combatPlayerId, string spell, CancellationToken cancellationToken)
    {
        var count = await _mediator.Send(new CountDamageTakenBySpellQuery(combatPlayerId, spell), cancellationToken);

        return Ok(count);
    }
}
