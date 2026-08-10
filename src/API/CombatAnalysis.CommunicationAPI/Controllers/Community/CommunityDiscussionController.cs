using CombatAnalysis.CommunicationAPI.Models.Community;
using CombatAnalysis.CommunicationAPI.Partials;
using Communication.Application.Commands.CreateCommunityDescussion;
using Communication.Application.Commands.DeleteCommunityDiscussion;
using Communication.Application.Commands.UpdateCommunityDiscussionTitle;
using Communication.Application.Queries.GetCommunityDiscussionById;
using Communication.Application.Queries.GetCommunityDiscussions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CombatAnalysis.CommunicationAPI.Controllers.Community;

[Route("api/v1/[controller]")]
[ApiController]
[Authorize]
public class CommunityDiscussionController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpGet("getByCommunityId/{communityId:int:min(1)}")]
    public async Task<IActionResult> GetByCommunityId(int communityId, int page, int pageSize, CancellationToken cancellationToken)
    {
        var discussions = await _mediator.Send(new GetCommunityDiscussionsQuery(communityId, page, pageSize), cancellationToken);

        return Ok(discussions);
    }

    [HttpGet("{id:int:min(1)}")]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
    {
        var discussion = await _mediator.Send(new GetCommunityDiscussionByIdQuery(id), cancellationToken);

        return Ok(discussion);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CommunityDiscussionModel request, CancellationToken cancellationToken)
    {
        var command = new CreateCommunityDescussionCommand(request.Title, request.Content, request.CommunityId, request.AppUserId);
        await _mediator.Send(command, cancellationToken);

        return NoContent();
    }

    [HttpPut("{id:int:min(1)}")]
    public async Task<IActionResult> Update(int id, [FromBody] CommunityDiscussionPartial request, CancellationToken cancellationToken)
    {
        if (id != request.Id)
        {
            return BadRequest("Route ID and body ID do not match.");
        }

        var command = new UpdateCommunityDiscussionTitleCommand(request.Id, request.Title);
        await _mediator.Send(command, cancellationToken);

        return NoContent();
    }

    [HttpDelete("{id:int:min(1)}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        await _mediator.Send(new DeleteCommunityDiscussionComand(id), cancellationToken);

        return NoContent();
    }
}
