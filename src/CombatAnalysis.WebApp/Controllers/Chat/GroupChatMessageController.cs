using CombatAnalysis.WebApp.Attributes;
using CombatAnalysis.WebApp.Consts;
using CombatAnalysis.WebApp.Interfaces;
using CombatAnalysis.WebApp.Models.Chat;
using Microsoft.AspNetCore.Mvc;

namespace CombatAnalysis.WebApp.Controllers.Chat;

[ServiceFilter(typeof(RequireAccessTokenAttribute))]
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

    [HttpGet("count/{chatId}")]
    public async Task<IActionResult> Count(int chatId)
    {
        var responseMessage = await _httpClient.GetAsync($"GroupChatMessage/count/{chatId}");
        var count = await responseMessage.Content.ReadFromJsonAsync<int>();

        return Ok(count);
    }

    [HttpGet("getByChatId")]
    public async Task<IActionResult> GetByChatId(int chatId, int pageSize)
    {
        var responseMessage = await _httpClient.GetAsync($"GroupChatMessage/getByChatId?chatId={chatId}&pageSize={pageSize}");
        if (responseMessage.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            return Unauthorized();
        }
        else if (responseMessage.IsSuccessStatusCode)
        {
            var messages = await responseMessage.Content.ReadFromJsonAsync<IEnumerable<GroupChatMessageModel>>();

            return Ok(messages);
        }

        return BadRequest();
    }

    [HttpGet("getMoreByChatId")]
    public async Task<IActionResult> GetMoreByChatId(int chatId, int offset, int pageSize)
    {
        var responseMessage = await _httpClient.GetAsync($"GroupChatMessage/getMoreByChatId?chatId={chatId}&offset={offset}&pageSize={pageSize}");
        if (responseMessage.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            return Unauthorized();
        }
        else if (responseMessage.IsSuccessStatusCode)
        {
            var messages = await responseMessage.Content.ReadFromJsonAsync<IEnumerable<GroupChatMessageModel>>();

            return Ok(messages);
        }

        return BadRequest();
    }

    [HttpPost]
    public async Task<IActionResult> Create(GroupChatMessageModel message)
    {
        var responseMessage = await _httpClient.PostAsync("GroupChatMessage", JsonContent.Create(message));
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
        var responseMessage = await _httpClient.PutAsync("GroupChatMessage", JsonContent.Create(message));
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
        var responseMessage = await _httpClient.DeletAsync($"GroupChatMessage/{id}");
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
