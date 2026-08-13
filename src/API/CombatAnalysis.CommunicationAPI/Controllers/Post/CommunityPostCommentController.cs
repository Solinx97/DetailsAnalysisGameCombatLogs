using CombatAnalysis.CommunicationAPI.Models.Post;
using CombatAnalysis.CommunicationAPI.Partials;
using Communication.Application.Commands.CreateCommunityPostComment;
using Communication.Application.Commands.DeleteCommunityPostComment;
using Communication.Application.Commands.UpdateCommunityPostCommentContent;
using Communication.Application.Queries.CountCommunityPostComment;
using Communication.Application.Queries.GetCommunityPostComments;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CombatAnalysis.CommunicationAPI.Controllers.Post;

[Route("api/v1/[controller]")]
[ApiController]
[Authorize]
public class CommunityPostCommentController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpGet("getByCommunityPostId/{communityPostId:int:min(1)}")]
    public async Task<IActionResult> GetByCommunityPostId(int communityPostId, int page, int pageSize, CancellationToken cancellationToken)
    {
        var comments = await _mediator.Send(new GetCommunityPostCommentsQuery(communityPostId, page, pageSize), cancellationToken);

        return Ok(comments);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CommunityPostCommentModel request, CancellationToken cancellationToken)
    {
        var command = new CreateCommunityPostCommentCommand(request.CommunityId, request.CommunityPostId, request.Content, request.CommentType, request.AppUserId);
        await _mediator.Send(command, cancellationToken);

        return NoContent();
    }

    [HttpPut("{id:int:min(1)}")]
    public async Task<IActionResult> Update(int id, [FromBody] CommunityPostCommentPartial request, CancellationToken cancellationToken)
    {
        if (id != request.Id)
        {
            return BadRequest("Route ID and body ID do not match.");
        }

        var command = new UpdateCommunityPostCommentContentCommand(request.Id, request.CommunityPostId, request.Content);
        await _mediator.Send(command, cancellationToken);

        return NoContent();
    }

    [HttpGet("count/{communityPostId:int:min(1)}")]
    public async Task<IActionResult> Count(int communityPostId, CancellationToken cancellationToken)
    {
        var count = await _mediator.Send(new CountCommunityPostCommentQuery(communityPostId), cancellationToken);

        return Ok(count);
    }

    [HttpDelete("{id:int:min(1)}")]
    public async Task<IActionResult> Delete(int id, int communityPostId, CancellationToken cancellationToken)
    {
        await _mediator.Send(new DeleteCommunityPostCommentCommand(id, communityPostId), cancellationToken);

        return NoContent();
    }
}
