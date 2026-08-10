using CombatAnalysis.CommunicationAPI.Models.Post;
using CombatAnalysis.CommunicationAPI.Partials;
using Communication.Application.Commands.CreateCommunityPost;
using Communication.Application.Commands.DeleteCommunityPost;
using Communication.Application.Commands.UpdateCommunityPostContent;
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

    [HttpGet("getByCommunityId")]
    public async Task<IActionResult> GetByCommunityId(int communityId, int page, int pageSize, CancellationToken cancellationToken)
    {
        var communityPosts = await _mediator.Send(new GetCommunityPostQuery(communityId, page, pageSize), cancellationToken);

        return Ok(communityPosts);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CommunityPostModel request, CancellationToken cancellationToken)
    {
        var command = new CreateCommunityPostCommand(request.CommunityName, request.Owner, request.Content, request.PostType, request.PublicType, request.Restrictions, request.Tags, request.CommunityId, request.AppUserId);
        var post = await _mediator.Send(command, cancellationToken);

        return Ok(post);
    }

    [HttpPut("{id:int:min(1)}")]
    public async Task<IActionResult> Update(int id, [FromBody] CommunityPostPartial request, CancellationToken cancellationToken)
    {
        if (id != request.Id)
        {
            return BadRequest("Route ID and body ID do not match.");
        }

        var command = new UpdateCommunityPostContentCommand(request.Id, request.Content);
        await _mediator.Send(command, cancellationToken);

        return NoContent();
    }

    [HttpGet("count/{communityId:int:min(1)}")]
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
