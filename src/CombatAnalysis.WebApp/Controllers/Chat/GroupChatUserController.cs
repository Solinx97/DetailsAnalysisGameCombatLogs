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
public class GroupChatUserController : ControllerBase
{
    private readonly IHttpClientHelper _httpClient;

    public GroupChatUserController(IHttpClientHelper httpClient)
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

        var responseMessage = await _httpClient.GetAsync("GroupChatUser", accessToken, Port.ChatApi);
        if (responseMessage.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            return Unauthorized();
        }
        else if (!responseMessage.IsSuccessStatusCode)
        {
            return BadRequest();
        }

        var groupChatUsers = await responseMessage.Content.ReadFromJsonAsync<IEnumerable<GroupChatUserModel>>();
        var myGroupCHatUser = groupChatUsers.Where(x => x.GroupChatId == chatId && x.CustomerId == userId).FirstOrDefault();

        return Ok(myGroupCHatUser);
    }

    [HttpGet("findByChatId/{id:int:min(1)}")]
    public async Task<IActionResult> FindByChatId(int id)
    {
        if (!Request.Cookies.TryGetValue(AuthenticationTokenType.AccessToken.ToString(), out var accessToken))
        {
            return Unauthorized();
        }

        var responseMessage = await _httpClient.GetAsync($"GroupChatUser/findByChatId/{id}", accessToken, Port.ChatApi);
        if (responseMessage.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            return Unauthorized();
        }
        else if (responseMessage.IsSuccessStatusCode)
        {
            var groupChatUsers = await responseMessage.Content.ReadFromJsonAsync<IEnumerable<GroupChatUserModel>>();

            return Ok(groupChatUsers);
        }

        return BadRequest();
    }


    [HttpGet("findByUserId/{id}")]
    public async Task<IActionResult> FindByUserId(string id)
    {
        if (!Request.Cookies.TryGetValue(AuthenticationTokenType.AccessToken.ToString(), out var accessToken))
        {
            return Unauthorized();
        }

        var responseMessage = await _httpClient.GetAsync($"GroupChatUser/findByUserId/{id}", accessToken, Port.ChatApi);
        if (responseMessage.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            return Unauthorized();
        }
        else if (responseMessage.IsSuccessStatusCode)
        {
            var groupChatUsers = await responseMessage.Content.ReadFromJsonAsync<IEnumerable<GroupChatUserModel>>();

            return Ok(groupChatUsers);
        }

        return BadRequest();
    }

    [HttpPost]
    public async Task<IActionResult> Create(GroupChatUserModel user)
    {
        if (!Request.Cookies.TryGetValue(AuthenticationTokenType.AccessToken.ToString(), out var accessToken))
        {
            return Unauthorized();
        }

        var responseMessage = await _httpClient.PostAsync("GroupChatUser", JsonContent.Create(user), accessToken, Port.ChatApi);
        if (responseMessage.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            return Unauthorized();
        }
        else if (responseMessage.IsSuccessStatusCode)
        {
            var groupChatUser = await responseMessage.Content.ReadFromJsonAsync<GroupChatUserModel>();

            return Ok(groupChatUser);
        }

        return BadRequest();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        if (!Request.Cookies.TryGetValue(AuthenticationTokenType.AccessToken.ToString(), out var accessToken))
        {
            return Unauthorized();
        }

        var responseMessage = await _httpClient.DeletAsync($"GroupChatUser/{id}", accessToken, Port.ChatApi);
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
