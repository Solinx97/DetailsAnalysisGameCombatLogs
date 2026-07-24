using CombatParser.Application.Queries.HealDone.GetGenericChart;
using CombatParser.Application.Queries.HealDone.CountHeal;
using CombatParser.Application.Queries.HealDone.GetCombatPlayerChart;
using CombatParser.Application.Queries.HealDone.GetHeals;
using CombatParser.Application.Queries.HealDone.GetUniqueHealSpells;
using CombatParser.Application.Queries.HealDone.GetUniqueHealTargets;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CombatAnalysis.CombatParserAPI.Controllers.CombatPlayerData;

[Route("api/v1/[controller]")]
[ApiController]
public class HealDoneController(IMediator mediator) : ControllerBase
{
    private const string NONE_VALUE = "NONE";
    private const string ZERO_TIME_VALUE = "00:00:00";
    private readonly IMediator _mediator = mediator;

    [HttpGet("count")]
    public async Task<IActionResult> Count(int combatPlayerId, string target, string creator, string spell, string from, string to, CancellationToken cancellationToken)
    {
        var count = await _mediator.Send(new CountHealQuery(
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
        var heals = await _mediator.Send(new GetHealsQuery(
            combatPlayerId,
            target.Equals(NONE_VALUE) ? string.Empty : target,
            creator.Equals(NONE_VALUE) ? string.Empty : creator,
            spell.Equals(NONE_VALUE) ? string.Empty : spell,
            from.Equals(ZERO_TIME_VALUE) ? string.Empty : from,
            to.Equals(ZERO_TIME_VALUE) ? string.Empty : to,
            page,
            pageSize
            ), cancellationToken);

        return Ok(heals);
    }

    [HttpGet("getCombatPlayerChart/{combatPlayerId}")]
    public async Task<IActionResult> GetCombatPlayerChart(int combatPlayerId, CancellationToken cancellationToken)
    {
        var heals = await _mediator.Send(new GetCombatPlayerChartQuery(combatPlayerId), cancellationToken);

        return Ok(heals);
    }

    [HttpGet("getGenericChart/{combatId}")]
    public async Task<IActionResult> GetGenericChart(int combatId, CancellationToken cancellationToken)
    {
        var heals = await _mediator.Send(new GetGenericChartQuery(combatId), cancellationToken);

        return Ok(heals);
    }

    [HttpGet("getUniqueTargets/{combatPlayerId}")]
    public async Task<IActionResult> GetUniqueTargets(int combatPlayerId, CancellationToken cancellationToken)
    {
        var uniqueTargets = await _mediator.Send(new GetUniqueHealTargetsQuery(combatPlayerId), cancellationToken);

        return Ok(uniqueTargets);
    }

    [HttpGet("getUniqueSpells/{combatPlayerId}")]
    public async Task<IActionResult> GetUniqueSpells(int combatPlayerId, CancellationToken cancellationToken)
    {
        var uniqueSpells = await _mediator.Send(new GetUniqueHealSpellsQuery(combatPlayerId), cancellationToken);

        return Ok(uniqueSpells);
    }
}
