using CombatParser.Application.Queries.DamageDone.CountDamage;
using CombatParser.Application.Queries.DamageDone.GetCombatPlayerChart;
using CombatParser.Application.Queries.DamageDone.GetDamages;
using CombatParser.Application.Queries.DamageDone.GetGenericChart;
using CombatParser.Application.Queries.DamageDone.GetUniqueDamageSpells;
using CombatParser.Application.Queries.DamageDone.GetUniqueDamageTargets;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CombatAnalysis.CombatParserAPI.Controllers.CombatPlayerData;

[Route("api/v1/[controller]")]
[ApiController]
public class DamageDoneController(IMediator mediator) : ControllerBase
{
    private const string NONE_VALUE = "NONE";
    private const string ZERO_TIME_VALUE = "00:00:00";
    private readonly IMediator _mediator = mediator;

    [HttpGet("count")]
    public async Task<IActionResult> Count(int combatPlayerId, string target, string creator, string spell, string from, string to, CancellationToken cancellationToken)
    {
        var count = await _mediator.Send(new CountDamageQuery(
            combatPlayerId,
            target.Equals(NONE_VALUE) ? string.Empty : target,
            creator.Equals(NONE_VALUE) ? string.Empty : creator, 
            spell.Equals(NONE_VALUE) ? string.Empty : spell, 
            from.Equals(ZERO_TIME_VALUE) ? string.Empty : from, 
            to.Equals(ZERO_TIME_VALUE) ? string.Empty : to
            ), cancellationToken);

        return Ok(count);
    }

    [HttpGet("getAll")]
    public async Task<IActionResult> GetAll(int combatPlayerId, string target, string creator, string spell, string from, string to, int page, int pageSize, CancellationToken cancellationToken)
    {
        var damages = await _mediator.Send(new GetDamagesQuery(
            combatPlayerId,
            target.Equals(NONE_VALUE) ? string.Empty : target,
            creator.Equals(NONE_VALUE) ? string.Empty : creator,
            spell.Equals(NONE_VALUE) ? string.Empty : spell,
            from.Equals(ZERO_TIME_VALUE) ? string.Empty : from,
            to.Equals(ZERO_TIME_VALUE) ? string.Empty : to,
            page,
            pageSize
            ), cancellationToken);

        return Ok(damages);
    }

    [HttpGet("getCombatPlayerChart/{combatPlayerId}")]
    public async Task<IActionResult> GetCombatPlayerChart(int combatPlayerId, CancellationToken cancellationToken)
    {
        var damages = await _mediator.Send(new GetCombatPlayerChartQuery(combatPlayerId), cancellationToken);

        return Ok(damages);
    }

    [HttpGet("getGenericChart/{combatId}")]
    public async Task<IActionResult> GetGenericChart(int combatId, CancellationToken cancellationToken)
    {
        var damages = await _mediator.Send(new GetGenericChartQuery(combatId), cancellationToken);

        return Ok(damages);
    }

    [HttpGet("getUniqueTargets/{combatPlayerId}")]
    public async Task<IActionResult> GetUniqueTargets(int combatPlayerId, CancellationToken cancellationToken)
    {
        var uniqueTargets = await _mediator.Send(new GetUniqueDamageTargetsQuery(combatPlayerId), cancellationToken);

        return Ok(uniqueTargets);
    }

    [HttpGet("getUniqueSpells/{combatPlayerId}")]
    public async Task<IActionResult> GetUniqueSpells(int combatPlayerId, CancellationToken cancellationToken)
    {
        var uniqueSpells = await _mediator.Send(new GetUniqueDamageSpellsQuery(combatPlayerId), cancellationToken);

        return Ok(uniqueSpells);
    }
}
