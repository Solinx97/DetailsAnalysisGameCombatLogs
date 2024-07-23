using CombatAnalysis.WebApp.Attributes;
using CombatAnalysis.WebApp.Consts;
using CombatAnalysis.WebApp.Enums;
using CombatAnalysis.WebApp.Extensions;
using CombatAnalysis.WebApp.Interfaces;
using CombatAnalysis.WebApp.Models.Chat;
using Microsoft.AspNetCore.Mvc;

namespace CombatAnalysis.WebApp.Controllers.Chat;

[RequireAccessToken]
[Route("api/v1/[controller]")]
[ApiController]
public class GroupChatMessageController : ControllerBase
{
    private readonly IHttpClientHelper _httpClient;

    public GroupChatMessageController(IHttpClientHelper httpClient)
    {
        _httpClient = httpClient;
    }

    [HttpGet("findByChatId/{chatId:int:min(1)}")]
    public async Task<IActionResult> Find(int chatId)
    {
        if (!Request.Cookies.TryGetValue(AuthenticationCookie.AccessToken.ToString(), out var accessToken))
        {
            return Unauthorized();
        }

        var responseMessage = await _httpClient.GetAsync($"GroupChatMessage/findByChatId/{chatId}", accessToken, Port.ChatApi);
        if (responseMessage.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            return Unauthorized();
        }
        else if (responseMessage.IsSuccessStatusCode)
        {
            var groupChatMessages = await responseMessage.Content.ReadFromJsonAsync<IEnumerable<GroupChatMessageModel>>();

            return Ok(groupChatMessages);
        }

        return BadRequest();
    }

    [HttpPost]
    public async Task<IActionResult> Create(GroupChatMessageModel message)
    {
        if (!Request.Cookies.TryGetValue(AuthenticationCookie.AccessToken.ToString(), out var accessToken))
        {
            return Unauthorized();
        }

        var responseMessage = await _httpClient.PostAsync("GroupChatMessage", JsonContent.Create(message), accessToken, Port.ChatApi);
        if (responseMessage.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            return Unauthorized();
        }
        else if (responseMessage.IsSuccessStatusCode)
        {
            var groupChatMessage = await responseMessage.Content.ReadFromJsonAsync<GroupChatMessageModel>();
            return Ok(groupChatMessage);
        }

        return BadRequest();
    }

    [HttpPut]
    public async Task<IActionResult> Update(GroupChatMessageModel message)
    {
        if (!Request.Cookies.TryGetValue(AuthenticationCookie.AccessToken.ToString(), out var accessToken))
        {
            return Unauthorized();
        }

        var responseMessage = await _httpClient.PutAsync("GroupChatMessage", JsonContent.Create(message), accessToken, Port.ChatApi);
        if (responseMessage.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            return Unauthorized();
        }
        else if (responseMessage.IsSuccessStatusCode)
        {
            return Ok();
        }

        return BadRequest();
    }

    [HttpDelete("{messageId:int:min(1)}")]
    public async Task<IActionResult> Delete(int messageId)
    {
        if (!Request.Cookies.TryGetValue(AuthenticationCookie.AccessToken.ToString(), out var accessToken))
        {
            return Unauthorized();
        }

        var responseMessage = await _httpClient.DeletAsync($"GroupChatMessage/{messageId}", accessToken, Port.ChatApi);
        if (responseMessage.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            return Unauthorized();
        }
        else if (responseMessage.IsSuccessStatusCode)
        {
            return Ok();
        }

        return BadRequest();
    }
}
