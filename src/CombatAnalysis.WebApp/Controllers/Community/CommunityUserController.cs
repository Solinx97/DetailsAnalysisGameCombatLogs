using CombatAnalysis.WebApp.Consts;
using CombatAnalysis.WebApp.Extensions;
using CombatAnalysis.WebApp.Interfaces;
using CombatAnalysis.WebApp.Models.Community;
using CombatAnalysis.WebApp.Models.Post;
using Microsoft.AspNetCore.Mvc;

namespace CombatAnalysis.WebApp.Controllers.Community;

[Route("api/v1/[controller]")]
[ApiController]
public class CommunityUserController : ControllerBase
{
    private readonly IHttpClientHelper _httpClient;

    public CommunityUserController(IHttpClientHelper httpClient)
    {
        _httpClient = httpClient;
    }

    [HttpGet("{id:int:min(1)}")]
    public async Task<IActionResult> GetById(int id)
    {
        var responseMessage = await _httpClient.GetAsync($"CommunityUser/{id}", Port.ChatApi);
        var post = await responseMessage.Content.ReadFromJsonAsync<CommunityUserModel>();

        return Ok(post);
    }

    [HttpGet("searchByCommunityId/{id:int:min(1)}")]
    public async Task<IActionResult> SearchByCommunityId(int id)
    {
        var responseMessage = await _httpClient.GetAsync($"CommunityUser/searchByCommunityId/{id}", Port.ChatApi);
        var posts = await responseMessage.Content.ReadFromJsonAsync<IEnumerable<CommunityUserModel>>();

        return Ok(posts);
    }


    [HttpGet("searchByUserId/{id}")]
    public async Task<IActionResult> SearchByUserId(string id)
    {
        var responseMessage = await _httpClient.GetAsync($"CommunityUser/searchByUserId/{id}", Port.ChatApi);
        var posts = await responseMessage.Content.ReadFromJsonAsync<IEnumerable<CommunityUserModel>>();

        return Ok(posts);
    }

    [HttpPut]
    public async Task<IActionResult> Update(CommunityUserModel model)
    {
        await _httpClient.PutAsync("CommunityUser", JsonContent.Create(model), Port.ChatApi);

        return Ok();
    }

    [HttpPost]
    public async Task<IActionResult> Create(CommunityUserModel model)
    {
        var responseMessage = await _httpClient.PostAsync("CommunityUser", JsonContent.Create(model), Port.ChatApi);
        var post = await responseMessage.Content.ReadFromJsonAsync<CommunityUserModel>();

        return Ok(post);
    }

    [HttpDelete("{id:int:min(1)}")]
    public async Task<IActionResult> Delete(int id)
    {
        var responseMessage = await _httpClient.DeletAsync($"CommunityUser/{id}", Port.ChatApi);
        if (responseMessage.StatusCode == System.Net.HttpStatusCode.OK)
        {
            return Ok();
        }

        return BadRequest();
    }
}
