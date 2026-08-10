using CombatAnalysis.CommunicationAPI.Models.Community;
using Communication.Application.Commands.CreateCommunity;
using Communication.Application.Commands.DeleteCommunity;
using Communication.Application.Commands.UpdateCommunityName;
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
    public async Task<IActionResult> GetByUserId(string appUserId, CancellationToken cancellationToken)
    {
        var communitityUsers = await _mediator.Send(new GetCommunitiesByUserIdQuery(appUserId), cancellationToken);

        return Ok(communitityUsers);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CommunityModel request, CancellationToken cancellationToken)
    {
        var command = new CreateCommunityCommand(request.Name, request.Description, request.PolicyType, request.AppUserId);
        var community = await _mediator.Send(command, cancellationToken);

        return Ok(community);
    }

    [HttpPut("{id:int:min(1)}")]
    public async Task<IActionResult> UpdateName(int id, [FromBody] CommunityModel request, CancellationToken cancellationToken)
    {
        if (id != request.Id)
        {
            return BadRequest("Route ID and body ID do not match.");
        }

        var command = new UpdateCommunityNameCommand(request.Id, request.Name);
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
