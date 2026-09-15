using CombatParser.Application.Queries.DamageDone.DamageTaken.CountDamageTaken;
using CombatParser.Application.Queries.DamageDone.DamageTaken.GetCombatPlayerChart;
using CombatParser.Application.Queries.DamageDone.DamageTaken.GetDamageTakens;
using CombatParser.Application.Queries.DamageDone.DamageTaken.GetUniqueDamageTakenCreators;
using CombatParser.Application.Queries.DamageDone.DamageTaken.GetUniqueDamageTakenSpells;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CombatAnalysis.CombatParserAPI.Controllers.CombatPlayerData;

[Route("api/v1/[controller]")]
[ApiController]
public class DamageTakenController(IMediator mediator) : ControllerBase
{
    private const string NONE_VALUE = "NONE";
    private const string ZERO_TIME_VALUE = "00:00:00";
    private readonly IMediator _mediator = mediator;

    [HttpGet("count")]
    public async Task<IActionResult> Count(string unitId, string target, string creator, string spell, string from, string to, CancellationToken cancellationToken)
    {
        var count = await _mediator.Send(new CountDamageTakenQuery(
            unitId,
            target.Equals(NONE_VALUE) ? string.Empty : target,
            creator.Equals(NONE_VALUE) ? string.Empty : creator,
            spell.Equals(NONE_VALUE) ? string.Empty : spell,
            from.Equals(ZERO_TIME_VALUE) ? string.Empty : from,
            to.Equals(ZERO_TIME_VALUE) ? string.Empty : to
            ), cancellationToken);

        return Ok(count);
    }

    [HttpGet("getAll")]
    public async Task<IActionResult> GetAll(string unitId, int combatId, string target, string creator, string spell, string from, string to, int page, int pageSize, CancellationToken cancellationToken)
    {
        var damageTakens = await _mediator.Send(new GetDamageTakensQuery(
            unitId,
            combatId,
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

    [HttpGet("getUnitChart/{unitId}")]
    public async Task<IActionResult> GetUnitChart(string unitId, CancellationToken cancellationToken)
    {
        var damageTakens = await _mediator.Send(new GetCombatPlayerChartQuery(unitId), cancellationToken);

        return Ok(damageTakens);
    }

    [HttpGet("getUniqueCreators/{unitId}")]
    public async Task<IActionResult> GetUniqueCreators(string unitId, CancellationToken cancellationToken)
    {
        var uniqueCreators = await _mediator.Send(new GetUniqueDamageTakenCreatorsQuery(unitId), cancellationToken);

        return Ok(uniqueCreators);
    }

    [HttpGet("getUniqueSpells/{unitId}")]
    public async Task<IActionResult> GetUniqueSpells(string unitId, CancellationToken cancellationToken)
    {
        var uniqueSpells = await _mediator.Send(new GetUniqueDamageTakenSpellsQuery(unitId), cancellationToken);

        return Ok(uniqueSpells);
    }
}
