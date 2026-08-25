using CombatAnalysis.EnhancedWebApp.Server.Attributes;
using CombatAnalysis.EnhancedWebApp.Server.Consts;
using CombatAnalysis.EnhancedWebApp.Server.Interfaces;
using CombatAnalysis.EnhancedWebApp.Server.Models.Community;
using CombatAnalysis.EnhancedWebApp.Server.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace CombatAnalysis.EnhancedWebApp.Server.Controllers.Community;

[ServiceFilter(typeof(RequireAccessTokenAttribute))]
[Route("api/v1/[controller]")]
[ApiController]
public class CommunityUserController : ControllerBase
{
    private readonly IHttpClientHelper _httpClient;

    public CommunityUserController(IOptions<Cluster> cluster, IHttpClientHelper httpClient)
    {
        _httpClient = httpClient;
        _httpClient.APIUrl = cluster.Value.Communication;
    }

    [HttpGet("getByUserId/{appUserId}")]
    public async Task<IActionResult> GetByUserId(string appUserId, int page, int pageSize)
    {
        var responseMessage = await _httpClient.GetAsync($"CommunityUser/getByUserId/{appUserId}?page={page}&pageSize={pageSize}");
        var users = await responseMessage.Content.ReadFromJsonAsync<CommunityUserResponse>();

        return Ok(users);
    }

    [HttpGet("getByCommunityId/{communityId:int:min(1)}")]
    public async Task<IActionResult> GetByCommunityId(int communityId, int page, int pageSize)
    {
        var responseMessage = await _httpClient.GetAsync($"CommunityUser/getByCommunityId/{communityId}?page={page}&pageSize={pageSize}");
        var users = await responseMessage.Content.ReadFromJsonAsync<CommunityUserResponse>();

        return Ok(users);
    }

    [HttpGet("getShortListByCommunityId/{communityId:int:min(1)}")]
    public async Task<IActionResult> GetShortListByCommunityId(int communityId, int pageSize)
    {
        var responseMessage = await _httpClient.GetAsync($"CommunityUser/getByCommunityId/{communityId}?page=1&pageSize={pageSize}");
        var users = await responseMessage.Content.ReadFromJsonAsync<CommunityUserResponse>();

        return Ok(users);
    }

    [HttpGet("canJoin/{appUserId}")]
    public async Task<IActionResult> CanJoin(string appUserId, int communityId)
    {
        var responseMessage = await _httpClient.GetAsync($"CommunityUser/canJoin/{appUserId}?communityId={communityId}");
        var canJoin = await responseMessage.Content.ReadFromJsonAsync<bool>();

        return Ok(canJoin);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CommunityUserModel request)
    {
        await _httpClient.PostAsync("CommunityUser", JsonContent.Create(request));
        return NoContent();
    }

    [HttpDelete("leave")]
    public async Task<IActionResult> Leave(string appUserId, int communityId)
    {
        await _httpClient.DeletAsync($"CommunityUser/leave?appUserId={appUserId}&communityId={communityId}");
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id, int communityId)
    {
        await _httpClient.DeletAsync($"CommunityUser/{id}?communityId={communityId}");
        return NoContent();
    }
}
