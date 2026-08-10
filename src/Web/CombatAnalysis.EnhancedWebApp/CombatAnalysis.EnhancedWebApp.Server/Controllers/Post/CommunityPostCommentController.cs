using CombatAnalysis.EnhancedWebApp.Server.Attributes;
using CombatAnalysis.EnhancedWebApp.Server.Consts;
using CombatAnalysis.EnhancedWebApp.Server.Interfaces;
using CombatAnalysis.EnhancedWebApp.Server.Models.Post;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace CombatAnalysis.EnhancedWebApp.Server.Controllers.Post;

[ServiceFilter(typeof(RequireAccessTokenAttribute))]
[Route("api/v1/[controller]")]
[ApiController]
public class CommunityPostCommentController : ControllerBase
{
    private readonly IHttpClientHelper _httpClient;

    public CommunityPostCommentController(IOptions<Cluster> cluster, IHttpClientHelper httpClient)
    {
        _httpClient = httpClient;
        _httpClient.APIUrl = cluster.Value.Communication;
    }

    [HttpGet("getByCommunityPostId")]
    public async Task<IActionResult> GetByCommunityPostId(int communityPostId, int page, int pageSize)
    {
        var responseMessage = await _httpClient.GetAsync($"CommunityPostComment/getByCommunityPostId?communityPostId={communityPostId}&page={page}&pageSize={pageSize}");
        var comments = await responseMessage.Content.ReadFromJsonAsync<IEnumerable<CommunityPostCommentModel>>();

        return Ok(comments);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CommunityPostCommentModel request)
    {
        var responseMessage = await _httpClient.PostAsync("CommunityPostComment", JsonContent.Create(request));
        return NoContent();
    }

    [HttpPut]
    public async Task<IActionResult> Update(CommunityPostCommentModel request)
    {
        var responseMessage = await _httpClient.PutAsync("CommunityPostComment", JsonContent.Create(request));
        return NoContent();
    }

    [HttpDelete("{id:int:min(1)}")]
    public async Task<IActionResult> Delete(int id, int communityPostId)
    {
        var responseMessage = await _httpClient.DeletAsync($"CommunityPostComment/{id}?communityPostId={communityPostId}");
        return NoContent();
    }
}
