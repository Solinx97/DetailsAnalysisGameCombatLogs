using CombatAnalysis.CommunicationAPI.Models.Community;
using Communication.Application.Commands.CreateCommunityUser;
using Communication.Application.Commands.DeleteCommunityUser;
using Communication.Application.Commands.LeaveCommunityUser;
using Communication.Application.Queries.CanJoinToCommunity;
using Communication.Application.Queries.GetCommunityUsers;
using Communication.Application.Queries.GetCommunityUsersByUserId;
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

    [HttpGet("getByUserId/{appUserId}")]
    public async Task<IActionResult> GetByUserId(string appUserId, int page, int pageSize, CancellationToken cancellationToken)
    {
        var communitityUsers = await _mediator.Send(new GetCommunityUsersByUserIdQuery(appUserId, page, pageSize), cancellationToken);

        return Ok(communitityUsers);
    }

    [HttpGet("getByCommunityId/{communityId:int:min(1)}")]
    public async Task<IActionResult> GetByCommunityId(int communityId, int page, int pageSize, CancellationToken cancellationToken)
    {
        var communitityUsers = await _mediator.Send(new GetCommunityUsersQuery(communityId, page, pageSize), cancellationToken);

        return Ok(communitityUsers);
    }

    [HttpGet("canJoin/{appUserId}")]
    public async Task<IActionResult> CanJoin(string appUserId, int communityId, CancellationToken cancellationToken)
    {
        var canJoin = await _mediator.Send(new CanJoinToCommunityQuery(appUserId, communityId), cancellationToken);

        return Ok(canJoin);
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
