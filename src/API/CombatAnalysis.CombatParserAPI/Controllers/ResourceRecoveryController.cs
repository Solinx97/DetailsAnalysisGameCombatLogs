using CombatParser.Application.Queries.Resources.CountResourceByAll;
using CombatParser.Application.Queries.Resources.CountResourcesByCreator;
using CombatParser.Application.Queries.Resources.CountResourcesBySpell;
using CombatParser.Application.Queries.Resources.GetResources;
using CombatParser.Application.Queries.Resources.GetResourcesByAll;
using CombatParser.Application.Queries.Resources.GetResourcesByCreator;
using CombatParser.Application.Queries.Resources.GetResourcesBySpell;
using CombatParser.Application.Queries.Resources.GetResourcesCount;
using CombatParser.Application.Queries.Resources.GetUniqueResourcesCreators;
using CombatParser.Application.Queries.Resources.GetUniqueResourcesSpells;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CombatAnalysis.CombatParserAPI.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class ResourceRecoveryController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpGet("getByCombatPlayerId")]
    public async Task<IActionResult> GetByCombatPlayerId(int combatPlayerId, int page, int pageSize, CancellationToken cancellationToken)
    {
        var resources = await _mediator.Send(new GetResourcesQuery(combatPlayerId, page, pageSize), cancellationToken);

        return Ok(resources);
    }
    
    [HttpGet("count/{combatPlayerId}")]
    public async Task<IActionResult> Count(int combatPlayerId, CancellationToken cancellationToken)
    {
        var count = await _mediator.Send(new GetResourcesCountQuery(combatPlayerId), cancellationToken);

        return Ok(count);
    }

    [HttpGet("getUniqueCreators/{combatPlayerId}")]
    public async Task<IActionResult> GetUniqueCreators(int combatPlayerId, CancellationToken cancellationToken)
    {
        var uniqueTargets = await _mediator.Send(new GetUniqueResourcesCreatorsQuery(combatPlayerId), cancellationToken);

        return Ok(uniqueTargets);
    }

    [HttpGet("getUniqueSpells/{combatPlayerId}")]
    public async Task<IActionResult> GetUniqueSpells(int combatPlayerId, CancellationToken cancellationToken)
    {
        var uniqueSpells = await _mediator.Send(new GetUniqueResourcesSpellsQuery(combatPlayerId), cancellationToken);

        return Ok(uniqueSpells);
    }

    [HttpGet("countByCreator")]
    public async Task<IActionResult> CountByCreator(int combatPlayerId, string creator, CancellationToken cancellationToken)
    {
        var count = await _mediator.Send(new CountResourcesByCreatorQuery(combatPlayerId, creator), cancellationToken);

        return Ok(count);
    }

    [HttpGet("countBySpell")]
    public async Task<IActionResult> CountBySpell(int combatPlayerId, string spell, CancellationToken cancellationToken)
    {
        var count = await _mediator.Send(new CountResourcesBySpellQuery(combatPlayerId, spell), cancellationToken);

        return Ok(count);
    }

    [HttpGet("countByAll")]
    public async Task<IActionResult> CountByAll(int combatPlayerId, string target, string spell, CancellationToken cancellationToken)
    {
        var count = await _mediator.Send(new CountResourceByAllQuery(combatPlayerId, target, spell), cancellationToken);

        return Ok(count);
    }

    [HttpGet("getByCreator")]
    public async Task<IActionResult> GetByCreator(int combatPlayerId, string creator, int page, int pageSize, CancellationToken cancellationToken)
    {
        var resources = await _mediator.Send(new GetResourcesByCreatorQuery(combatPlayerId, creator, page, pageSize), cancellationToken); ;

        return Ok(resources);
    }

    [HttpGet("getBySpell")]
    public async Task<IActionResult> GetBySpell(int combatPlayerId, string spell, int page, int pageSize, CancellationToken cancellationToken)
    {
        var resources = await _mediator.Send(new GetResourcesBySpellQuery(combatPlayerId, spell, page, pageSize), cancellationToken);

        return Ok(resources);
    }

    [HttpGet("getByAll")]
    public async Task<IActionResult> GetByAll(int combatPlayerId, string target, string spell, int page, int pageSize, CancellationToken cancellationToken)
    {
        var resources = await _mediator.Send(new GetResourcesByAllQuery(combatPlayerId, target, spell, page, pageSize), cancellationToken);

        return Ok(resources);
    }
}
