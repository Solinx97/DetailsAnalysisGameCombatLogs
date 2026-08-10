using CombatAnalysis.EnhancedWebApp.Server.Attributes;
using CombatAnalysis.EnhancedWebApp.Server.Consts;
using CombatAnalysis.EnhancedWebApp.Server.Interfaces;
using CombatAnalysis.EnhancedWebApp.Server.Models.Community;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace CombatAnalysis.EnhancedWebApp.Server.Controllers.Community;

[ServiceFilter(typeof(RequireAccessTokenAttribute))]
[Route("api/v1/[controller]")]
[ApiController]
public class CommunityDiscussionCommentController : ControllerBase
{
    private readonly IHttpClientHelper _httpClient;

    public CommunityDiscussionCommentController(IOptions<Cluster> cluster, IHttpClientHelper httpClient)
    {
        _httpClient = httpClient;
        _httpClient.APIUrl = cluster.Value.Communication;
    }

    [HttpGet("getByDiscussionId")]
    public async Task<IActionResult> GetByDiscussionId(int communityId, int page, int pageSize)
    {
        var responseMessage = await _httpClient.GetAsync($"CommunityDiscussionComment/getByDiscussionId?communityId={communityId}&page={page}&pageSize={pageSize}");
        var comments = await responseMessage.Content.ReadFromJsonAsync<IEnumerable<CommunityDiscussionCommentModel>>();

        return Ok(comments);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CommunityDiscussionCommentModel request)
    {
        var responseMessage = await _httpClient.PostAsync("CommunityDiscussionComment", JsonContent.Create(request));
        await responseMessage.Content.ReadFromJsonAsync<CommunityDiscussionCommentModel>();

        return NoContent();
    }

    [HttpPut]
    public async Task<IActionResult> Update(CommunityDiscussionCommentModel request)
    {
        var responseMessage = await _httpClient.PutAsync("CommunityDiscussionComment", JsonContent.Create(request));
        return NoContent();
    }

    [HttpDelete("{id:int:min(1)}")]
    public async Task<IActionResult> Delete(int id, int discussionId)
    {
        var responseMessage = await _httpClient.DeletAsync($"CommunityDiscussionComment/{id}?discussionId={discussionId}");
        return NoContent();
    }
}
