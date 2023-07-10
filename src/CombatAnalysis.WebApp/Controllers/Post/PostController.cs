using CombatAnalysis.WebApp.Consts;
using CombatAnalysis.WebApp.Extensions;
using CombatAnalysis.WebApp.Interfaces;
using CombatAnalysis.WebApp.Models.Post;
using Microsoft.AspNetCore.Mvc;

namespace CombatAnalysis.WebApp.Controllers.Post;

[Route("api/v1/[controller]")]
[ApiController]
public class PostController : ControllerBase
{
    private readonly IHttpClientHelper _httpClient;

    public PostController(IHttpClientHelper httpClient)
    {
        _httpClient = httpClient;
    }

    [HttpGet("{id:int:min(1)}")]
    public async Task<IActionResult> GetById(int id)
    {
        var responseMessage = await _httpClient.GetAsync($"Post/{id}", Port.ChatApi);
        var post = await responseMessage.Content.ReadFromJsonAsync<PostModel>();

        return Ok(post);
    }

    [HttpGet("searchByOwnerId/{id}")]
    public async Task<IActionResult> SearchByOwnerId(string id)
    {
        var responseMessage = await _httpClient.GetAsync($"Post/searchByOwnerId/{id}", Port.ChatApi);
        var posts = await responseMessage.Content.ReadFromJsonAsync<IEnumerable<PostModel>>();

        return Ok(posts);
    }

    [HttpPut]
    public async Task<IActionResult> Update(PostModel model)
    {
        await _httpClient.PutAsync("Post", JsonContent.Create(model), Port.ChatApi);

        return Ok();
    }

    [HttpPost]
    public async Task<IActionResult> Create(PostModel model)
    {
        var responseMessage = await _httpClient.PostAsync("Post", JsonContent.Create(model), Port.ChatApi);
        var post = await responseMessage.Content.ReadFromJsonAsync<PostModel>();

        return Ok(post);
    }

    [HttpDelete("{id:int:min(1)}")]
    public async Task<IActionResult> Delete(int id)
    {
        var responseMessage = await _httpClient.DeletAsync($"Post/{id}", Port.ChatApi);
        if (responseMessage.StatusCode == System.Net.HttpStatusCode.OK)
        {
            return Ok();
        }

        return BadRequest();
    }
}
