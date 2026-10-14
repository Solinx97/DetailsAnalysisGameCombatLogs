using CombatAnalysis.EnhancedWebApp.Server.Interfaces.HttpClients.Communications;
using CombatAnalysis.EnhancedWebApp.Server.Models.Post;
using Microsoft.AspNetCore.Mvc;

namespace CombatAnalysis.EnhancedWebApp.Server.Controllers.Post;

[Route("api/v1/[controller]")]
[ApiController]
public class UserPostLikeController(IUserPostReactionsApiClient httpClient) : ControllerBase
{
    private readonly IUserPostReactionsApiClient _httpClient = httpClient;

    [HttpGet("count/{communityPostId:int:min(1)}")]
    public async Task<IActionResult> Count(int communityPostId, CancellationToken cancellationToken)
    {
        var count = await _httpClient.CountLikeAsync(communityPostId, cancellationToken);
        return Ok(count);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] UserPostLikeModel like, CancellationToken cancellationToken)
    {
        var createdLike = await _httpClient.CreateLikeAsync(like, cancellationToken);
        return Ok(createdLike);
    }
}
