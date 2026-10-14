using CombatAnalysis.EnhancedWebApp.Server.Interfaces.HttpClients.Communications;
using CombatAnalysis.EnhancedWebApp.Server.Models.Community;
using CombatAnalysis.EnhancedWebApp.Server.Patches;
using Microsoft.AspNetCore.Mvc;

namespace CombatAnalysis.EnhancedWebApp.Server.Controllers.Community;

[Route("api/v1/[controller]")]
[ApiController]
public class CommunityDiscussionCommentController(ICommunityDiscussionCommentApiClient httpClient) : ControllerBase
{
    private readonly ICommunityDiscussionCommentApiClient _httpClient = httpClient;

    [HttpGet("getByDiscussionId/{discussionId:int:min(1)}")]
    public async Task<IActionResult> GetByDiscussionId(int discussionId, int page, int pageSize, CancellationToken cancellationToken)
    {
        var comments = await _httpClient.GetAsync(discussionId, page, pageSize, cancellationToken);
        return Ok(comments);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CommunityDiscussionCommentModel comment, CancellationToken cancellationToken)
    {
        var createdComment = await _httpClient.CreateAsync(comment, cancellationToken);
        return Ok(createdComment);
    }

    [HttpPatch("{id:int:min(1)}")]
    public async Task<IActionResult> Update(int id, [FromBody] CommunityDiscussionCommentPatch comment, CancellationToken cancellationToken)
    {
        await _httpClient.UpdateAsync(id, comment,cancellationToken);
        return NoContent();
    }

    [HttpDelete("{id:int:min(1)}")]
    public async Task<IActionResult> Delete(int id, int discussionId, CancellationToken cancellationToken)
    {
        await _httpClient.DeleteAsync(id, discussionId, cancellationToken);
        return NoContent();
    }
}
