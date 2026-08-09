using CombatAnalysis.CommunicationAPI.Models.Post;
using CombatAnalysis.CommunicationAPI.Partials;
using Communication.Application.Commands.CreateUserPostLike;
using Communication.Application.Commands.DeleteUserPostComment;
using Communication.Application.Commands.UpdateUserPostCommentContent;
using Communication.Application.Queries.GetUserPostComments;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CombatAnalysis.CommunicationAPI.Controllers.Post;

[Route("api/v1/[controller]")]
[ApiController]
[Authorize]
public class UserPostCommentController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpGet("getByUserPostId")]
    public async Task<IActionResult> GetByUserPostId(int userPostId, int page, int pageSize, CancellationToken cancellationToken)
    {
        var comments = await _mediator.Send(new GetUserPostCommentsQuery(userPostId, page, pageSize), cancellationToken);

        return Ok(comments);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] UserPostCommentModel request, CancellationToken cancellationToken)
    {
        var command = new CreateUserPostLikeCommand(request.UserPostId, request.AppUserId);
        await _mediator.Send(command, cancellationToken);

        return NoContent();
    }

    [HttpPut("{id:int:min(1)}")]
    public async Task<IActionResult> Update(int id, [FromBody] UserPostCommentPartial request, CancellationToken cancellationToken)
    {
        if (id != request.Id)
        {
            return BadRequest("Route ID and body ID do not match.");
        }

        var command = new UpdateUserPostCommentContentCommand(request.Id, request.UserPostId, request.Content);
        await _mediator.Send(command, cancellationToken);

        return NoContent();
    }

    [HttpDelete]
    public async Task<IActionResult> Delete(int id, int userPostId, CancellationToken cancellationToken)
    {
        await _mediator.Send(new DeleteUserPostCommentCommand(id, userPostId), cancellationToken);

        return NoContent();
    }
}
