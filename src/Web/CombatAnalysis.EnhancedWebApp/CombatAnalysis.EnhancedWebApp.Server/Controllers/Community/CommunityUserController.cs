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
public class CommunityUserController : ControllerBase
{
    private readonly IHttpClientHelper _httpClient;

    public CommunityUserController(IOptions<Cluster> cluster, IHttpClientHelper httpClient)
    {
        _httpClient = httpClient;
        _httpClient.APIUrl = cluster.Value.Communication;
    }

    [HttpGet("getByCommunityId/{communityId:int:min(1)}")]
    public async Task<IActionResult> GetByCommunityId(int communityId)
    {
        var responseMessage = await _httpClient.GetAsync($"CommunityUser/getByCommunityId/{communityId}");
        var users = await responseMessage.Content.ReadFromJsonAsync<IEnumerable<CommunityUserModel>>();

        return Ok(users);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CommunityUserModel request)
    {
        await _httpClient.PostAsync("CommunityUser", JsonContent.Create(request));
        return NoContent();
    }

    [HttpPut]
    public async Task<IActionResult> Update(CommunityUserModel request)
    {
        var responseMessage = await _httpClient.PutAsync("CommunityUser", JsonContent.Create(request));
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        var responseMessage = await _httpClient.DeletAsync($"CommunityUser/{id}");
        return NoContent();
    }
}
