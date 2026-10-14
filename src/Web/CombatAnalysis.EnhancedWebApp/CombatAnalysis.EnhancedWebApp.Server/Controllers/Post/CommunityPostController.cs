using CombatAnalysis.EnhancedWebApp.Server.Interfaces.HttpClients.Communications;
using CombatAnalysis.EnhancedWebApp.Server.Models.Post;
using Microsoft.AspNetCore.Mvc;

namespace CombatAnalysis.EnhancedWebApp.Server.Controllers.Post;

[Route("api/v1/[controller]")]
[ApiController]
public class CommunityPostController(ICommunityPostApiClient httpClient) : ControllerBase
{
    private readonly ICommunityPostApiClient _httpClient = httpClient;

    [HttpGet("getByCommunityId/{communityId:int:min(0)}")]
    public async Task<IActionResult> GetByCommunityId(int communityId, Guid appUserId, int page, int pageSize, CancellationToken cancellationToken)
    {
        var comunityPosts = await _httpClient.GetByCommunityIdAsync(communityId, appUserId, page, pageSize, cancellationToken);
        return Ok(comunityPosts);
    }

    [HttpGet("countNewPosts/{communityId:int:min(0)}")]
    public async Task<IActionResult> CountNewPosts(int communityId, [FromQuery] DateTimeOffset lastCheck, CancellationToken cancellationToken)
    {
        var count = await _httpClient.CountNewPostsAsync(communityId, lastCheck, cancellationToken);
        return Ok(count);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CommunityPostModel post, CancellationToken cancellationToken)
    {
        var communityPost = await _httpClient.CreateAsync(post, cancellationToken);
        return Ok(communityPost);
    }

    [HttpDelete("{id:int:min(1)}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        await _httpClient.DeleteAsync(id, cancellationToken);
        return NoContent();
    }
}
