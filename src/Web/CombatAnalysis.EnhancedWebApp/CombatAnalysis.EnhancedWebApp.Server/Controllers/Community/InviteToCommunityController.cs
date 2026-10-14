using CombatAnalysis.EnhancedWebApp.Server.Interfaces.HttpClients.Communications;
using CombatAnalysis.EnhancedWebApp.Server.Models.Community;
using Microsoft.AspNetCore.Mvc;

namespace CombatAnalysis.EnhancedWebApp.Server.Controllers.Community;

[Route("api/v1/[controller]")]
[ApiController]
public class InviteToCommunityController(IInviteToCommunityApiClient httpClient) : ControllerBase
{
    private readonly IInviteToCommunityApiClient _httpClient = httpClient;

    [HttpGet("getByUserId/{appUserId}")]
    public async Task<IActionResult> GetByUserId(Guid appUserId, CancellationToken cancellationToken)
    {
        var invites = await _httpClient.GetByUserIdAsync(appUserId, cancellationToken);
        return Ok(invites);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] InviteToCommunityModel request, CancellationToken cancellationToken)
    {
        await _httpClient.CreateAsync(request, cancellationToken);
        return NoContent();
    }

    [HttpDelete("accept/{id:int:min(1)}")]
    public async Task<IActionResult> AcceptRequest(int id, int communityId, Guid appUserId, CancellationToken cancellationToken)
    {
        await _httpClient.AcceptRequestAsync(id, communityId, appUserId, cancellationToken);
        return NoContent();
    }

    [HttpDelete("{id:int:min(1)}")]
    public async Task<IActionResult> Delete(int id, int communityId, CancellationToken cancellationToken)
    {
        await _httpClient.DeleteAsync(id, communityId, cancellationToken);
        return NoContent();
    }
}
