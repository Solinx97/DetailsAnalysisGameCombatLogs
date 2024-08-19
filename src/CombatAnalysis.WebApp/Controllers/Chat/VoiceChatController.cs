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
public class VoiceChatController : ControllerBase
{
    private readonly IHttpClientHelper _httpClient;

    public VoiceChatController(IHttpClientHelper httpClient)
    {
        _httpClient = httpClient;
    }

    [HttpPost]
    public async Task<IActionResult> Create(VoiceChatModel chat)
    {
        if (!Request.Cookies.TryGetValue(AuthenticationCookie.AccessToken.ToString(), out var accessToken))
        {
            return Unauthorized();
        }

        var responseMessage = await _httpClient.PostAsync("VoiceChat", JsonContent.Create(chat), accessToken, Port.ChatApi);
        if (responseMessage.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            return Unauthorized();
        }
        else if (responseMessage.IsSuccessStatusCode)
        {
            var groupChat = await responseMessage.Content.ReadFromJsonAsync<VoiceChatModel>();

            return Ok(groupChat);
        }

        return BadRequest();
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        if (!Request.Cookies.TryGetValue(AuthenticationCookie.AccessToken.ToString(), out var accessToken))
        {
            return Unauthorized();
        }

        var responseMessage = await _httpClient.GetAsync("VoiceChat", accessToken, Port.ChatApi);
        if (responseMessage.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            return Unauthorized();
        }
        else if (responseMessage.IsSuccessStatusCode)
        {
            var groupChats = await responseMessage.Content.ReadFromJsonAsync<IEnumerable<VoiceChatModel>>();

            return Ok(groupChats);
        }

        return BadRequest();
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(string id)
    {
        if (!Request.Cookies.TryGetValue(AuthenticationCookie.AccessToken.ToString(), out var accessToken))
        {
            return Unauthorized();
        }

        var responseMessage = await _httpClient.GetAsync($"VoiceChat/{id}", accessToken, Port.ChatApi);
        if (responseMessage.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            return Unauthorized();
        }
        else if (responseMessage.IsSuccessStatusCode)
        {
            var groupChat = await responseMessage.Content.ReadFromJsonAsync<VoiceChatModel>();

            return Ok(groupChat);
        }

        return BadRequest();
    }

    [HttpPut]
    public async Task<IActionResult> Update(VoiceChatModel chat)
    {
        if (!Request.Cookies.TryGetValue(AuthenticationCookie.AccessToken.ToString(), out var accessToken))
        {
            return Unauthorized();
        }

        var responseMessage = await _httpClient.PutAsync("VoiceChat", JsonContent.Create(chat), accessToken, Port.ChatApi);
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

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        if (!Request.Cookies.TryGetValue(AuthenticationCookie.AccessToken.ToString(), out var accessToken))
        {
            return Unauthorized();
        }

        var responseMessage = await _httpClient.DeletAsync($"VoiceChat/{id}", accessToken, Port.ChatApi);
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
