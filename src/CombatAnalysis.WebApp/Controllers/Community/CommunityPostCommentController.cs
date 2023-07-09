using CombatAnalysis.WebApp.Consts;
using CombatAnalysis.WebApp.Interfaces;
using CombatAnalysis.WebApp.Models.Community;
using Microsoft.AspNetCore.Mvc;

namespace CombatAnalysis.WebApp.Controllers.Community;

[Route("api/v1/[controller]")]
[ApiController]
public class CommunityPostCommentController : ControllerBase
{
    private readonly IHttpClientHelper _httpClient;

    public CommunityPostCommentController(IHttpClientHelper httpClient)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = Port.UserApi;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var responseMessage = await _httpClient.GetAsync($"CommunityPostComment/{id}");
        var postComment = await responseMessage.Content.ReadFromJsonAsync<CommunityPostCommentModel>();

        return Ok(postComment);
    }

    [HttpGet("searchByPostId/{id}")]
    public async Task<IActionResult> SearchByPostId(int id)
    {
        var responseMessage = await _httpClient.GetAsync($"CommunityPostComment/searchByPostId/{id}");
        var postComments = await responseMessage.Content.ReadFromJsonAsync<IEnumerable<CommunityPostCommentModel>>();

        return Ok(postComments);
    }

    [HttpPut]
    public async Task<IActionResult> Update(CommunityPostCommentModel model)
    {
        await _httpClient.PutAsync("CommunityPostComment", JsonContent.Create(model));

        return Ok();
    }

    [HttpPost]
    public async Task<IActionResult> Create(CommunityPostCommentModel model)
    {
        var responseMessage = await _httpClient.PostAsync("CommunityPostComment", JsonContent.Create(model));
        var postComment = await responseMessage.Content.ReadFromJsonAsync<CommunityPostCommentModel>();

        return Ok(postComment);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _httpClient.DeletAsync($"CommunityPostComment/{id}");

        return Ok();
    }
}
