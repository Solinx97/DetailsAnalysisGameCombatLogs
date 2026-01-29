using CombatParser.Application.Queries.DamageDone.CountDamageDoneBySpell;
using CombatParser.Application.Queries.DamageDone.CountDamageDoneByTarget;
using CombatParser.Application.Queries.DamageDone.GetDamageByEachTarget;
using CombatParser.Application.Queries.DamageDone.GetDamageDoneCountByCombatPlayerId;
using CombatParser.Application.Queries.DamageDone.GetDamageDonesByCombatPlayerId;
using CombatParser.Application.Queries.DamageDone.GetDamageDonesBySpell;
using CombatParser.Application.Queries.DamageDone.GetDamageDonesByTarget;
using CombatParser.Application.Queries.DamageDone.GetDamageValueByTarget;
using CombatParser.Application.Queries.DamageDone.GetUniqueSpellsDamageDone;
using CombatParser.Application.Queries.DamageDone.GetUniqueTargetsDamageDone;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CombatAnalysis.CombatParserAPI.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class DamageDoneController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpGet("getByCombatPlayerId")]
    public async Task<IActionResult> GetByCombatPlayerId(int combatPlayerId, int page, int pageSize, CancellationToken cancellationToken)
    {
        var damageDones = await _mediator.Send(new GetDamageDonesByCombatPlayerIdQuery(combatPlayerId, page, pageSize), cancellationToken);

        return Ok(damageDones);
    }

    [HttpGet("count/{combatPlayerId}")]
    public async Task<IActionResult> Count(int combatPlayerId, CancellationToken cancellationToken)
    {
        var count = await _mediator.Send(new GetDamageDoneCountByCombatPlayerIdQuery(combatPlayerId), cancellationToken);

        return Ok(count);
    }

    [HttpGet("getUniqueTargets/{combatPlayerId}")]
    public async Task<IActionResult> GetUniqueTargets(int combatPlayerId, CancellationToken cancellationToken)
    {
        var uniqueTargets = await _mediator.Send(new GetUniqueTargetsDamageDoneQuery(combatPlayerId), cancellationToken);

        return Ok(uniqueTargets);
    }

    [HttpGet("getDamageByEachTarget/{combatId}")]
    public async Task<IActionResult> GetDamageByEachTarget(int combatId, CancellationToken cancellationToken)
    {
        var damageByEachTarget = await _mediator.Send(new GetDamageByEachTargetQuery(combatId), cancellationToken);

        return Ok(damageByEachTarget);
    }

    [HttpGet("getByTarget")]
    public async Task<IActionResult> GetByTarget(int combatPlayerId, string target, int page, int pageSize, CancellationToken cancellationToken)
    {
        var damageDones = await _mediator.Send(new GetDamageDonesByTargetQuery(combatPlayerId, target, page, pageSize), cancellationToken);

        return Ok(damageDones);
    }

    [HttpGet("getValueByTarget")]
    public async Task<IActionResult> GetValueByTarget(int combatPlayerId, string target, CancellationToken cancellationToken)
    {
        var value = await _mediator.Send(new GetDamageValueByTargetQuery(combatPlayerId, target), cancellationToken);

        return Ok(value);
    }

    [HttpGet("countByTarget")]
    public async Task<IActionResult> CountByTarget(int combatPlayerId, string target, CancellationToken cancellationToken)
    {
        var count = await _mediator.Send(new CountDamageDoneByTargetQuery(combatPlayerId, target), cancellationToken);

        return Ok(count);
    }

    [HttpGet("getUniqueSpells/{combatPlayerId}")]
    public async Task<IActionResult> GetUniqueSpells(int combatPlayerId, CancellationToken cancellationToken)
    {
        var uniqueSpells = await _mediator.Send(new GetUniqueSpellsDamageDoneQuery(combatPlayerId), cancellationToken);

        return Ok(uniqueSpells);
    }

    [HttpGet("getBySpell")]
    public async Task<IActionResult> GetBySpell(int combatPlayerId, string spell, int page, int pageSize, CancellationToken cancellationToken)
    {
        var damageDones = await _mediator.Send(new GetDamageDonesBySpellQuery(combatPlayerId, spell, page, pageSize), cancellationToken);

        return Ok(damageDones);
    }

    [HttpGet("countBySpell")]
    public async Task<IActionResult> CountBySpell(int combatPlayerId, string spell, CancellationToken cancellationToken)
    {
        var count = await _mediator.Send(new CountDamageDoneBySpellQuery(combatPlayerId, spell), cancellationToken);

        return Ok(count);
    }
}
