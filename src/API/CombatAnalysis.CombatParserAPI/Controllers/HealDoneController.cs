using CombatParser.Application.Queries.HealDone.CountHealBySpell;
using CombatParser.Application.Queries.HealDone.CountHealByTarget;
using CombatParser.Application.Queries.HealDone.GetHealCountByCombatPlayerId;
using CombatParser.Application.Queries.HealDone.GetHeals;
using CombatParser.Application.Queries.HealDone.GetHealsBySpell;
using CombatParser.Application.Queries.HealDone.GetHealsByTarget;
using CombatParser.Application.Queries.HealDone.GetUniqueHealSpells;
using CombatParser.Application.Queries.HealDone.GetUniqueHealTargets;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CombatAnalysis.CombatParserAPI.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class HealDoneController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpGet("getByCombatPlayerId")]
    public async Task<IActionResult> GetByCombatPlayerId(int combatPlayerId, int page, int pageSize, CancellationToken cancellationToken)
    {
        var heals = await _mediator.Send(new GetHealsQuery(combatPlayerId, page, pageSize), cancellationToken);

        return Ok(heals);
    }

    [HttpGet("count/{combatPlayerId}")]
    public async Task<IActionResult> Count(int combatPlayerId, CancellationToken cancellationToken)
    {
        var count = await _mediator.Send(new GetHealCountQuery(combatPlayerId), cancellationToken);

        return Ok(count);
    }

    [HttpGet("getUniqueTargets/{combatPlayerId}")]
    public async Task<IActionResult> GetUniqueTargets(int combatPlayerId, CancellationToken cancellationToken)
    {
        var uniqueTargets = await _mediator.Send(new GetUniqueHealTargetsQuery(combatPlayerId), cancellationToken);

        return Ok(uniqueTargets);
    }

    [HttpGet("getByTarget")]
    public async Task<IActionResult> GetByTarget(int combatPlayerId, string target, int page, int pageSize, CancellationToken cancellationToken)
    {
        var heals = await _mediator.Send(new GetHealsByTargetQuery(combatPlayerId, target, page, pageSize), cancellationToken); ;

        return Ok(heals);
    }

    [HttpGet("countByTarget")]
    public async Task<IActionResult> CountByTarget(int combatPlayerId, string target, CancellationToken cancellationToken)
    {
        var count = await _mediator.Send(new CountHealByTargetQuery(combatPlayerId, target), cancellationToken);

        return Ok(count);
    }

    [HttpGet("getUniqueSpells/{combatPlayerId}")]
    public async Task<IActionResult> GetUniqueSpells(int combatPlayerId, CancellationToken cancellationToken)
    {
        var uniqueSpells = await _mediator.Send(new GetUniqueHealSpellsQuery(combatPlayerId), cancellationToken);

        return Ok(uniqueSpells);
    }

    [HttpGet("getBySpell")]
    public async Task<IActionResult> GetBySpell(int combatPlayerId, string spell, int page, int pageSize, CancellationToken cancellationToken)
    {
        var heals = await _mediator.Send(new GetHealsBySpellQuery(combatPlayerId, spell, page, pageSize), cancellationToken);

        return Ok(heals);
    }

    [HttpGet("countBySpell")]
    public async Task<IActionResult> CountBySpell(int combatPlayerId, string spell, CancellationToken cancellationToken)
    {
        var count = await _mediator.Send(new CountHealBySpellQuery(combatPlayerId, spell), cancellationToken);

        return Ok(count);
    }
}
