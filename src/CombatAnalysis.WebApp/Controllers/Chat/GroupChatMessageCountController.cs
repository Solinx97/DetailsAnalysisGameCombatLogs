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
public class GroupChatMessageCountController : ControllerBase
{
    private readonly IHttpClientHelper _httpClient;

    public GroupChatMessageCountController(IHttpClientHelper httpClient)
    {
        _httpClient = httpClient;
    }

    [HttpGet("find")]
    public async Task<IActionResult> Find(int chatId, string userId)
    {
        if (!Request.Cookies.TryGetValue(AuthenticationTokenType.AccessToken.ToString(), out var accessToken))
        {
            return Unauthorized();
        }

        var responseMessage = await _httpClient.GetAsync("GroupChatMessageCount", accessToken, Port.ChatApi);
        if (responseMessage.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            return Unauthorized();
        }
        else if (!responseMessage.IsSuccessStatusCode)
        {
            return BadRequest();
        }

        var groupChatMessagesCount = await responseMessage.Content.ReadFromJsonAsync<IEnumerable<GroupChatMessageCountModel>>();
        var myGroupChatMessageCount = groupChatMessagesCount.Where(x => x.GroupChatId == chatId && x.GroupChatUserId == userId).FirstOrDefault();

        return Ok(myGroupChatMessageCount);
    }

    [HttpPost]
    public async Task<IActionResult> Create(GroupChatMessageCountModel message)
    {
        if (!Request.Cookies.TryGetValue(AuthenticationTokenType.AccessToken.ToString(), out var accessToken))
        {
            return Unauthorized();
        }

        var responseMessage = await _httpClient.PostAsync("GroupChatMessageCount", JsonContent.Create(message), accessToken, Port.ChatApi);
        if (responseMessage.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            return Unauthorized();
        }
        else if (responseMessage.IsSuccessStatusCode)
        {
            var personalChatMessage = await responseMessage.Content.ReadFromJsonAsync<GroupChatMessageCountModel>();
            return Ok(personalChatMessage);
        }

        return BadRequest();
    }

    [HttpPut]
    public async Task<IActionResult> Update(GroupChatMessageCountModel message)
    {
        if (!Request.Cookies.TryGetValue(AuthenticationTokenType.AccessToken.ToString(), out var accessToken))
        {
            return Unauthorized();
        }

        var responseMessage = await _httpClient.PutAsync("GroupChatMessageCount", JsonContent.Create(message), accessToken, Port.ChatApi);
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

    [HttpDelete("{id:int:min(1)}")]
    public async Task<IActionResult> Delete(int id)
    {
        if (!Request.Cookies.TryGetValue(AuthenticationTokenType.AccessToken.ToString(), out var accessToken))
        {
            return Unauthorized();
        }

        var responseMessage = await _httpClient.DeletAsync($"GroupChatMessageCount/{id}", accessToken, Port.ChatApi);
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
