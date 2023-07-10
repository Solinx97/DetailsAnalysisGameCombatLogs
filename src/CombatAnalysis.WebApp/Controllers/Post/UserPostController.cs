using CombatAnalysis.WebApp.Consts;
using CombatAnalysis.WebApp.Extensions;
using CombatAnalysis.WebApp.Interfaces;
using CombatAnalysis.WebApp.Models.Post;
using Microsoft.AspNetCore.Mvc;

namespace CombatAnalysis.WebApp.Controllers.Post;

[Route("api/v1/[controller]")]
[ApiController]
public class UserPostController : ControllerBase
{
    private readonly IHttpClientHelper _httpClient;

    public UserPostController(IHttpClientHelper httpClient)
    {
        _httpClient = httpClient;
    }

    [HttpGet("{id:int:min(1)}")]
    public async Task<IActionResult> GetById(int id)
    {
        var responseMessage = await _httpClient.GetAsync($"UserPost/{id}", Port.ChatApi);
        var post = await responseMessage.Content.ReadFromJsonAsync<UserPostModel>();

        return Ok(post);
    }

    [HttpGet("searchByUserId/{id}")]
    public async Task<IActionResult> SearchByCommunityId(string id)
    {
        var responseMessage = await _httpClient.GetAsync($"UserPost/searchByUserId/{id}", Port.ChatApi);
        var posts = await responseMessage.Content.ReadFromJsonAsync<IEnumerable<UserPostModel>>();

        return Ok(posts);
    }

    [HttpPut]
    public async Task<IActionResult> Update(UserPostModel model)
    {
        await _httpClient.PutAsync("UserPost", JsonContent.Create(model), Port.ChatApi);

        return Ok();
    }

    [HttpPost]
    public async Task<IActionResult> Create(UserPostModel model)
    {
        var responseMessage = await _httpClient.PostAsync("UserPost", JsonContent.Create(model), Port.ChatApi);
        var post = await responseMessage.Content.ReadFromJsonAsync<UserPostModel>();

        return Ok(post);
    }

    [HttpDelete("{id:int:min(1)}")]
    public async Task<IActionResult> Delete(int id)
    {
        var responseMessage = await _httpClient.DeletAsync($"UserPost/{id}", Port.ChatApi);
        if (responseMessage.StatusCode == System.Net.HttpStatusCode.OK)
        {
            return Ok();
        }

        return BadRequest();
    }
}
