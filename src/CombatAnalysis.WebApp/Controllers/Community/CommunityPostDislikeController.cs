using CombatAnalysis.WebApp.Consts;
using CombatAnalysis.WebApp.Interfaces;
using CombatAnalysis.WebApp.Models.Community;
using Microsoft.AspNetCore.Mvc;

namespace CombatAnalysis.WebApp.Controllers.Community;

[Route("api/v1/[controller]")]
[ApiController]
public class CommunityPostDislikeController : ControllerBase
{
    private readonly IHttpClientHelper _httpClient;

    public CommunityPostDislikeController(IHttpClientHelper httpClient)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = Port.UserApi;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var responseMessage = await _httpClient.GetAsync($"CommunityPostDislike/{id}");
        var postLike = await responseMessage.Content.ReadFromJsonAsync<CommunityPostDislikeModel>();

        return Ok(postLike);
    }

    [HttpGet("searchByPostId/{id}")]
    public async Task<IActionResult> SearchByPostId(int id)
    {
        var responseMessage = await _httpClient.GetAsync($"CommunityPostDislike/searchByPostId/{id}");
        var postLikes = await responseMessage.Content.ReadFromJsonAsync<IEnumerable<CommunityPostDislikeModel>>();

        return Ok(postLikes);
    }

    [HttpPut]
    public async Task<IActionResult> Update(CommunityPostLikeModel model)
    {
        await _httpClient.PutAsync("CommunityPostDislike", JsonContent.Create(model));

        return Ok();
    }

    [HttpPost]
    public async Task<IActionResult> Create(CommunityPostLikeModel model)
    {
        var responseMessage = await _httpClient.PostAsync("CommunityPostDislike", JsonContent.Create(model));
        var postLike = await responseMessage.Content.ReadFromJsonAsync<CommunityPostDislikeModel>();

        return Ok(postLike);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _httpClient.DeletAsync($"CommunityPostDislike/{id}");

        return Ok();
    }
}
