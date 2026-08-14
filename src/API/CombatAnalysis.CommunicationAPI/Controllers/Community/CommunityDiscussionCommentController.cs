using CombatAnalysis.CommunicationAPI.Models.Community;
using CombatAnalysis.CommunicationAPI.Partials;
using Communication.Application.Commands.CreateDiscussionComment;
using Communication.Application.Commands.DeleteDiscussionComment;
using Communication.Application.Commands.UpdateDiscussionCommentContent;
using Communication.Application.Queries.GetCommunityDiscussionComments;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CombatAnalysis.CommunicationAPI.Controllers.Community;

[Route("api/v1/[controller]")]
[ApiController]
[Authorize]
public class CommunityDiscussionCommentController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpGet("getByDiscussionId/{communityId:int:min(1)}")]
    public async Task<IActionResult> GetByDiscussionId(int communityId, int page, int pageSize, CancellationToken cancellationToken)
    {
        var comments = await _mediator.Send(new GetCommunityDiscussionCommentsQuery(communityId, page, pageSize), cancellationToken);

        return Ok(comments);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CommunityDiscussionCommentModel request, CancellationToken cancellationToken)
    {
        var command = new CreateDiscussionCommentCommand(request.CommunityDiscussionId, request.Content, request.AppUserId);
        await _mediator.Send(command, cancellationToken);

        return NoContent();
    }

    [HttpPut("{id:int:min(1)}")]
    public async Task<IActionResult> Update(int id, [FromBody] CommunityDiscussionCommentPartial request, CancellationToken cancellationToken)
    {
        if (id != request.Id)
        {
            return BadRequest("Route ID and body ID do not match.");
        }

        var command = new UpdateDiscussionCommentContentCommand(request.Id, request.CommunityDiscussionId, request.Content);
        await _mediator.Send(command, cancellationToken);

        return NoContent();
    }

    [HttpDelete("{id:int:min(1)}")]
    public async Task<IActionResult> Delete(int id, int discussionId, CancellationToken cancellationToken)
    {
        await _mediator.Send(new DeleteDiscussionCommentCommand(id, discussionId), cancellationToken);

        return NoContent();
    }
}
