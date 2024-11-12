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
        var accessToken = HttpContext.Items[AuthenticationCookie.AccessToken.ToString()] as string;

        var responseMessage = await _httpClient.GetAsync($"PersonalChat", accessToken, Port.ChatApi);
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
        var accessToken = HttpContext.Items[AuthenticationCookie.AccessToken.ToString()] as string;

        var responseMessage = await _httpClient.GetAsync($"PersonalChat", accessToken, Port.ChatApi);
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
        var accessToken = HttpContext.Items[AuthenticationCookie.AccessToken.ToString()] as string;

        var responseMessage = await _httpClient.PostAsync("PersonalChat", JsonContent.Create(chat), accessToken, Port.ChatApi);
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
        var accessToken = HttpContext.Items[AuthenticationCookie.AccessToken.ToString()] as string;

        var responseMessage = await _httpClient.PutAsync("PersonalChat", JsonContent.Create(chat), accessToken, Port.ChatApi);
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
        var accessToken = HttpContext.Items[AuthenticationCookie.AccessToken.ToString()] as string;

        var responseMessage = await _httpClient.DeletAsync($"PersonalChat/{chatId}", accessToken, Port.ChatApi);
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
