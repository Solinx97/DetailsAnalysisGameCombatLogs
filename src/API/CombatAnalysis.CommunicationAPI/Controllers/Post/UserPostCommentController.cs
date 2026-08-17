using CombatAnalysis.CommunicationAPI.Models.Post;
using CombatAnalysis.CommunicationAPI.Partials;
using Communication.Application.Commands.CreateUserPostComment;
using Communication.Application.Commands.DeleteUserPostComment;
using Communication.Application.Commands.UpdateUserPostCommentContent;
using Communication.Application.Queries.CountUserPostComment;
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

    [HttpGet("getByUserPostId/{userPostId:int:min(1)}")]
    public async Task<IActionResult> GetByUserPostId(int userPostId, int page, int pageSize, CancellationToken cancellationToken)
    {
        var comments = await _mediator.Send(new GetUserPostCommentsQuery(userPostId, page, pageSize), cancellationToken);

        return Ok(comments);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] UserPostCommentModel request, CancellationToken cancellationToken)
    {
        var command = new CreateUserPostCommentCommand(request.UserPostId, request.Content, request.AppUserId);
        var comment = await _mediator.Send(command, cancellationToken);

        return Ok(comment);
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

    [HttpGet("count/{userPostId:int:min(1)}")]
    public async Task<IActionResult> Count(int userPostId, CancellationToken cancellationToken)
    {
        var count = await _mediator.Send(new CountUserPostCommentQuery(userPostId), cancellationToken);

        return Ok(count);
    }

    [HttpDelete("{id:int:min(1)}")]
    public async Task<IActionResult> Delete(int id, int userPostId, CancellationToken cancellationToken)
    {
        await _mediator.Send(new DeleteUserPostCommentCommand(id, userPostId), cancellationToken);

        return NoContent();
    }
}
