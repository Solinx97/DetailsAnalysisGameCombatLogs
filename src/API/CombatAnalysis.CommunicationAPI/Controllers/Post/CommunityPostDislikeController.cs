using CombatAnalysis.CommunicationAPI.Models.Post;
using Communication.Application.Commands.CreateCommunityPostDislike;
using Communication.Application.Commands.DeleteCommunityPostDislike;
using Communication.Application.Queries.CountCommunityPostDislike;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CombatAnalysis.CommunicationAPI.Controllers.Post;

[Route("api/v1/[controller]")]
[ApiController]
[Authorize]
public class CommunityPostDislikeController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpGet("count/{communityPostId:int:min(1)}")]
    public async Task<IActionResult> Count(int communityPostId, CancellationToken cancellationToken)
    {
        var count = await _mediator.Send(new CountCommunityPostDislikeQuery(communityPostId), cancellationToken);

        return Ok(count);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CommunityPostDislikeModel request, CancellationToken cancellationToken)
    {
        var command = new CreateCommunityPostDislikeCommand(request.CommunityPostId, request.AppUserId);
        await _mediator.Send(command, cancellationToken);

        return NoContent();
    }

    [HttpDelete("{id:int:min(1)}")]
    public async Task<IActionResult> Delete(int id, int communityPostId, CancellationToken cancellationToken)
    {
        await _mediator.Send(new DeleteCommunityPostDislikeCommand(id, communityPostId), cancellationToken);

        return NoContent();
    }
}
