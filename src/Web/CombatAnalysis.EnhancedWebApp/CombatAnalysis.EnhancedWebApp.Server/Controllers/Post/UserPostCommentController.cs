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
public class UserPostCommentController : ControllerBase
{
    private readonly IHttpClientHelper _httpClient;

    public UserPostCommentController(IOptions<Cluster> cluster, IHttpClientHelper httpClient)
    {
        _httpClient = httpClient;
        _httpClient.APIUrl = cluster.Value.Communication;
    }

    [HttpGet("getByUserPostId/{userPostId:int:min(1)}")]
    public async Task<IActionResult> GetByUserPostId(int userPostId, int page, int pageSize)
    {
        var responseMessage = await _httpClient.GetAsync($"UserPostComment/getByUserPostId/{userPostId}?page={page}&pageSize={pageSize}");
        var comments = await responseMessage.Content.ReadFromJsonAsync<UserPostCommentResponse>();

        return Ok(comments);
    }

    [HttpPost]
    public async Task<IActionResult> Create(UserPostCommentModel request)
    {
        var responseMessage =  await _httpClient.PostAsync("UserPostComment", JsonContent.Create(request));
        var comment = await responseMessage.Content.ReadFromJsonAsync<UserPostCommentModel>();

        return Ok(comment);
    }

    [HttpPut("{id:int:min(1)}")]
    public async Task<IActionResult> Update(int id, UserPostCommentModel request)
    {
        await _httpClient.PutAsync($"UserPostComment/{id}", JsonContent.Create(request));
        return NoContent();
    }

    [HttpGet("count/{userPostId:int:min(1)}")]
    public async Task<IActionResult> Count(int userPostId)
    {
        var responseMessage = await _httpClient.GetAsync($"UserPostComment/count/{userPostId}");
        var count = await responseMessage.Content.ReadFromJsonAsync<int>();

        return Ok(count);
    }

    [HttpDelete("{id:int:min(1)}")]
    public async Task<IActionResult> Delete(int id, int userPostId)
    {
        var responseMessage = await _httpClient.DeletAsync($"UserPostComment/{id}?userPostId={userPostId}");
        return NoContent();
    }
}
