using CombatAnalysis.WebApp.Consts;
using CombatAnalysis.WebApp.Interfaces;
using CombatAnalysis.WebApp.Models.Community;
using Microsoft.AspNetCore.Mvc;

namespace CombatAnalysis.WebApp.Controllers.Community;

[Route("api/v1/[controller]")]
[ApiController]
public class CommunityPostLikeController : ControllerBase
{
    private readonly IHttpClientHelper _httpClient;

    public CommunityPostLikeController(IHttpClientHelper httpClient)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = Port.UserApi;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var responseMessage = await _httpClient.GetAsync($"CommunityPostLike/{id}");
        var postLike = await responseMessage.Content.ReadFromJsonAsync<CommunityPostLikeModel>();

        return Ok(postLike);
    }

    [HttpGet("searchByPostId/{id}")]
    public async Task<IActionResult> SearchByPostId(int id)
    {
        var responseMessage = await _httpClient.GetAsync($"CommunityPostLike/searchByPostId/{id}");
        var postLikes = await responseMessage.Content.ReadFromJsonAsync<IEnumerable<CommunityPostLikeModel>>();

        return Ok(postLikes);
    }

    [HttpPut]
    public async Task<IActionResult> Update(CommunityPostLikeModel model)
    {
        await _httpClient.PutAsync("CommunityPostLike", JsonContent.Create(model));

        return Ok();
    }

    [HttpPost]
    public async Task<IActionResult> Create(CommunityPostLikeModel model)
    {
        var responseMessage = await _httpClient.PostAsync("CommunityPostLike", JsonContent.Create(model));
        var postLike = await responseMessage.Content.ReadFromJsonAsync<CommunityPostLikeModel>();

        return Ok(postLike);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _httpClient.DeletAsync($"CommunityPostLike/{id}");

        return Ok();
    }
}
