using CombatAnalysis.EnhancedWebApp.Server.Interfaces.HttpClients.Communications;
using CombatAnalysis.EnhancedWebApp.Server.Models.Post;
using CombatAnalysis.EnhancedWebApp.Server.Patches;
using Microsoft.AspNetCore.Mvc;

namespace CombatAnalysis.EnhancedWebApp.Server.Controllers.Post;

[Route("api/v1/[controller]")]
[ApiController]
public class UserPostCommentController(IUserPostReactionsApiClient httpClient) : ControllerBase
{
    private readonly IUserPostReactionsApiClient _httpClient = httpClient;

    [HttpGet("getByUserPostId/{userPostId:int:min(1)}")]
    public async Task<IActionResult> GetByUserPostId(int userPostId, int page, int pageSize, CancellationToken cancellationToken)
    {
        var comments = await _httpClient.GetCommentsAsync(userPostId, page, pageSize, cancellationToken);
        return Ok(comments);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] UserPostCommentModel request, CancellationToken cancellationToken)
    {
        var comment =  await _httpClient.CreateCommentAsync(request, cancellationToken);
        return Ok(comment);
    }

    [HttpPatch("{id:int:min(1)}")]
    public async Task<IActionResult> Update(int id, [FromBody] UserPostCommentPatch request, CancellationToken cancellationToken)
    {
        await _httpClient.UpdateCommentAsync(id, request, cancellationToken);
        return NoContent();
    }

    [HttpGet("count/{userPostId:int:min(1)}")]
    public async Task<IActionResult> Count(int userPostId, CancellationToken cancellationToken)
    {
        var count = await _httpClient.CountCommentsAsync(userPostId, cancellationToken);
        return Ok(count);
    }

    [HttpDelete("{id:int:min(1)}")]
    public async Task<IActionResult> Delete(int id, int userPostId, CancellationToken cancellationToken)
    {
        await _httpClient.DeleteCommentAsync(id, userPostId, cancellationToken);
        return NoContent();
    }
}
