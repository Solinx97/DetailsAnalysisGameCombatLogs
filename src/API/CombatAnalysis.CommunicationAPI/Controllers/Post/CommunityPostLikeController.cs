using CombatAnalysis.CommunicationAPI.Models.Post;
using Communication.Application.Commands.CreateCommunityPostLike;
using Communication.Application.Queries.CountCommunityPostLike;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CombatAnalysis.CommunicationAPI.Controllers.Post;

[Route("api/v1/[controller]")]
[ApiController]
[Authorize]
public class CommunityPostLikeController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpGet("count/{communityPostId:int:min(1)}")]
    public async Task<IActionResult> Count(int communityPostId, CancellationToken cancellationToken)
    {
        var count = await _mediator.Send(new CountCommunityPostLikeQuery(communityPostId), cancellationToken);

        return Ok(count);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CommunityPostLikeModel request, CancellationToken cancellationToken)
    {
        var command = new CreateCommunityPostLikeCommand(request.CommunityPostId, request.AppUserId);
        await _mediator.Send(command, cancellationToken);

        return NoContent();
    }
}

