using CombatAnalysis.WebApp.Consts;
using CombatAnalysis.WebApp.Extensions;
using CombatAnalysis.WebApp.Interfaces;
using CombatAnalysis.WebApp.Models.Post;
using Microsoft.AspNetCore.Mvc;

namespace CombatAnalysis.WebApp.Controllers.Post;

[Route("api/v1/[controller]")]
[ApiController]
public class PostDislikeController : ControllerBase
{
    private readonly IHttpClientHelper _httpClient;

    public PostDislikeController(IHttpClientHelper httpClient)
    {
        _httpClient = httpClient;
    }

    [HttpGet("{id:int:min(1)}")]
    public async Task<IActionResult> GetById(int id)
    {
        var responseMessage = await _httpClient.GetAsync($"PostDislike/{id}", Port.ChatApi);
        var postLike = await responseMessage.Content.ReadFromJsonAsync<PostDislikeModel>();

        return Ok(postLike);
    }

    [HttpGet("searchByPostId/{id:int:min(1)}")]
    public async Task<IActionResult> SearchByPostId(int id)
    {
        var responseMessage = await _httpClient.GetAsync($"PostDislike/searchByPostId/{id}", Port.ChatApi);
        var postLikes = await responseMessage.Content.ReadFromJsonAsync<IEnumerable<PostDislikeModel>>();

        return Ok(postLikes);
    }

    [HttpPut]
    public async Task<IActionResult> Update(PostLikeModel model)
    {
        await _httpClient.PutAsync("PostDislike", JsonContent.Create(model), Port.ChatApi);

        return Ok();
    }

    [HttpPost]
    public async Task<IActionResult> Create(PostLikeModel model)
    {
        var responseMessage = await _httpClient.PostAsync("PostDislike", JsonContent.Create(model), Port.ChatApi);
        var postLike = await responseMessage.Content.ReadFromJsonAsync<PostDislikeModel>();

        return Ok(postLike);
    }

    [HttpDelete("{id:int:min(1)}")]
    public async Task<IActionResult> Delete(int id)
    {
        var responseMessage = await _httpClient.DeletAsync($"PostDislike/{id}", Port.ChatApi);
        if (responseMessage.StatusCode == System.Net.HttpStatusCode.OK)
        {
            return Ok();
        }

        return BadRequest();
    }
}
