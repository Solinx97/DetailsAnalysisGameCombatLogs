using CombatAnalysis.EnhancedWebApp.Server.Interfaces.HttpClients.Communications;
using CombatAnalysis.EnhancedWebApp.Server.Models.Community;
using Microsoft.AspNetCore.Mvc;

namespace CombatAnalysis.EnhancedWebApp.Server.Controllers.Community;

[Route("api/v1/[controller]")]
[ApiController]
public class CommunityUserController(ICommunityUserApiClient httpClient) : ControllerBase
{
    private readonly ICommunityUserApiClient _httpClient = httpClient;

    [HttpGet("getByUserId/{appUserId}")]
    public async Task<IActionResult> GetByUserId(Guid appUserId, int page, int pageSize, CancellationToken cancellationToken)
    {
        var users = await _httpClient.GetByUserIdAsync(appUserId, page, pageSize, cancellationToken);
        return Ok(users);
    }

    [HttpGet("getByCommunityId/{communityId:int:min(1)}")]
    public async Task<IActionResult> GetByCommunityId(int communityId, int page, int pageSize, CancellationToken cancellationToken)
    {
        var users = await _httpClient.GetByCommunityIdAsync(communityId, page, pageSize, cancellationToken);
        return Ok(users);
    }

    [HttpGet("getShortList/{communityId:int:min(1)}")]
    public async Task<IActionResult> GetShortList(int communityId, int pageSize, CancellationToken cancellationToken)
    {
        var users = await _httpClient.GetShortListAsync(communityId, pageSize, cancellationToken);
        return Ok(users);
    }

    [HttpGet("canJoin/{appUserId}")]
    public async Task<IActionResult> CanJoin(Guid appUserId, int communityId, CancellationToken cancellationToken)
    {
        var canJoin = await _httpClient.CanJoinAsync(appUserId, communityId, cancellationToken);
        return Ok(canJoin);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CommunityUserModel user, CancellationToken cancellationToken)
    {
        await _httpClient.CreateAsync(user, cancellationToken);
        return NoContent();
    }

    [HttpDelete("leave")]
    public async Task<IActionResult> Leave(Guid appUserId, int communityId, CancellationToken cancellationToken)
    {
        await _httpClient.LeaveAsync(appUserId, communityId, cancellationToken);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id, int communityId, CancellationToken cancellationToken)
    {
        await _httpClient.DeleteAsync(id, communityId, cancellationToken);
        return NoContent();
    }
}
