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
public class CommunityPostLikeController : ControllerBase
{
    private readonly IHttpClientHelper _httpClient;

    public CommunityPostLikeController(IOptions<Cluster> cluster, IHttpClientHelper httpClient)
    {
        _httpClient = httpClient;
        _httpClient.APIUrl = cluster.Value.Communication;
    }

    [HttpGet("count/{communityPostId:int:min(1)}")]
    public async Task<IActionResult> Count(int communityPostId)
    {
        var responseMessage = await _httpClient.GetAsync($"CommunityPostLike/count/{communityPostId}");
        var count = await responseMessage.Content.ReadFromJsonAsync<int>();

        return Ok(count);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CommunityPostLikeModel request)
    {
        var responseMessage = await _httpClient.PostAsync("CommunityPostLike", JsonContent.Create(request));
        var like = await responseMessage.Content.ReadFromJsonAsync<CommunityPostLikeModel>();

        return Ok(like);
    }
}
