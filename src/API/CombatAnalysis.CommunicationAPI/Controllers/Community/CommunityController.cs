using CombatAnalysis.CommunicationAPI.Models.Community;
using CombatAnalysis.CommunicationAPI.Partials;
using Communication.Application.Commands.CreateCommunity;
using Communication.Application.Commands.DeleteCommunity;
using Communication.Application.Commands.UpdateCommunity;
using Communication.Application.Commands.UpdateCommunityRules;
using Communication.Application.Queries.CountCommunity;
using Communication.Application.Queries.GetCommunitiesByUserId;
using Communication.Application.Queries.GetCommunity;
using Communication.Application.Queries.GetCommunityById;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CombatAnalysis.CommunicationAPI.Controllers.Community;

[Route("api/v1/[controller]")]
[ApiController]
[Authorize]
public class CommunityController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpGet]
    public async Task<IActionResult> Get(int page, int pageSize, CancellationToken cancellationToken)
    {
        var communitites = await _mediator.Send(new GetCommunityQuery(page, pageSize), cancellationToken);

        return Ok(communitites);
    }

    [HttpGet("{id:int:min(1)}")]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
    {
        var community = await _mediator.Send(new GetCommunityByIdQuery(id), cancellationToken);

        return Ok(community);
    }

    [HttpGet("getByUserId/{appUserId}")]
    public async Task<IActionResult> GetByUserId(string appUserId, int page, int pageSize, CancellationToken cancellationToken)
    {
        var communities = await _mediator.Send(new GetCommunitiesByUserIdQuery(appUserId, page, pageSize), cancellationToken);

        return Ok(communities);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CommunityModel request, CancellationToken cancellationToken)
    {
        var command = new CreateCommunityCommand(request.Name, request.Description, request.PolicyType, request.AppUserId);
        var community = await _mediator.Send(command, cancellationToken);

        return Ok(community);
    }

    [HttpPut("{id:int:min(1)}")]
    public async Task<IActionResult> Update(int id, [FromBody] CommunityPartial request, CancellationToken cancellationToken)
    {
        if (id != request.Id)
        {
            return BadRequest("Route ID and body ID do not match.");
        }

        var command = new UpdateCommunityCommand(request.Id, request.Name, request.Description);
        await _mediator.Send(command, cancellationToken);

        return NoContent();
    }

    [HttpPut("updateRules/{id:int:min(1)}")]
    public async Task<IActionResult> UpdateRules(int id, [FromBody] CommunityRulesPartial request, CancellationToken cancellationToken)
    {
        if (id != request.Id)
        {
            return BadRequest("Route ID and body ID do not match.");
        }

        var command = new UpdateCommunityRulesCommand(request.Id, request.PolicyType);
        await _mediator.Send(command, cancellationToken);

        return NoContent();
    }

    [HttpDelete("{id:int:min(1)}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        await _mediator.Send(new DeleteCommunityCommand(id), cancellationToken);

        return NoContent();
    }

    [HttpGet("count")]
    public async Task<IActionResult> Count(CancellationToken cancellationToken)
    {
        var count = await _mediator.Send(new CountCommunityQuery(), cancellationToken);

        return Ok(count);
    }
}
