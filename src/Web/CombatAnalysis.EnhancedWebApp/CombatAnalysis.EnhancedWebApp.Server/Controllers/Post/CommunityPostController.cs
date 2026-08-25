using CombatAnalysis.EnhancedWebApp.Server.Attributes;
using CombatAnalysis.EnhancedWebApp.Server.Consts;
using CombatAnalysis.EnhancedWebApp.Server.Interfaces;
using CombatAnalysis.EnhancedWebApp.Server.Models.Post;
using CombatAnalysis.EnhancedWebApp.Server.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace CombatAnalysis.EnhancedWebApp.Server.Controllers.Post;

[ServiceFilter(typeof(RequireAccessTokenAttribute))]
[Route("api/v1/[controller]")]
[ApiController]
public class CommunityPostController : ControllerBase
{
    private readonly IHttpClientHelper _httpClient;

    public CommunityPostController(IOptions<Cluster> cluster, IHttpClientHelper httpClient)
    {
        _httpClient = httpClient;
        _httpClient.APIUrl = cluster.Value.Communication;
    }

    [HttpGet("countNewPosts/{communityId:int:min(0)}")]
    public async Task<IActionResult> CountNewPosts(int communityId, [FromQuery] DateTimeOffset lastCheck)
    {
        var lastCheckValue = Uri.EscapeDataString(lastCheck.ToString("O"));
        var responseMessage = await _httpClient.GetAsync($"CommunityPost/countNewPosts/{communityId}?lastCheck={lastCheckValue}");

        var count = await responseMessage.Content.ReadFromJsonAsync<int>();

        return Ok(count);
    }

    [HttpGet("getByCommunityId/{communityId:int:min(0)}")]
    public async Task<IActionResult> GetByCommunityId(int communityId, string appUserId, int page, int pageSize)
    {
        var responseMessage = await _httpClient.GetAsync($"CommunityPost/getByCommunityId/{communityId}?appUserId={appUserId}&page={page}&pageSize={pageSize}");
        var comunityPosts = await responseMessage.Content.ReadFromJsonAsync<ComunityPostsResponse>();

        return Ok(comunityPosts);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CommunityPostModel request)
    {
        var responseMessage = await _httpClient.PostAsync("CommunityPost", JsonContent.Create(request));
        var communityPost = await responseMessage.Content.ReadFromJsonAsync<CommunityPostModel>();

        return Ok(communityPost);
    }

    [HttpGet("count/{communityId:int:min(0)}")]
    public async Task<IActionResult> Count(int communityId, CancellationToken cancellationToken)
    {
        var responseMessage = await _httpClient.GetAsync($"CommunityPost/count/{communityId}");
        var count = await responseMessage.Content.ReadFromJsonAsync<int>();

        return Ok(count);
    }

    [HttpDelete("{id:int:min(1)}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _httpClient.DeletAsync($"CommunityPost/{id}");
        return NoContent();
    }
}
