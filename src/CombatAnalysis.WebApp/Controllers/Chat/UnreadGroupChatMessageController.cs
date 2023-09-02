using CombatAnalysis.WebApp.Consts;
using CombatAnalysis.WebApp.Extensions;
using CombatAnalysis.WebApp.Interfaces;
using CombatAnalysis.WebApp.Models.Chat;
using Microsoft.AspNetCore.Mvc;

namespace CombatAnalysis.WebApp.Controllers.Chat;

[Route("api/v1/[controller]")]
[ApiController]
public class UnreadGroupChatMessageController : ControllerBase
{
    private readonly IHttpClientHelper _httpClient;

    public UnreadGroupChatMessageController(IHttpClientHelper httpClient)
    {
        _httpClient = httpClient;
    }

    [HttpGet("find")]
    public async Task<IActionResult> Find(int messageId, string groupChatUserId)
    {
        if (!HttpContext.Request.Cookies.TryGetValue("refreshToken", out var refreshToken))
        {
            return Unauthorized();
        }

        var responseMessage = await _httpClient.GetAsync("UnreadGroupChatMessage", refreshToken, Port.ChatApi);
        if (responseMessage.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            return Unauthorized();
        }
        else if (!responseMessage.IsSuccessStatusCode)
        {
            return BadRequest();
        }

        var groupChatMessagesCount = await responseMessage.Content.ReadFromJsonAsync<IEnumerable<UnreadGroupChatMessageModel>>();
        var myGroupChatMessagesCount = groupChatMessagesCount.Where(x => x.GroupChatMessageId == messageId && x.GroupChatUserId == groupChatUserId).FirstOrDefault();

        return Ok(myGroupChatMessagesCount);
    }

    [HttpGet("findByMessageId/{id}")]
    public async Task<IActionResult> FindByMessageId(int id)
    {
        if (!HttpContext.Request.Cookies.TryGetValue("refreshToken", out var refreshToken))
        {
            return Unauthorized();
        }

        var responseMessage = await _httpClient.GetAsync($"UnreadGroupChatMessage/findByMessageId/{id}", refreshToken, Port.ChatApi);
        if (responseMessage.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            return Unauthorized();
        }
        else if (responseMessage.IsSuccessStatusCode)
        {
            var unredMessages = await responseMessage.Content.ReadFromJsonAsync<IEnumerable<UnreadGroupChatMessageModel>>();

            return Ok(unredMessages);
        }

        return BadRequest();
    }

    [HttpPost]
    public async Task<IActionResult> Create(UnreadGroupChatMessageModel message)
    {
        if (!HttpContext.Request.Cookies.TryGetValue("refreshToken", out var refreshToken))
        {
            return Unauthorized();
        }

        var responseMessage = await _httpClient.PostAsync("UnreadGroupChatMessage", JsonContent.Create(message), refreshToken, Port.ChatApi);
        if (responseMessage.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            return Unauthorized();
        }
        else if (responseMessage.IsSuccessStatusCode)
        {
            var personalChatMessage = await responseMessage.Content.ReadFromJsonAsync<UnreadGroupChatMessageModel>();
            return Ok(personalChatMessage);
        }

        return BadRequest();
    }

    [HttpPut]
    public async Task<IActionResult> Update(UnreadGroupChatMessageModel message)
    {
        if (!HttpContext.Request.Cookies.TryGetValue("refreshToken", out var refreshToken))
        {
            return Unauthorized();
        }

        var responseMessage = await _httpClient.PutAsync("UnreadGroupChatMessage", JsonContent.Create(message), refreshToken, Port.ChatApi);
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

        var responseMessage = await _httpClient.DeletAsync($"UnreadGroupChatMessage/{id}", refreshToken, Port.ChatApi);
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
