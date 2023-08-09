using CombatAnalysis.WebApp.Consts;
using CombatAnalysis.WebApp.Extensions;
using CombatAnalysis.WebApp.Interfaces;
using CombatAnalysis.WebApp.Models.Chats;
using Microsoft.AspNetCore.Mvc;

namespace CombatAnalysis.WebApp.Controllers.Chats;

[Route("api/v1/[controller]")]
[ApiController]
public class InviteToGroupChatController : ControllerBase
{
    private readonly IHttpClientHelper _httpClient;

    public InviteToGroupChatController(IHttpClientHelper httpClient)
    {
        _httpClient = httpClient;
    }

    [HttpPost]
    public async Task<IActionResult> Create(InviteToGroupChatModel chat)
    {
        if (!HttpContext.Request.Cookies.TryGetValue("refreshToken", out var refreshToken))
        {
            return Unauthorized();
        }

        var responseMessage = await _httpClient.PostAsync("InviteToGroupChat", JsonContent.Create(chat), refreshToken, Port.ChatApi);
        if (responseMessage.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            return Unauthorized();
        }
        else if (responseMessage.IsSuccessStatusCode)
        {
            var inviteToGroupChat = await responseMessage.Content.ReadFromJsonAsync<InviteToGroupChatModel>();

            return Ok(inviteToGroupChat);
        }

        return BadRequest();
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        if (!HttpContext.Request.Cookies.TryGetValue("refreshToken", out var refreshToken))
        {
            return Unauthorized();
        }

        var responseMessage = await _httpClient.GetAsync("InviteToGroupChat", refreshToken, Port.ChatApi);
        if (responseMessage.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            return Unauthorized();
        }
        else if (responseMessage.IsSuccessStatusCode)
        {
            var invitesToGroupChat = await responseMessage.Content.ReadFromJsonAsync<IEnumerable<InviteToGroupChatModel>>();

            return Ok(invitesToGroupChat);
        }

        return BadRequest();
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        if (!HttpContext.Request.Cookies.TryGetValue("refreshToken", out var refreshToken))
        {
            return Unauthorized();
        }

        var responseMessage = await _httpClient.GetAsync($"InviteToGroupChat/{id}", refreshToken, Port.ChatApi);
        if (responseMessage.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            return Unauthorized();
        }
        else if (responseMessage.IsSuccessStatusCode)
        {
            var inviteToGroupChat = await responseMessage.Content.ReadFromJsonAsync<InviteToGroupChatModel>();

            return Ok(inviteToGroupChat);
        }

        return BadRequest();
    }

    [HttpPut]
    public async Task<IActionResult> Update(InviteToGroupChatModel chat)
    {
        if (!HttpContext.Request.Cookies.TryGetValue("refreshToken", out var refreshToken))
        {
            return Unauthorized();
        }

        var responseMessage = await _httpClient.PutAsync("InviteToGroupChat", JsonContent.Create(chat), refreshToken, Port.ChatApi);
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
