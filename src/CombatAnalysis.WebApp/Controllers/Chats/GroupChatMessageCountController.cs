using CombatAnalysis.WebApp.Consts;
using CombatAnalysis.WebApp.Extensions;
using CombatAnalysis.WebApp.Interfaces;
using CombatAnalysis.WebApp.Models.Chats;
using Microsoft.AspNetCore.Mvc;

namespace CombatAnalysis.WebApp.Controllers.Chats;

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
        if (!HttpContext.Request.Cookies.TryGetValue("refreshToken", out var refreshToken))
        {
            return Unauthorized();
        }

        var responseMessage = await _httpClient.GetAsync("GroupChatMessageCount", refreshToken, Port.ChatApi);
        if (responseMessage.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            return Unauthorized();
        }
        else if (!responseMessage.IsSuccessStatusCode)
        {
            return BadRequest();
        }

        var groupChatMessagesCount = await responseMessage.Content.ReadFromJsonAsync<IEnumerable<GroupChatMessageCountModel>>();
        var myGroupChatMessageCount = groupChatMessagesCount.Where(x => x.GroupChatId == chatId && x.UserId == userId).FirstOrDefault();

        return Ok(myGroupChatMessageCount);
    }

    [HttpPost]
    public async Task<IActionResult> Create(GroupChatMessageCountModel message)
    {
        if (!HttpContext.Request.Cookies.TryGetValue("refreshToken", out var refreshToken))
        {
            return Unauthorized();
        }

        var responseMessage = await _httpClient.PostAsync("GroupChatMessageCount", JsonContent.Create(message), refreshToken, Port.ChatApi);
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
        if (!HttpContext.Request.Cookies.TryGetValue("refreshToken", out var refreshToken))
        {
            return Unauthorized();
        }

        var responseMessage = await _httpClient.PutAsync("GroupChatMessageCount", JsonContent.Create(message), refreshToken, Port.ChatApi);
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
        if (!HttpContext.Request.Cookies.TryGetValue("refreshToken", out var refreshToken))
        {
            return Unauthorized();
        }

        var responseMessage = await _httpClient.DeletAsync($"GroupChatMessageCount/{id}", refreshToken, Port.ChatApi);
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
