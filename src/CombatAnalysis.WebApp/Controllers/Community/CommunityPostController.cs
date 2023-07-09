using CombatAnalysis.WebApp.Consts;
using CombatAnalysis.WebApp.Interfaces;
using CombatAnalysis.WebApp.Models.Community;
using Microsoft.AspNetCore.Mvc;

namespace CombatAnalysis.WebApp.Controllers.Community;

[Route("api/v1/[controller]")]
[ApiController]
public class CommunityPostController : ControllerBase
{
    private readonly IHttpClientHelper _httpClient;

    public CommunityPostController(IHttpClientHelper httpClient)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = Port.UserApi;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var responseMessage = await _httpClient.GetAsync($"CommunityPost/{id}");
        var post = await responseMessage.Content.ReadFromJsonAsync<CommunityPostModel>();

        return Ok(post);
    }

    [HttpGet("searchByOwnerId/{id}")]
    public async Task<IActionResult> SearchByOwnerId(string id)
    {
        var responseMessage = await _httpClient.GetAsync($"CommunityPost/searchByOwnerId/{id}");
        var posts = await responseMessage.Content.ReadFromJsonAsync<IEnumerable<CommunityPostModel>>();

        return Ok(posts);
    }

    [HttpPut]
    public async Task<IActionResult> Update(CommunityPostModel model)
    {
        await _httpClient.PutAsync("CommunityPost", JsonContent.Create(model));

        return Ok();
    }

    [HttpPost]
    public async Task<IActionResult> Create(CommunityPostModel model)
    {
        var responseMessage = await _httpClient.PostAsync("CommunityPost", JsonContent.Create(model));
        var post = await responseMessage.Content.ReadFromJsonAsync<CommunityPostModel>();

        return Ok(post);
    }
}
