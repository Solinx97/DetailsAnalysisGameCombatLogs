using CombatAnalysis.WebApp.Consts;
using CombatAnalysis.WebApp.Interfaces;
using CombatAnalysis.WebApp.Models.User;
using Microsoft.AspNetCore.Mvc;

namespace CombatAnalysis.WebApp.Controllers.User;

[Route("api/v1/[controller]")]
[ApiController]
public class PostDislikeController : ControllerBase
{
    private readonly IHttpClientHelper _httpClient;

    public PostDislikeController(IHttpClientHelper httpClient)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = Port.UserApi;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var responseMessage = await _httpClient.GetAsync($"PostDislike/{id}");
        var postLike = await responseMessage.Content.ReadFromJsonAsync<PostDislikeModel>();

        return Ok(postLike);
    }

    [HttpGet("searchByPostId/{id}")]
    public async Task<IActionResult> SearchByPostId(int id)
    {
        var responseMessage = await _httpClient.GetAsync($"PostDislike/searchByPostId/{id}");
        var postLikes = await responseMessage.Content.ReadFromJsonAsync<IEnumerable<PostDislikeModel>>();

        return Ok(postLikes);
    }

    [HttpPut]
    public async Task<IActionResult> Update(PostLikeModel model)
    {
        await _httpClient.PutAsync("PostDislike", JsonContent.Create(model));

        return Ok();
    }

    [HttpPost]
    public async Task<IActionResult> Create(PostLikeModel model)
    {
        var responseMessage = await _httpClient.PostAsync("PostDislike", JsonContent.Create(model));
        var postLike = await responseMessage.Content.ReadFromJsonAsync<PostDislikeModel>();

        return Ok(postLike);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _httpClient.DeletAsync($"PostDislike/{id}");

        return Ok();
    }
}
