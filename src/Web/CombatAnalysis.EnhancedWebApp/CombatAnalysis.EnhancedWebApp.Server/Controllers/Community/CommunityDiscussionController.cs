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
public class CommunityDiscussionController : ControllerBase
{
    private readonly IHttpClientHelper _httpClient;

    public CommunityDiscussionController(IOptions<Cluster> cluster, IHttpClientHelper httpClient)
    {
        _httpClient = httpClient;
        _httpClient.APIUrl = cluster.Value.Communication;
    }

    [HttpGet("getByCommunityId/{communityId:int:min(1)}")]
    public async Task<IActionResult> GetByCommunityId(int communityId, int page, int pageSize)
    {
        var responseMessage = await _httpClient.GetAsync($"CommunityDiscussion/getByCommunityId/{communityId}?page={page}&pageSize={pageSize}");
        var discussions = await responseMessage.Content.ReadFromJsonAsync<IEnumerable<CommunityDiscussionModel>>();

        return Ok(discussions);
    }

    [HttpGet("{id:int:min(1)}")]
    public async Task<IActionResult> GetById(int id)
    {
        var responseMessage = await _httpClient.GetAsync($"CommunityDiscussion/{id}");
        var discussion = await responseMessage.Content.ReadFromJsonAsync<CommunityDiscussionModel>();

        return Ok(discussion);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CommunityDiscussionModel request)
    {
        await _httpClient.PostAsync("CommunityDiscussion", JsonContent.Create(request));
        return NoContent();
    }

    [HttpPut("{id:int:min(1)}")]
    public async Task<IActionResult> Update(int id, CommunityDiscussionModel request)
    {
        await _httpClient.PutAsync($"CommunityDiscussion/{id}", JsonContent.Create(request));
        return NoContent();
    }

    [HttpDelete("{id:int:min(1)}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _httpClient.DeletAsync($"CommunityDiscussion/{id}");
        return NoContent();
    }
}
