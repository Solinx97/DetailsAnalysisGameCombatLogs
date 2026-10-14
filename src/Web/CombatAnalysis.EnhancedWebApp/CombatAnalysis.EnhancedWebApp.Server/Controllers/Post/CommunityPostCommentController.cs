using CombatAnalysis.EnhancedWebApp.Server.Interfaces.HttpClients.Communications;
using CombatAnalysis.EnhancedWebApp.Server.Models.Post;
using CombatAnalysis.EnhancedWebApp.Server.Patches;
using Microsoft.AspNetCore.Mvc;

namespace CombatAnalysis.EnhancedWebApp.Server.Controllers.Post;

[Route("api/v1/[controller]")]
[ApiController]
public class CommunityPostCommentController(ICommunityPostReactionsApiClient httpClient) : ControllerBase
{
    private readonly ICommunityPostReactionsApiClient _httpClient = httpClient;

    [HttpGet("getByCommunityPostId/{communityPostId:int:min(1)}")]
    public async Task<IActionResult> GetByCommunityPostId(int communityPostId, int page, int pageSize, CancellationToken cancellationToken)
    {
        var comments = await _httpClient.GetCommentsAsync(communityPostId, page, pageSize, cancellationToken);
        return Ok(comments);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CommunityPostCommentModel comment, CancellationToken cancellationToken)
    {
        var createdComment = await _httpClient.CreateCommentAsync(comment, cancellationToken);
        return Ok(createdComment);
    }

    [HttpPatch("{id:int:min(1)}")]
    public async Task<IActionResult> Update(int id, [FromBody] CommunityPostCommentPatch comment, CancellationToken cancellationToken)
    {
        await _httpClient.UpdateCommentAsync(id, comment, cancellationToken);
        return NoContent();
    }

    [HttpGet("count/{communityPostId:int:min(1)}")]
    public async Task<IActionResult> Count(int communityPostId, CancellationToken cancellationToken)
    {
        var count = await _httpClient.CountCommentsAsync(communityPostId, cancellationToken);
        return Ok(count);
    }

    [HttpDelete("{id:int:min(1)}")]
    public async Task<IActionResult> Delete(int id, int communityPostId, CancellationToken cancellationToken)
    {
        await _httpClient.DeleteCommentAsync(id, communityPostId, cancellationToken);
        return NoContent();
    }
}
