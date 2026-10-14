using CombatAnalysis.EnhancedWebApp.Server.Interfaces.HttpClients.Communications;
using CombatAnalysis.EnhancedWebApp.Server.Models.Post;
using Microsoft.AspNetCore.Mvc;

namespace CombatAnalysis.EnhancedWebApp.Server.Controllers.Post;

[Route("api/v1/[controller]")]
[ApiController]
public class UserPostController(IUserPostApiClient httpClient) : ControllerBase
{
    private readonly IUserPostApiClient _httpClient = httpClient;

    [HttpGet("getByUserId/{appUserId}")]
    public async Task<IActionResult> GetByUserId(Guid appUserId, int page, int pageSize, CancellationToken cancellationToken)
    {
        var posts = await _httpClient.GetByUserIdAsync(appUserId, page, pageSize, cancellationToken);
        return Ok(posts);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] UserPostModel post, CancellationToken cancellationToken)
    {
        var createdPost = await _httpClient.CreateAsync(post, cancellationToken);
        return Ok(createdPost);
    }

    [HttpDelete("{id:int:min(1)}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        await _httpClient.DeleteAsync(id, cancellationToken);
        return NoContent();
    }
}
