using CombatAnalysis.EnhancedWebApp.Server.Attributes;
using CombatAnalysis.EnhancedWebApp.Server.Consts;
using CombatAnalysis.EnhancedWebApp.Server.Interfaces;
using CombatAnalysis.EnhancedWebApp.Server.Models.User;
using CombatAnalysis.EnhancedWebApp.Server.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace CombatAnalysis.EnhancedWebApp.Server.Controllers.Post;

[ServiceFilter(typeof(RequireAccessTokenAttribute))]
[Route("api/v1/[controller]")]
[ApiController]
public class UserFeedController(IOptions<Cluster> cluster, IHttpClientHelper httpClient) : ControllerBase
{
    private readonly IOptions<Cluster> _cluster = cluster;
    private readonly IHttpClientHelper _httpClient = httpClient;

    [HttpGet("countNewPosts/{appUserId}")]
    public async Task<IActionResult> CountNewPosts(string appUserId, [FromQuery] DateTimeOffset lastCheck)
    {
        _httpClient.APIUrl = _cluster.Value.User;
        var responseMessage = await _httpClient.GetAsync($"Friend/findByUserId/{appUserId}");
        var friends = await responseMessage.Content.ReadFromJsonAsync<IEnumerable<FriendModel>>()
            ?? Enumerable.Empty<FriendModel>();

        var friendIds = friends
            .Select(x =>
                x.WhoFriendId == appUserId
                    ? x.ForWhomId
                    : x.WhoFriendId)
            .ToList();

        _httpClient.APIUrl = _cluster.Value.Communication;
        var query = string.Join("&", friendIds.Select(id => $"friendIds={Uri.EscapeDataString(id)}"));
        var lastCheckValue = Uri.EscapeDataString(lastCheck.ToString("O"));
        responseMessage = await _httpClient.GetAsync($"UserFeed/countNewPosts/{appUserId}?{query}&lastCheck={lastCheckValue}");

        var count = await responseMessage.Content.ReadFromJsonAsync<int>();

        return Ok(count);
    }

    [HttpGet("{appUserId}")]
    public async Task<IActionResult> GetByUserId(string appUserId, int page, int pageSize)
    {
        _httpClient.APIUrl = _cluster.Value.User;
        var responseMessage = await _httpClient.GetAsync($"Friend/findByUserId/{appUserId}");
        var friends = await responseMessage.Content.ReadFromJsonAsync<IEnumerable<FriendModel>>() 
            ?? Enumerable.Empty<FriendModel>();

        var friendIds = friends
            .Select(x =>
                x.WhoFriendId == appUserId
                    ? x.ForWhomId
                    : x.WhoFriendId)
            .ToList();

        _httpClient.APIUrl = _cluster.Value.Communication;
        var query = string.Join("&", friendIds.Select(id => $"friendIds={Uri.EscapeDataString(id)}"));
        responseMessage = await _httpClient.GetAsync($"UserFeed/{appUserId}?{query}&page={page}&pageSize={pageSize}");

        var userFeed = await responseMessage.Content.ReadFromJsonAsync<UserFeedResponse>();

        return Ok(userFeed);
    }
}
