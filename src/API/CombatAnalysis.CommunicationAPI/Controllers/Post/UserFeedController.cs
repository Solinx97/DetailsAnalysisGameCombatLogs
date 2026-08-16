using Communication.Application.Queries.CountFeedNewPosts;
using Communication.Application.Queries.GetUserFeed;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CombatAnalysis.CommunicationAPI.Controllers.Post;

[Route("api/v1/[controller]")]
[ApiController]
[Authorize]
public class UserFeedController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpGet("countNewPosts/{appUserId}")]
    public async Task<IActionResult> CountNewPosts(string appUserId, [FromQuery] List<string> friendIds, [FromQuery] DateTimeOffset lastCheck, CancellationToken cancellationToken)
    {
        var count = await _mediator.Send(new CountFeedNewPostsQuery(appUserId, friendIds, lastCheck), cancellationToken);

        return Ok(count);
    }

    [HttpGet("{appUserId}")]
    public async Task<IActionResult> GetByUserId(string appUserId, [FromQuery] List<string> friendIds, int page, int pageSize, CancellationToken cancellationToken)
    {
        var userFeed = await _mediator.Send(new GetUserFeedQuery(appUserId, friendIds, page, pageSize), cancellationToken);

        return Ok(userFeed);
    }
}
