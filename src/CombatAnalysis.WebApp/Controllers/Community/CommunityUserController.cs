using CombatAnalysis.WebApp.Consts;
using CombatAnalysis.WebApp.Extensions;
using CombatAnalysis.WebApp.Interfaces;
using CombatAnalysis.WebApp.Models.Community;
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

    [HttpGet("{id}")]
    public async Task<IActionResult> GetByIdUserId(string id)
    {
        var responseMessage = await _httpClient.GetAsync($"CommunityUser/findByUserId/{id}", Port.ChatApi);
        if (responseMessage.StatusCode == System.Net.HttpStatusCode.OK)
        {
            var myGroupChatUser = await responseMessage.Content.ReadFromJsonAsync<IEnumerable<CommunityUserModel>>();

            return Ok(myGroupChatUser);
        }

        return BadRequest();
    }

    [HttpGet("findByChatId/{id:int:min(1)}")]
    public async Task<IActionResult> Find(int id)
    {
        var responseMessage = await _httpClient.GetAsync($"CommunityUser/findByChatId/{id}", Port.ChatApi);
        if (responseMessage.StatusCode == System.Net.HttpStatusCode.OK)
        {
            var groupChatUsers = await responseMessage.Content.ReadFromJsonAsync<IEnumerable<CommunityUserModel>>();

            return Ok(groupChatUsers);
        }

        return BadRequest();
    }

    [HttpPost]
    public async Task<IActionResult> Create(CommunityUserModel user)
    {
        var responseMessage = await _httpClient.PostAsync("CommunityUser", JsonContent.Create(user), Port.ChatApi);
        if (responseMessage.StatusCode == System.Net.HttpStatusCode.OK)
        {
            return Ok();
        }

        return BadRequest();
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
