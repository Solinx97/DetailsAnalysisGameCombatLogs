using CombatAnalysis.EnhancedWebApp.Server.Attributes;
using CombatAnalysis.EnhancedWebApp.Server.Consts;
using CombatAnalysis.EnhancedWebApp.Server.Interfaces;
using CombatAnalysis.EnhancedWebApp.Server.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace CombatAnalysis.EnhancedWebApp.Server.Controllers.Post;

[ServiceFilter(typeof(RequireAccessTokenAttribute))]
[Route("api/v1/[controller]")]
[ApiController]
public class UserFeedController : ControllerBase
{
    private readonly IHttpClientHelper _httpClient;

    public UserFeedController(IOptions<Cluster> cluster, IHttpClientHelper httpClient)
    {
        _httpClient = httpClient;
        _httpClient.APIUrl = cluster.Value.Communication;
    }

    [HttpGet("{appUserId}")]
    public async Task<IActionResult> GetByUserId(string appUserId, int page, int pageSize)
    {
        var responseMessage = await _httpClient.GetAsync($"UserFeed/{appUserId}?page={page}&pageSize={pageSize}");
        var userFeed = await responseMessage.Content.ReadFromJsonAsync<UserFeedResponse>();

        return Ok(userFeed);
    }
}
