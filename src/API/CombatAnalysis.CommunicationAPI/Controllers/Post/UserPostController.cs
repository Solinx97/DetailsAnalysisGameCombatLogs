using CombatAnalysis.CommunicationAPI.Models.Post;
using CombatAnalysis.CommunicationAPI.Partials;
using Communication.Application.Commands.CreateUserPost;
using Communication.Application.Commands.DeleteUserPost;
using Communication.Application.Commands.UpdateUserPostContent;
using Communication.Application.Queries.GetUserPost;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CombatAnalysis.CommunicationAPI.Controllers.Post;

[Route("api/v1/[controller]")]
[ApiController]
[Authorize]
public class UserPostController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpGet]
    public async Task<IActionResult> Get(int page, int pageSize, CancellationToken cancellationToken)
    {
        var userPosts = await _mediator.Send(new GetUserPostQuery(page, pageSize), cancellationToken);

        return Ok(userPosts);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] UserPostModel request, CancellationToken cancellationToken)
    {
        var command = new CreateUserPostCommand(request.Owner, request.Content, request.PublicType, request.Tags, request.AppUserId);
        var post = await _mediator.Send(command, cancellationToken);

        return Ok(post);
    }

    [HttpPut("{id:int:min(1)}")]
    public async Task<IActionResult> Update(int id, [FromBody] UserPostPartial request, CancellationToken cancellationToken)
    {
        if (id != request.Id)
        {
            return BadRequest("Route ID and body ID do not match.");
        }

        var command = new UpdateUserPostContentCommand(request.Id, request.Content);
        await _mediator.Send(command, cancellationToken);

        return NoContent();
    }

    [HttpDelete("{id:int:min(1)}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        await _mediator.Send(new DeleteUserPostCommand(id), cancellationToken);

        return NoContent();
    }
}