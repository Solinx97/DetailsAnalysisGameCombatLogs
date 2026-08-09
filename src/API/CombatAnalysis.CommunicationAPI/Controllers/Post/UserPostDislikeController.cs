using CombatAnalysis.CommunicationAPI.Models.Post;
using Communication.Application.Commands.CreateUserPostDislike;
using Communication.Application.Commands.DeleteUserPostDislike;
using Communication.Application.Queries.CountUserPostDislike;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CombatAnalysis.CommunicationAPI.Controllers.Post;

[Route("api/v1/[controller]")]
[ApiController]
[Authorize]
public class UserPostDislikeController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpGet("count/{userPostId:int:min(1)}")]
    public async Task<IActionResult> Count(int userPostId, CancellationToken cancellationToken)
    {
        var count = await _mediator.Send(new CountUserPostDislikeQuery(userPostId), cancellationToken);

        return Ok(count);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] UserPostDislikeModel request, CancellationToken cancellationToken)
    {
        var command = new CreateUserPostDislikeCommand(request.UserPostId, request.AppUserId);
        await _mediator.Send(command, cancellationToken);

        return NoContent();
    }

    [HttpDelete]
    public async Task<IActionResult> Delete(int id, int userPostId, CancellationToken cancellationToken)
    {
        await _mediator.Send(new DeleteUserPostDislikeCommand(id, userPostId), cancellationToken);

        return NoContent();
    }
}
