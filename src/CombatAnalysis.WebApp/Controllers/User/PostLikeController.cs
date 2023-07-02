using CombatAnalysis.WebApp.Consts;
using CombatAnalysis.WebApp.Interfaces;
using CombatAnalysis.WebApp.Models.User;
using Microsoft.AspNetCore.Mvc;

namespace CombatAnalysis.WebApp.Controllers.User;

[Route("api/v1/[controller]")]
[ApiController]
public class PostLikeController : ControllerBase
{
    private readonly IHttpClientHelper _httpClient;

    public PostLikeController(IHttpClientHelper httpClient)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = Port.UserApi;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var responseMessage = await _httpClient.GetAsync($"PostLike/{id}");
        var postLike = await responseMessage.Content.ReadFromJsonAsync<PostLikeModel>();

        return Ok(postLike);
    }

    [HttpGet("searchByPostId/{id}")]
    public async Task<IActionResult> SearchByPostId(int id)
    {
        var responseMessage = await _httpClient.GetAsync($"PostLike/searchByPostId/{id}");
        var postLikes = await responseMessage.Content.ReadFromJsonAsync<IEnumerable<PostLikeModel>>();

        return Ok(postLikes);
    }

    [HttpPut]
    public async Task<IActionResult> Update(PostLikeModel model)
    {
        await _httpClient.PutAsync("PostLike", JsonContent.Create(model));

        return Ok();
    }

    [HttpPost]
    public async Task<IActionResult> Create(PostLikeModel model)
    {
        var responseMessage = await _httpClient.PostAsync("PostLike", JsonContent.Create(model));
        var postLike = await responseMessage.Content.ReadFromJsonAsync<PostLikeModel>();

        return Ok(postLike);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _httpClient.DeletAsync($"PostLike/{id}");

        return Ok();
    }
}
