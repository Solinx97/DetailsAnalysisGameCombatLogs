using CombatAnalysis.CommunicationAPI.Models.Post;
using Communication.Application.Commands.CreateCommunityPost;
using Communication.Application.Commands.DeleteCommunityPost;
using Communication.Application.Queries.CountCommunityNewPosts;
using Communication.Application.Queries.CountCommunityPost;
using Communication.Application.Queries.GetCommunityPost;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CombatAnalysis.CommunicationAPI.Controllers.Post;

[Route("api/v1/[controller]")]
[ApiController]
[Authorize]
public class CommunityPostController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpGet("countNewPosts/{communityId:int:min(0)}")]
    public async Task<IActionResult> CountNewPosts(int communityId, [FromQuery] DateTimeOffset lastCheck, CancellationToken cancellationToken)
    {
        var count = await _mediator.Send(new CountCommunityNewPostsQuery(communityId, lastCheck), cancellationToken);

        return Ok(count);
    }

    [HttpGet("getByCommunityId/{communityId:int:min(0)}")]
    public async Task<IActionResult> GetByCommunityId(int communityId, string appUserId, int page, int pageSize, CancellationToken cancellationToken)
    {
        var allCommunityPosts = await _mediator.Send(new GetCommunityPostQuery(communityId, appUserId, page, pageSize), cancellationToken);

        return Ok(allCommunityPosts);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CommunityPostModel request, CancellationToken cancellationToken)
    {
        var command = new CreateCommunityPostCommand(request.Content, request.PostType, request.PublicType, request.Restrictions, request.Tags, request.CommunityId, request.AppUserId);
        var post = await _mediator.Send(command, cancellationToken);

        return Ok(post);
    }

    [HttpGet("count/{communityId:int:min(0)}")]
    public async Task<IActionResult> Count(int communityId, CancellationToken cancellationToken)
    {
        var count = await _mediator.Send(new CountCommunityPostQuery(communityId), cancellationToken);

        return Ok(count);
    }

    [HttpDelete("{id:int:min(1)}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        await _mediator.Send(new DeleteCommunityPostCommand(id), cancellationToken);

        return NoContent();
    }
}
