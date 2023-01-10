using CombatAnalysis.WebApp.Consts;
using CombatAnalysis.WebApp.Interfaces;
using CombatAnalysis.WebApp.Models.Chats;
using Microsoft.AspNetCore.Mvc;

namespace CombatAnalysis.WebApp.Controllers.Chats;

[Route("api/v1/[controller]")]
[ApiController]
public class GroupChatMessageController : ControllerBase
{
    private readonly IHttpClientHelper _httpClient;

    public GroupChatMessageController(IHttpClientHelper httpClient)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = Port.ChatApi;
    }

    [HttpGet("findByChatId/{chatId:int:min(1)}")]
    public async Task<IActionResult> Find(int chatId)
    {
        var responseMessage = await _httpClient.GetAsync($"GroupChatMessage/findByChatId/{chatId}");
        var groupChatMessages = await responseMessage.Content.ReadFromJsonAsync<IEnumerable<GroupChatMessageModel>>();

        return Ok(groupChatMessages);
    }

    [HttpPost]
    public async Task<IActionResult> Create(GroupChatMessageModel message)
    {
        var responseMessage = await _httpClient.PostAsync("GroupChatMessage", JsonContent.Create(message));
        if (responseMessage.StatusCode == System.Net.HttpStatusCode.OK)
        {
            return Ok();
        }

        return BadRequest();
    }
}
