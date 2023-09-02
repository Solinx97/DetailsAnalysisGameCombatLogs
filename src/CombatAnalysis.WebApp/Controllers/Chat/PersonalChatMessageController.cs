using CombatAnalysis.WebApp.Consts;
using CombatAnalysis.WebApp.Extensions;
using CombatAnalysis.WebApp.Interfaces;
using CombatAnalysis.WebApp.Models.Chat;
using Microsoft.AspNetCore.Mvc;

namespace CombatAnalysis.WebApp.Controllers.Chat;

[Route("api/v1/[controller]")]
[ApiController]
public class PersonalChatMessageController : ControllerBase
{
    private readonly IHttpClientHelper _httpClient;

    public PersonalChatMessageController(IHttpClientHelper httpClient)
    {
        _httpClient = httpClient;
    }

    [HttpGet("findByChatId/{chatId:int:min(1)}")]
    public async Task<IActionResult> Find(int chatId)
    {
        if (!HttpContext.Request.Cookies.TryGetValue("refreshToken", out var refreshToken))
        {
            return Unauthorized();
        }

        var responseMessage = await _httpClient.GetAsync($"PersonalChatMessage/findByChatId/{chatId}", refreshToken, Port.ChatApi);
        if (responseMessage.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            return Unauthorized();
        }
        else if (responseMessage.IsSuccessStatusCode)
        {
            var personalChatMessages = await responseMessage.Content.ReadFromJsonAsync<IEnumerable<PersonalChatMessageModel>>();

            return Ok(personalChatMessages);
        }

        return BadRequest();
    }

    [HttpPost]
    public async Task<IActionResult> Create(PersonalChatMessageModel message)
    {
        if (!HttpContext.Request.Cookies.TryGetValue("refreshToken", out var refreshToken))
        {
            return Unauthorized();
        }

        var responseMessage = await _httpClient.PostAsync("PersonalChatMessage", JsonContent.Create(message), refreshToken, Port.ChatApi);
        if (responseMessage.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            return Unauthorized();
        }
        else if (responseMessage.IsSuccessStatusCode)
        {
            var personalChatMessage = await responseMessage.Content.ReadFromJsonAsync<PersonalChatMessageModel>();
            return Ok(personalChatMessage);
        }

        return BadRequest();
    }

    [HttpPut]
    public async Task<IActionResult> Update(PersonalChatMessageModel message)
    {
        if (!HttpContext.Request.Cookies.TryGetValue("refreshToken", out var refreshToken))
        {
            return Unauthorized();
        }

        var responseMessage = await _httpClient.PutAsync("PersonalChatMessage", JsonContent.Create(message), refreshToken, Port.ChatApi);
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
        if (!HttpContext.Request.Cookies.TryGetValue("refreshToken", out var refreshToken))
        {
            return Unauthorized();
        }

        var responseMessage = await _httpClient.DeletAsync($"PersonalChatMessage/{messageId}", refreshToken, Port.ChatApi);
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

    [HttpDelete("deleteByChatId/{chatId:int:min(1)}")]
    public async Task<IActionResult> DeleteByChatId(int chatId)
    {
        if (!HttpContext.Request.Cookies.TryGetValue("refreshToken", out var refreshToken))
        {
            return Unauthorized();
        }

        var responseMessage = await _httpClient.GetAsync($"PersonalChatMessage/findByChatId/{chatId}", refreshToken, Port.ChatApi);
        if (responseMessage.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            return Unauthorized();
        }
        else if (responseMessage.IsSuccessStatusCode)
        {
            var messages = await responseMessage.Content.ReadFromJsonAsync<IEnumerable<PersonalChatMessageModel>>();
            foreach (var item in messages)
            {
                await Delete(item.Id);
            }

            return Ok();
        }

        return BadRequest();
    }
}
