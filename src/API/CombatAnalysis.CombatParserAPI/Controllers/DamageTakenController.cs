using CombatParser.Application.Queries.DamageTaken.CountDamageTaken;
using CombatParser.Application.Queries.DamageTaken.GetCombatPlayerChart;
using CombatParser.Application.Queries.DamageTaken.GetDamageTakens;
using CombatParser.Application.Queries.DamageTaken.GetUniqueDamageTakenCreators;
using CombatParser.Application.Queries.DamageTaken.GetUniqueDamageTakenSpells;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CombatAnalysis.CombatParserAPI.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class DamageTakenController(IMediator mediator) : ControllerBase
{
    private const string NONE_VALUE = "NONE";
    private const string ZERO_TIME_VALUE = "00:00:00";
    private readonly IMediator _mediator = mediator;

    [HttpGet("count")]
    public async Task<IActionResult> Count(int combatPlayerId, string target, string creator, string spell, string from, string to, CancellationToken cancellationToken)
    {
        var count = await _mediator.Send(new CountDamageTakenQuery(
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
        var damageTakens = await _mediator.Send(new GetDamageTakensQuery(
            combatPlayerId,
            target.Equals(NONE_VALUE) ? string.Empty : target,
            creator.Equals(NONE_VALUE) ? string.Empty : creator,
            spell.Equals(NONE_VALUE) ? string.Empty : spell,
            from.Equals(ZERO_TIME_VALUE) ? string.Empty : from,
            to.Equals(ZERO_TIME_VALUE) ? string.Empty : to,
            page,
            pageSize
            ), cancellationToken);

        return Ok(damageTakens);
    }

    [HttpGet("getCombatPlayerChart/{combatPlayerId}")]
    public async Task<IActionResult> GetCombatPlayerChart(int combatPlayerId, CancellationToken cancellationToken)
    {
        var damageTakens = await _mediator.Send(new GetCombatPlayerChartQuery(combatPlayerId), cancellationToken);

        return Ok(damageTakens);
    }

    [HttpGet("getUniqueCreators/{combatPlayerId}")]
    public async Task<IActionResult> GetUniqueCreators(int combatPlayerId, CancellationToken cancellationToken)
    {
        var uniqueCreators = await _mediator.Send(new GetUniqueDamageTakenCreatorsQuery(combatPlayerId), cancellationToken);

        return Ok(uniqueCreators);
    }

    [HttpGet("getUniqueSpells/{combatPlayerId}")]
    public async Task<IActionResult> GetUniqueSpells(int combatPlayerId, CancellationToken cancellationToken)
    {
        var uniqueSpells = await _mediator.Send(new GetUniqueDamageTakenSpellsQuery(combatPlayerId), cancellationToken);

        return Ok(uniqueSpells);
    }
}
