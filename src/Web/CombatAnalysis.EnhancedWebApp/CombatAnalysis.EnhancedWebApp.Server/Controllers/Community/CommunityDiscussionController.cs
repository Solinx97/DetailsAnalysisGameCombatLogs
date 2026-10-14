using CombatAnalysis.EnhancedWebApp.Server.Interfaces.HttpClients.Communications;
using CombatAnalysis.EnhancedWebApp.Server.Models.Community;
using CombatAnalysis.EnhancedWebApp.Server.Patches;
using Microsoft.AspNetCore.Mvc;

namespace CombatAnalysis.EnhancedWebApp.Server.Controllers.Community;

[Route("api/v1/[controller]")]
[ApiController]
public class CommunityDiscussionController(ICommunityDiscussionApiClient httpClient) : ControllerBase
{
    private readonly ICommunityDiscussionApiClient _httpClient = httpClient;

    [HttpGet("getByCommunityId/{communityId:int:min(1)}")]
    public async Task<IActionResult> GetByCommunityId(int communityId, int page, int pageSize, CancellationToken cancellationToken)
    {
        var discussions = await _httpClient.GetAsync(communityId, page, pageSize, cancellationToken);
        return Ok(discussions);
    }

    [HttpGet("getShortList/{communityId:int:min(1)}")]
    public async Task<IActionResult> GetShortList(int communityId, int pageSize, CancellationToken cancellationToken)
    {
        var discussions = await _httpClient.GetShortListAsync(communityId, pageSize, cancellationToken);
        return Ok(discussions);
    }

    [HttpGet("{id:int:min(1)}")]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
    {
        var discussion = await _httpClient.GetByIdAsync(id, cancellationToken);
        return Ok(discussion);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CommunityDiscussionModel discussion, CancellationToken cancellationToken)
    {
        var createdDiscussion = await _httpClient.CreateAsync(discussion, cancellationToken);
        return Ok(createdDiscussion);
    }

    [HttpPatch("{id:int:min(1)}")]
    public async Task<IActionResult> Update(int id, CommunityDiscussionPatch discussion, CancellationToken cancellationToken)
    {
        await _httpClient.UpdateAsync(id, discussion, cancellationToken);
        return NoContent();
    }

    [HttpDelete("{id:int:min(1)}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        await _httpClient.DeleteAsync(id, cancellationToken);
        return NoContent();
    }
}
