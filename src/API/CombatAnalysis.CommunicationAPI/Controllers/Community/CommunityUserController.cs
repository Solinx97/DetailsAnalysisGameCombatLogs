using CombatAnalysis.CommunicationAPI.Models.Community;
using Communication.Application.Commands.CreateCommunityUser;
using Communication.Application.Commands.DeleteCommunityUser;
using Communication.Application.Commands.LeaveCommunityUser;
using Communication.Application.Queries.GetCommunityUsers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CombatAnalysis.CommunicationAPI.Controllers.Community;

[Route("api/v1/[controller]")]
[ApiController]
[Authorize]
public class CommunityUserController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpGet("getByCommunityId/{communityId:int:min(1)}")]
    public async Task<IActionResult> GetByCommunityId(int communityId, CancellationToken cancellationToken)
    {
        var communitityUsers = await _mediator.Send(new GetCommunityUsersQuery(communityId), cancellationToken);

        return Ok(communitityUsers);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CommunityUserModel request, CancellationToken cancellationToken)
    {
        var command = new CreateCommunityUserCommand(request.CommunityId, request.AppUserId);
        await _mediator.Send(command, cancellationToken);

        return NoContent();
    }

    [HttpDelete("leave")]
    public async Task<IActionResult> Leave(string appUserId, int communityId, CancellationToken cancellationToken)
    {
        await _mediator.Send(new LeaveCommunityUserCommand(appUserId, communityId), cancellationToken);

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id, int communityId, CancellationToken cancellationToken)
    {
        await _mediator.Send(new DeleteCommunityUserCommand(id, communityId), cancellationToken);

        return NoContent();
    }
}
