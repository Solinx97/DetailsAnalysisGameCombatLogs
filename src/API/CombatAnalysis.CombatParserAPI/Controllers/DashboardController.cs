using CombatAnalysis.CombatParserAPI.Consts;
using CombatParser.Application.Queries.Dashboards.GetDamageSpells;
using CombatParser.Application.Queries.Dashboards.GetDPS;
using CombatParser.Application.Queries.Dashboards.GetHealSpells;
using CombatParser.Application.Queries.Dashboards.GetHPS;
using CombatParser.Application.Queries.Dashboards.GetPotions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CombatAnalysis.CombatParserAPI.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class DashboardController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpGet("getDPS/{combatLogId:int:min(1)}")]
    public async Task<IActionResult> GetDPS(int combatLogId, int combatId, string unitName, CancellationToken cancellationToken)
    {
        var dashboard = await _mediator.Send(new GetDPSQuery(
            combatLogId, 
            combatId, 
            unitName.Equals(NoneValue.NONE_VALUE) ? string.Empty : unitName
            ), cancellationToken);

        return Ok(dashboard);
    }

    [HttpGet("getHPS/{combatLogId:int:min(1)}")]
    public async Task<IActionResult> GetHPS(int combatLogId, int combatId, string unitName, CancellationToken cancellationToken)
    {
        var dashboard = await _mediator.Send(new GetHPSQuery(
            combatLogId,
            combatId,
            unitName.Equals(NoneValue.NONE_VALUE) ? string.Empty : unitName
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
