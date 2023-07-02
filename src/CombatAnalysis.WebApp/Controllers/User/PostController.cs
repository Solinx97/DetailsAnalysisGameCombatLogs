using CombatAnalysis.WebApp.Consts;
using CombatAnalysis.WebApp.Interfaces;
using CombatAnalysis.WebApp.Models.User;
using Microsoft.AspNetCore.Mvc;

namespace CombatAnalysis.WebApp.Controllers.User;

[Route("api/v1/[controller]")]
[ApiController]
public class PostController : ControllerBase
{
    private readonly IHttpClientHelper _httpClient;

    public PostController(IHttpClientHelper httpClient)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = Port.UserApi;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var responseMessage = await _httpClient.GetAsync($"Post/{id}");
        var post = await responseMessage.Content.ReadFromJsonAsync<PostModel>();

        return Ok(post);
    }

    [HttpGet("searchByOwnerId/{id}")]
    public async Task<IActionResult> SearchByOwnerId(string id)
    {
        var responseMessage = await _httpClient.GetAsync($"Post/searchByOwnerId/{id}");
        var posts = await responseMessage.Content.ReadFromJsonAsync<IEnumerable<PostModel>>();

        return Ok(posts);
    }

    [HttpPut]
    public async Task<IActionResult> Update(PostModel model)
    {
        await _httpClient.PutAsync("Post", JsonContent.Create(model));

        return Ok();
    }

    [HttpPost]
    public async Task<IActionResult> Create(PostModel model)
    {
        var responseMessage = await _httpClient.PostAsync("Post", JsonContent.Create(model));
        var post = await responseMessage.Content.ReadFromJsonAsync<PostModel>();

        return Ok(post);
    }
}
