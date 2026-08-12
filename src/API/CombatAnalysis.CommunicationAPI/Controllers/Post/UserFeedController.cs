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

    [HttpGet("{appUserId}")]
    public async Task<IActionResult> GetByUserId(string appUserId, int page, int pageSize, CancellationToken cancellationToken)
    {
        var feed = await _mediator.Send(new GetUserFeedQuery(appUserId, page, pageSize), cancellationToken);

        return Ok(feed);
    }
}
