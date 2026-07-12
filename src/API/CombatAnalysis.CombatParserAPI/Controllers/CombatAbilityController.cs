using CombatParser.Application.Queries.GetAbilitiesByAbilityType;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CombatAnalysis.CombatParserAPI.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class CombatAbilityController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpGet]
    public async Task<IActionResult> GetByAbilityType(int combatPlayerId, [FromQuery] int[] abilityTypes, CancellationToken cancellationToken)
    {
        var abilities = await _mediator.Send(new GetAbilitiesByAbilityTypeQuery(combatPlayerId, abilityTypes), cancellationToken);

        return Ok(abilities);
    }
}
