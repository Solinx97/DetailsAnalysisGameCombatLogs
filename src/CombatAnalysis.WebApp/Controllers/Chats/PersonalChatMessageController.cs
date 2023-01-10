using CombatAnalysis.WebApp.Consts;
using CombatAnalysis.WebApp.Interfaces;
using CombatAnalysis.WebApp.Models.Chats;
using Microsoft.AspNetCore.Mvc;

namespace CombatAnalysis.WebApp.Controllers.Chats;

[Route("api/v1/[controller]")]
[ApiController]
public class PersonalChatMessageController : ControllerBase
{
    private readonly IHttpClientHelper _httpClient;

    public PersonalChatMessageController(IHttpClientHelper httpClient)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = Port.ChatApi;
    }

    [HttpGet("findByChatId/{chatId:int:min(1)}")]
    public async Task<IActionResult> Find(int chatId)
    {
        var responseMessage = await _httpClient.GetAsync($"PersonalChatMessage/findByChatId/{chatId}");
        var personalChatMessages = await responseMessage.Content.ReadFromJsonAsync<IEnumerable<PersonalChatMessageModel>>();

        return Ok(personalChatMessages);
    }

    [HttpPost]
    public async Task<IActionResult> Create(PersonalChatMessageModel message)
    {
        var responseMessage = await _httpClient.PostAsync("PersonalChatMessage", JsonContent.Create(message));
        if (responseMessage.StatusCode == System.Net.HttpStatusCode.OK)
        {
            return Ok();
        }

        return BadRequest();
    }
}
