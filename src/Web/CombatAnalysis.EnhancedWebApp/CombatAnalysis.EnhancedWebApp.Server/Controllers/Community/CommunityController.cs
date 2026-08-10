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
public class CommunityController : ControllerBase
{
    private readonly IHttpClientHelper _httpClient;

    public CommunityController(IOptions<Cluster> cluster, IHttpClientHelper httpClient)
    {
        _httpClient = httpClient;
        _httpClient.APIUrl = cluster.Value.Communication;
    }

    [HttpGet]
    public async Task<IActionResult> Get(int page, int pageSize)
    {
        var responseMessage = await _httpClient.GetAsync($"Community?page={page}&pageSize={pageSize}");
        var communities = await responseMessage.Content.ReadFromJsonAsync<IEnumerable<CommunityModel>>();

        return Ok(communities);
    }

    [HttpGet("{id:int:min(1)}")]
    public async Task<IActionResult> GetById(int id)
    {
        var responseMessage = await _httpClient.GetAsync($"Community/{id}");
        var community = await responseMessage.Content.ReadFromJsonAsync<CommunityModel>();

        return Ok(community);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CommunityModel request)
    {
        var responseMessage = await _httpClient.PostAsync("Community", JsonContent.Create(request));
        var community = await responseMessage.Content.ReadFromJsonAsync<CommunityModel>();

        return Ok(community);
    }

    [HttpPut]
    public async Task<IActionResult> Update(CommunityModel request)
    {
        await _httpClient.PutAsync("Community", JsonContent.Create(request));
        return NoContent();
    }

    [HttpDelete("{id:int:min(1)}")]
    public async Task<IActionResult> Delete(int id)
    {
        var responseMessage = await _httpClient.DeletAsync($"Community/{id}");
        return NoContent();
    }

    [HttpGet("count")]
    public async Task<IActionResult> Count()
    {
        var responseMessage = await _httpClient.GetAsync("Community/count");
        var count = await responseMessage.Content.ReadFromJsonAsync<int>();

        return Ok(count);
    }
}
