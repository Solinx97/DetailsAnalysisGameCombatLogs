using CombatAnalysis.CommunicationAPI.Models.Community;
using Communication.Application.Commands.CreateCommunity;
using Communication.Application.Commands.UpdateCommunityName;
using Communication.Application.Queries.GetCommunity;
using Communication.Application.Queries.GetCommunityById;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CombatAnalysis.CommunicationAPI.Controllers.Community;

[Route("api/v1/[controller]")]
[ApiController]
[Authorize]
public class CommunityController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpGet]
    public async Task<IActionResult> Get(int page, int pageSize, CancellationToken cancellationToken)
    {
        var communitites = await _mediator.Send(new GetCommunityQuery(page, pageSize), cancellationToken);

        return Ok(communitites);
    }

    [HttpGet("{id:int:min(1)}")]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
    {
        var community = await _mediator.Send(new GetCommunityByIdQuery(id), cancellationToken);

        return Ok(community);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CommunityModel request, CancellationToken cancellationToken)
    {
        var command = new CreateCommunityCommand(request.Name, request.Description, request.PolicyType, request.AppUserId);
        var community = await _mediator.Send(command, cancellationToken);

        return Ok(community);
    }

    [HttpPut("{id:int:min(1)}")]
    public async Task<IActionResult> UpdateName(int id, [FromBody] CommunityModel request, CancellationToken cancellationToken)
    {
        if (id != request.Id)
        {
            return BadRequest("Route ID and body ID do not match.");
        }

        var command = new UpdateCommunityNameCommand(request.Id, request.Name);
        await _mediator.Send(command, cancellationToken);

        return NoContent();
    }

    //[HttpDelete("{id:int:min(1)}")]
    //public async Task<IActionResult> Delete(int id)
    //{
    //    try
    //    {
    //        await _service.DeleteAsync(id);

    //        return NoContent();
    //    }
    //    catch (DbUpdateConcurrencyException ex)
    //    {
    //        _logger.LogWarning(ex, "The resource was modified by another user. Please refresh and try again.");

    //        return Conflict(new { message = "The resource was modified by another user. Please refresh and try again." });
    //    }
    //}

    //[HttpGet("count")]
    //public async Task<IActionResult> Count()
    //{
    //    var count = await _service.CountAsync();

    //    return Ok(count);
    //}
}
