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
public class UserPostController : ControllerBase
{
    private readonly IHttpClientHelper _httpClient;

    public UserPostController(IOptions<Cluster> cluster, IHttpClientHelper httpClient)
    {
        _httpClient = httpClient;
        _httpClient.APIUrl = cluster.Value.Communication;
    }

    [HttpGet("getByUserId/{appUserId}")]
    public async Task<IActionResult> GetByUserId(string appUserId, int page, int pageSize)
    {
        var responseMessage = await _httpClient.GetAsync($"UserPost/getByUserId/{appUserId}?page={page}&pageSize={pageSize}");
        var posts = await responseMessage.Content.ReadFromJsonAsync<IEnumerable<UserPostModel>>();

        return Ok(posts);
    }

    [HttpPost]
    public async Task<IActionResult> Create(UserPostModel request)
    {
        var responseMessage = await _httpClient.PostAsync("UserPost", JsonContent.Create(request));
        var post = await responseMessage.Content.ReadFromJsonAsync<UserPostModel>();

        return Ok(post);
    }

    [HttpPut]
    public async Task<IActionResult> Update(UserPostModel request)
    {
        await _httpClient.PutAsync("UserPost", JsonContent.Create(request));
        return NoContent();
    }

    [HttpGet("count/{appUserId}")]
    public async Task<IActionResult> Count(string appUserId, CancellationToken cancellationToken)
    {
        var responseMessage = await _httpClient.GetAsync($"UserPost/count/{appUserId}");
        var count = await responseMessage.Content.ReadFromJsonAsync<int>();

        return Ok(count);
    }

    [HttpDelete("{id:int:min(1)}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _httpClient.DeletAsync($"UserPost/{id}");
        return NoContent();
    }
}
