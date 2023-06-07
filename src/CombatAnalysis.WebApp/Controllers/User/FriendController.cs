using CombatAnalysis.WebApp.Consts;
using CombatAnalysis.WebApp.Interfaces;
using CombatAnalysis.WebApp.Models.User;
using Microsoft.AspNetCore.Mvc;

namespace CombatAnalysis.WebApp.Controllers.User;

[Route("api/v1/[controller]")]
[ApiController]
public class FriendController : ControllerBase
{
    private readonly IHttpClientHelper _httpClient;

    public FriendController(IHttpClientHelper httpClient)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = Port.UserApi;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(string id)
    {
        var responseMessage = await _httpClient.GetAsync($"Friend/{id}");
        var friend = await responseMessage.Content.ReadFromJsonAsync<FriendModel>();

        return Ok(friend);
    }

    [HttpGet("searchByForWhomId/{id}")]
    public async Task<IActionResult> SearchByForWhomId(string id)
    {
        var responseMessage = await _httpClient.GetAsync($"Friend/searchByForWhomId/{id}");
        var friends = await responseMessage.Content.ReadFromJsonAsync<IEnumerable<FriendModel>>();

        return Ok(friends);
    }

    [HttpGet("searchByUserId/{id}")]
    public async Task<IActionResult> SearchByUserId(string id)
    {
        var responseMessage = await _httpClient.GetAsync($"Friend/searchByUserId/{id}");
        var friends = await responseMessage.Content.ReadFromJsonAsync<IEnumerable<FriendModel>>();

        return Ok(friends);
    }

    [HttpPost]
    public async Task<IActionResult> Create(FriendModel model)
    {
        var responseMessage = await _httpClient.PostAsync("Friend", JsonContent.Create(model));
        var friend = await responseMessage.Content.ReadFromJsonAsync<FriendModel>();

        return Ok(friend);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _httpClient.DeletAsync($"Friend/{id}");

        return Ok();
    }
}
