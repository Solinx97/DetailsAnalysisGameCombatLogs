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
public class InviteToCommunityController : ControllerBase
{
    private readonly IHttpClientHelper _httpClient;

    public InviteToCommunityController(IOptions<Cluster> cluster, IHttpClientHelper httpClient)
    {
        _httpClient = httpClient;
        _httpClient.APIUrl = cluster.Value.Communication;
    }

    [HttpGet("getByUserId/{appUserId}")]
    public async Task<IActionResult> GetByUserId(string appUserId)
    {
        var responseMessage = await _httpClient.GetAsync($"InviteToCommunity/getByUserId/{appUserId}");
        var invites = await responseMessage.Content.ReadFromJsonAsync<IEnumerable<InviteToCommunityModel>>();

        return Ok(invites);
    }

    [HttpPost]
    public async Task<IActionResult> Create(InviteToCommunityModel request)
    {
        var responseMessage = await _httpClient.PostAsync("InviteToCommunity", JsonContent.Create(request));
        return NoContent();
    }

    [HttpPut]
    public async Task<IActionResult> Update(InviteToCommunityModel request)
    {
        var responseMessage = await _httpClient.PutAsync("InviteToCommunity", JsonContent.Create(request));
        return NoContent(); ;
    }

    [HttpDelete("{id:int:min(1)}")]
    public async Task<IActionResult> Delete(int id, int communityId)
    {
        var responseMessage = await _httpClient.DeletAsync($"InviteToCommunity/{id}?communityId={communityId}");
        return NoContent();
    }
}
