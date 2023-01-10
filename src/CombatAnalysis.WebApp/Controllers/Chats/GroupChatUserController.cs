using CombatAnalysis.WebApp.Consts;
using CombatAnalysis.WebApp.Interfaces;
using CombatAnalysis.WebApp.Models.Chats;
using Microsoft.AspNetCore.Mvc;

namespace CombatAnalysis.WebApp.Controllers.Chats;

[Route("api/v1/[controller]")]
[ApiController]
public class GroupChatUserController : ControllerBase
{
    private readonly IHttpClientHelper _httpClient;

    public GroupChatUserController(IHttpClientHelper httpClient)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = Port.ChatApi;
    }

    [HttpGet("{userId}")]
    public async Task<IActionResult> GetByIdUserId(string userId)
    {
        var responseMessage = await _httpClient.GetAsync($"GroupChatUser/findByUserId/{userId}");
        var myGroupChatUser = await responseMessage.Content.ReadFromJsonAsync<IEnumerable<GroupChatUserModel>>();

        return Ok(myGroupChatUser);
    }
}
