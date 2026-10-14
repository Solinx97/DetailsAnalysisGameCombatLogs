using CombatAnalysis.EnhancedWebApp.Server.Interfaces.HttpClients.Communications;
using Microsoft.AspNetCore.Mvc;

namespace CombatAnalysis.EnhancedWebApp.Server.Controllers.Post;

[Route("api/v1/[controller]")]
[ApiController]
public class UserFeedController(IUserFeedApiClient httpClient) : ControllerBase
{
    private readonly IUserFeedApiClient _httpClient = httpClient;

    [HttpGet("countNewPosts/{appUserId}")]
    public async Task<IActionResult> CountNewPosts(Guid appUserId, [FromQuery] DateTimeOffset lastCheck, CancellationToken cancellationToken)
    {
        var count = await _httpClient.CountNewPostsAsync(appUserId, lastCheck, cancellationToken);
        return Ok(count);
    }

    [HttpGet("{appUserId}")]
    public async Task<IActionResult> GetByUserId(Guid appUserId, int page, int pageSize, CancellationToken cancellationToken)
    {
        var posts = await _httpClient.GetByUserIdAsync(appUserId, page, pageSize, cancellationToken);
        return Ok(posts);
    }
}
