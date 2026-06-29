using CombatParser.Application.Queries.DamageDone.CountDamageBySpell;
using CombatParser.Application.Queries.DamageDone.CountDamageByTarget;
using CombatParser.Application.Queries.DamageDone.GetDamageByEachTarget;
using CombatParser.Application.Queries.DamageDone.GetDamageCount;
using CombatParser.Application.Queries.DamageDone.GetDamages;
using CombatParser.Application.Queries.DamageDone.GetDamagesByAll;
using CombatParser.Application.Queries.DamageDone.GetDamagesBySpell;
using CombatParser.Application.Queries.DamageDone.GetDamagesByTarget;
using CombatParser.Application.Queries.DamageDone.GetDamageValueToTarget;
using CombatParser.Application.Queries.DamageDone.GetUniqueDamageSpells;
using CombatParser.Application.Queries.DamageDone.GetUniqueDamageTargets;
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
        var damages = await _mediator.Send(new GetDamagesQuery(combatPlayerId, page, pageSize), cancellationToken);

        return Ok(damages);
    }

    [HttpGet("count/{combatPlayerId}")]
    public async Task<IActionResult> Count(int combatPlayerId, CancellationToken cancellationToken)
    {
        var count = await _mediator.Send(new GetDamageCountQuery(combatPlayerId), cancellationToken);

        return Ok(count);
    }

    [HttpGet("getUniqueTargets/{combatPlayerId}")]
    public async Task<IActionResult> GetUniqueTargets(int combatPlayerId, CancellationToken cancellationToken)
    {
        var uniqueTargets = await _mediator.Send(new GetUniqueDamageTargetsQuery(combatPlayerId), cancellationToken);

        return Ok(uniqueTargets);
    }

    [HttpGet("getDamageByEachTarget/{combatId}")]
    public async Task<IActionResult> GetDamageByEachTarget(int combatId, CancellationToken cancellationToken)
    {
        var damages = await _mediator.Send(new GetDamageByEachTargetQuery(combatId), cancellationToken);

        return Ok(damages);
    }

    [HttpGet("getByTarget")]
    public async Task<IActionResult> GetByTarget(int combatPlayerId, string target, int page, int pageSize, CancellationToken cancellationToken)
    {
        var damages = await _mediator.Send(new GetDamagesByTargetQuery(combatPlayerId, target, page, pageSize), cancellationToken);

        return Ok(damages);
    }

    [HttpGet("getValueToTarget")]
    public async Task<IActionResult> GetValueToTarget(int combatPlayerId, string target, CancellationToken cancellationToken)
    {
        var value = await _mediator.Send(new GetDamageValueToTargetQuery(combatPlayerId, target), cancellationToken);

        return Ok(value);
    }

    [HttpGet("countByTarget")]
    public async Task<IActionResult> CountByTarget(int combatPlayerId, string target, CancellationToken cancellationToken)
    {
        var count = await _mediator.Send(new CountDamageByTargetQuery(combatPlayerId, target), cancellationToken);

        return Ok(count);
    }

    [HttpGet("getUniqueSpells/{combatPlayerId}")]
    public async Task<IActionResult> GetUniqueSpells(int combatPlayerId, CancellationToken cancellationToken)
    {
        var uniqueSpells = await _mediator.Send(new GetUniqueDamageSpellsQuery(combatPlayerId), cancellationToken);

        return Ok(uniqueSpells);
    }

    [HttpGet("getBySpell")]
    public async Task<IActionResult> GetBySpell(int combatPlayerId, string spell, int page, int pageSize, CancellationToken cancellationToken)
    {
        var damages = await _mediator.Send(new GetDamagesBySpellQuery(combatPlayerId, spell, page, pageSize), cancellationToken);

        return Ok(damages);
    }

    [HttpGet("getByAll")]
    public async Task<IActionResult> GetByAll(int combatPlayerId, string target, string spell, int page, int pageSize, CancellationToken cancellationToken)
    {
        var damages = await _mediator.Send(new GetDamagesByAllQuery(combatPlayerId, target, spell, page, pageSize), cancellationToken);

        return Ok(damages);
    }

    [HttpGet("countBySpell")]
    public async Task<IActionResult> CountBySpell(int combatPlayerId, string spell, CancellationToken cancellationToken)
    {
        var count = await _mediator.Send(new CountDamageBySpellQuery(combatPlayerId, spell), cancellationToken);

        return Ok(count);
    }
}
