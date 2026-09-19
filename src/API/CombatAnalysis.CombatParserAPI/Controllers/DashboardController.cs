using CombatAnalysis.CombatParserAPI.Consts;
using CombatParser.Application.Queries.Dashboards.GetDamage;
using CombatParser.Application.Queries.Dashboards.GetDamageSpells;
using CombatParser.Application.Queries.Dashboards.GetDamageTaken;
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
    public async Task<IActionResult> GetDamage(int combatLogId, int combatId, string unitName, int valueType, CancellationToken cancellationToken)
    {
        var dashboard = await _mediator.Send(new GetDamageQuery(
            combatLogId, 
            combatId, 
            unitName.Equals(NoneValue.NONE_VALUE) ? string.Empty : unitName,
            valueType
            ), cancellationToken);

        return Ok(dashboard);
    }

    [HttpGet("getHeal/{combatLogId:int:min(1)}")]
    public async Task<IActionResult> GetHeal(int combatLogId, int combatId, string unitName, int valueType, CancellationToken cancellationToken)
    {
        var dashboard = await _mediator.Send(new GetHealQuery(
            combatLogId,
            combatId,
            unitName.Equals(NoneValue.NONE_VALUE) ? string.Empty : unitName,
            valueType
            ), cancellationToken);

        return Ok(dashboard);
    }

    [HttpGet("getDamageTaken/{combatLogId:int:min(1)}")]
    public async Task<IActionResult> GetDamageTaken(int combatLogId, int combatId, string unitName, int valueType, CancellationToken cancellationToken)
    {
        var dashboard = await _mediator.Send(new GetDamageTakenQuery(
            combatLogId,
            combatId,
            unitName.Equals(NoneValue.NONE_VALUE) ? string.Empty : unitName,
            valueType
            ), cancellationToken);

        return Ok(dashboard);
    }

    [HttpGet("getDamageSpells/{combatLogId:int:min(1)}")]
    public async Task<IActionResult> GetDamageSpells(int combatLogId, int combatId, string unitName, CancellationToken cancellationToken)
    {
        var dashboard = await _mediator.Send(new GetDamageSpellsQuery(
            combatLogId,
            combatId,
            unitName.Equals(NoneValue.NONE_VALUE) ? string.Empty : unitName
            ), cancellationToken);

        return Ok(dashboard);
    }

    [HttpGet("getHealSpells/{combatLogId:int:min(1)}")]
    public async Task<IActionResult> GetHealSpells(int combatLogId, int combatId, string unitName, CancellationToken cancellationToken)
    {
        var spells = await _mediator.Send(new GetHealSpellsQuery(
            combatLogId,
            combatId,
            unitName.Equals(NoneValue.NONE_VALUE) ? string.Empty : unitName
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
