using CombatAnalysis.WebApp.Consts;
using CombatAnalysis.WebApp.Extensions;
using CombatAnalysis.WebApp.Interfaces;
using CombatAnalysis.WebApp.Models.Chat;
using Microsoft.AspNetCore.Mvc;

namespace CombatAnalysis.WebApp.Controllers.Chat;

[Route("api/v1/[controller]")]
[ApiController]
public class PersonalChatController : ControllerBase
{
    private readonly IHttpClientHelper _httpClient;

    public PersonalChatController(IHttpClientHelper httpClient)
    {
        _httpClient = httpClient;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetByUserId(string id)
    {
        if (!HttpContext.Request.Cookies.TryGetValue("refreshToken", out var refreshToken))
        {
            return Unauthorized();
        }

        var responseMessage = await _httpClient.GetAsync($"PersonalChat", refreshToken, Port.ChatApi);
        if (responseMessage.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            return Unauthorized();
        }
        else if (responseMessage.IsSuccessStatusCode)
        {
            var personalChats = await responseMessage.Content.ReadFromJsonAsync<IEnumerable<PersonalChatModel>>();
            var myPersonalChats = personalChats.Where(x => x.InitiatorId == id || x.CompanionId == id).ToList();

            return Ok(myPersonalChats);
        }

        return BadRequest();
    }

    [HttpGet("isExist")]
    public async Task<IActionResult> IsExist(string initiatorId, string companionId)
    {
        if (!HttpContext.Request.Cookies.TryGetValue("refreshToken", out var refreshToken))
        {
            return Unauthorized();
        }

        var responseMessage = await _httpClient.GetAsync($"PersonalChat", refreshToken, Port.ChatApi);
        if (responseMessage.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            return Unauthorized();
        }
        else if (!responseMessage.IsSuccessStatusCode)
        {
            return BadRequest();
        }

        var personalChats = await responseMessage.Content.ReadFromJsonAsync<IEnumerable<PersonalChatModel>>();
        var chats = personalChats.Where(x => x.InitiatorId == initiatorId && x.CompanionId == companionId).ToList();
        if (!chats.Any())
        {
            chats = personalChats.Where(x => x.CompanionId == companionId && x.InitiatorId == initiatorId).ToList();
            if (!chats.Any())
            {
                return Ok(false);
            }
        }

        return Ok(true);
    }

    [HttpPost]
    public async Task<IActionResult> Create(PersonalChatModel chat)
    {
        if (!HttpContext.Request.Cookies.TryGetValue("refreshToken", out var refreshToken))
        {
            return Unauthorized();
        }

        var responseMessage = await _httpClient.PostAsync("PersonalChat", JsonContent.Create(chat), refreshToken, Port.ChatApi);
        if (responseMessage.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            return Unauthorized();
        }
        else if (responseMessage.IsSuccessStatusCode)
        {
            var personalChat = await responseMessage.Content.ReadFromJsonAsync<PersonalChatModel>();
            return Ok(personalChat);
        }

        return BadRequest();
    }

    [HttpPut]
    public async Task<IActionResult> Update(PersonalChatModel chat)
    {
        if (!HttpContext.Request.Cookies.TryGetValue("refreshToken", out var refreshToken))
        {
            return Unauthorized();
        }

        var responseMessage = await _httpClient.PutAsync("PersonalChat", JsonContent.Create(chat), refreshToken, Port.ChatApi);
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

    [HttpDelete("{chatId:int:min(1)}")]
    public async Task<IActionResult> Delete(int chatId)
    {
        if (!HttpContext.Request.Cookies.TryGetValue("refreshToken", out var refreshToken))
        {
            return Unauthorized();
        }

        var responseMessage = await _httpClient.DeletAsync($"PersonalChat/{chatId}", refreshToken, Port.ChatApi);
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
