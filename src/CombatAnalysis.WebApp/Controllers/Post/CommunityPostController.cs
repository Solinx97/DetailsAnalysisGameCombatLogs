using CombatAnalysis.WebApp.Consts;
using CombatAnalysis.WebApp.Extensions;
using CombatAnalysis.WebApp.Interfaces;
using CombatAnalysis.WebApp.Models.Post;
using Microsoft.AspNetCore.Mvc;

namespace CombatAnalysis.WebApp.Controllers.Post;

[Route("api/v1/[controller]")]
[ApiController]
public class CommunityPostController : ControllerBase
{
    private readonly IHttpClientHelper _httpClient;

    public CommunityPostController(IHttpClientHelper httpClient)
    {
        _httpClient = httpClient;
    }

    [HttpGet("{id:int:min(1)}")]
    public async Task<IActionResult> GetById(int id)
    {
        var responseMessage = await _httpClient.GetAsync($"CommunityPost/{id}", Port.ChatApi);
        var post = await responseMessage.Content.ReadFromJsonAsync<CommunityPostModel>();

        return Ok(post);
    }

    [HttpGet("searchByCommunityId/{id:int:min(1)}")]
    public async Task<IActionResult> SearchByCommunityId(int id)
    {
        var responseMessage = await _httpClient.GetAsync($"CommunityPost/searchByCommunityId/{id}", Port.ChatApi);
        var posts = await responseMessage.Content.ReadFromJsonAsync<IEnumerable<CommunityPostModel>>();

        return Ok(posts);
    }

    [HttpPut]
    public async Task<IActionResult> Update(CommunityPostModel model)
    {
        await _httpClient.PutAsync("CommunityPost", JsonContent.Create(model), Port.ChatApi);

        return Ok();
    }

    [HttpPost]
    public async Task<IActionResult> Create(CommunityPostModel model)
    {
        var responseMessage = await _httpClient.PostAsync("CommunityPost", JsonContent.Create(model), Port.ChatApi);
        var post = await responseMessage.Content.ReadFromJsonAsync<CommunityPostModel>();

        return Ok(post);
    }

    [HttpDelete("{id:int:min(1)}")]
    public async Task<IActionResult> Delete(int id)
    {
        var responseMessage = await _httpClient.DeletAsync($"CommunityPost/{id}", Port.ChatApi);
        if (responseMessage.StatusCode == System.Net.HttpStatusCode.OK)
        {
            return Ok();
        }

        return BadRequest();
    }
}
