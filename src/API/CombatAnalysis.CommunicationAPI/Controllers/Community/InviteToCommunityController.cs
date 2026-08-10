using CombatAnalysis.CommunicationAPI.Models.Community;
using Communication.Application.Commands.CreateInviteToCommunity;
using Communication.Application.Commands.DeleteInviteToCommunity;
using Communication.Application.Queries.GetInvitesToCommunity;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CombatAnalysis.CommunicationAPI.Controllers.Community;

[Route("api/v1/[controller]")]
[ApiController]
[Authorize]
public class InviteToCommunityController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpGet("getByUserId/{appUserId}")]
    public async Task<IActionResult> GetByUserId(string appUserId, CancellationToken cancellationToken)
    {
        var invites = await _mediator.Send(new GetInvitesToCommunityQuery(appUserId), cancellationToken);

        return Ok(invites);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] InviteToCommunityModel request, CancellationToken cancellationToken)
    {
        var command = new CreateInviteToCommunityCommand(request.CommunityId, request.AppUserId, request.ToAppUserId);
        await _mediator.Send(command, cancellationToken);

        return NoContent();
    }

    [HttpDelete("{id:int:min(1)}")]
    public async Task<IActionResult> Delete(int id, int communityId, CancellationToken cancellationToken)
    {
        await _mediator.Send(new DeleteInviteToCommunityCommand(id, communityId), cancellationToken);

        return NoContent();
    }
}
