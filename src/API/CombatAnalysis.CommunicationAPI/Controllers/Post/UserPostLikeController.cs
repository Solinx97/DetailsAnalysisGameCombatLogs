using CombatAnalysis.CommunicationAPI.Models.Post;
using Communication.Application.Commands.CreateUserPostLike;
using Communication.Application.Commands.DeleteUserPostLike;
using Communication.Application.Queries.CountUserPostLike;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CombatAnalysis.CommunicationAPI.Controllers.Post;

[Route("api/v1/[controller]")]
[ApiController]
[Authorize]
public class UserPostLikeController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpGet("count/{userPostId:int:min(1)}")]
    public async Task<IActionResult> Count(int userPostId, CancellationToken cancellationToken)
    {
        var count = await _mediator.Send(new CountUserPostLikeQuery(userPostId), cancellationToken);

        return Ok(count);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] UserPostDislikeModel request, CancellationToken cancellationToken)
    {
        var command = new CreateUserPostLikeCommand(request.UserPostId, request.AppUserId);
        await _mediator.Send(command, cancellationToken);

        return NoContent();
    }

    [HttpDelete("{id:int:min(1)}")]
    public async Task<IActionResult> Delete(int id, int userPostId, CancellationToken cancellationToken)
    {
        await _mediator.Send(new DeleteUserPostLikeCommand(id, userPostId), cancellationToken);

        return NoContent();
    }
}
