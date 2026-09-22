using CombatAnalysis.CombatParserAPI.Consts;
using CombatParser.Application.Queries.Dashboards.GetDamage;
using CombatParser.Application.Queries.Dashboards.GetDamageSpells;
using CombatParser.Application.Queries.Dashboards.GetHeal;
using CombatParser.Application.Queries.Dashboards.GetHealSpells;
using CombatParser.Application.Queries.Dashboards.GetPotions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CombatAnalysis.CombatParserAPI.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class DashboardController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpGet("getDamage/{combatLogId:int:min(1)}")]
    public async Task<IActionResult> GetDamage(int combatLogId, string bossName, int combatId, string creatorName, string targetName, int valueType, CancellationToken cancellationToken)
    {
        var dashboard = await _mediator.Send(new GetDamageQuery(
            combatLogId,
            bossName.Equals(NoneValue.NONE_VALUE) ? string.Empty : bossName,
            combatId,
            creatorName.Equals(NoneValue.NONE_VALUE) ? string.Empty : creatorName,
            targetName.Equals(NoneValue.NONE_VALUE) ? string.Empty : targetName,
            valueType
            ), cancellationToken);

        return Ok(dashboard);
    }

    [HttpGet("getHeal/{combatLogId:int:min(1)}")]
    public async Task<IActionResult> GetHeal(int combatLogId, string bossName, int combatId, string creatorName, string targetName, int valueType, CancellationToken cancellationToken)
    {
        var dashboard = await _mediator.Send(new GetHealQuery(
            combatLogId,
            bossName.Equals(NoneValue.NONE_VALUE) ? string.Empty : bossName,
            combatId,
            creatorName.Equals(NoneValue.NONE_VALUE) ? string.Empty : creatorName,
            targetName.Equals(NoneValue.NONE_VALUE) ? string.Empty : targetName,
            valueType
            ), cancellationToken);

        return Ok(dashboard);
    }

    [HttpGet("getDamageSpells/{combatLogId:int:min(1)}")]
    public async Task<IActionResult> GetDamageSpells(int combatLogId, string bossName, int combatId, CancellationToken cancellationToken)
    {
        var dashboard = await _mediator.Send(new GetDamageSpellsQuery(
            combatLogId,
            bossName.Equals(NoneValue.NONE_VALUE) ? string.Empty : bossName,
            combatId
            ), cancellationToken);

        return Ok(dashboard);
    }

    [HttpGet("getHealSpells/{combatLogId:int:min(1)}")]
    public async Task<IActionResult> GetHealSpells(int combatLogId, string bossName, int combatId, CancellationToken cancellationToken)
    {
        var spells = await _mediator.Send(new GetHealSpellsQuery(
            combatLogId,
            bossName.Equals(NoneValue.NONE_VALUE) ? string.Empty : bossName,
            combatId
            ), cancellationToken);

        return Ok(spells);
    }

    [HttpGet("getPotions/{combatLogId:int:min(1)}")]
    public async Task<IActionResult> GetPotions(int combatLogId, CancellationToken cancellationToken)
    {
        var potions = await _mediator.Send(new GetPotionsQuery(combatLogId), cancellationToken);

        return Ok(potions);
    }
}
