using CombatAnalysis.WebApp.Attributes;
using CombatAnalysis.WebApp.Consts;
using CombatAnalysis.WebApp.Interfaces;
using CombatAnalysis.WebApp.Models.Chat;
using Microsoft.AspNetCore.Mvc;

namespace CombatAnalysis.WebApp.Controllers.Chat;

[ServiceFilter(typeof(RequireAccessTokenAttribute))]
[Route("api/v1/[controller]")]
[ApiController]
public class GroupChatUserController : ControllerBase
{
    private readonly IHttpClientHelper _httpClient;

    public GroupChatUserController(IHttpClientHelper httpClient)
    {
        _httpClient = httpClient;
        _httpClient.APIUrl = API.Chat;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(string id)
    {
        var responseMessage = await _httpClient.GetAsync($"GroupChatUser/{id}");
        if (responseMessage.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            return Unauthorized();
        }
        else if (!responseMessage.IsSuccessStatusCode)
        {
            return BadRequest();
        }

        var groupChatUser = await responseMessage.Content.ReadFromJsonAsync<GroupChatUserModel>();

        return Ok(groupChatUser);
    }

    [HttpGet("findMeInChat")]
    public async Task<IActionResult> Find(int chatId, string appUserId)
    {
        var responseMessage = await _httpClient.GetAsync($"GroupChatUser/findMeInChat?chatId={chatId}&appUserId={appUserId}");
        if (responseMessage.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            return Unauthorized();
        }
        else if (!responseMessage.IsSuccessStatusCode)
        {
            return BadRequest();
        }

        var meInChat = await responseMessage.Content.ReadFromJsonAsync<GroupChatUserModel>();

        return Ok(meInChat);
    }

    [HttpGet("findByChatId/{id:int:min(1)}")]
    public async Task<IActionResult> FindByChatId(int id)
    {
        var responseMessage = await _httpClient.GetAsync($"GroupChatUser/findByChatId/{id}");
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
        var responseMessage = await _httpClient.GetAsync($"GroupChatUser/findByUserId/{id}");
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
        var responseMessage = await _httpClient.PostAsync("GroupChatUser", JsonContent.Create(user));
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
        var responseMessage = await _httpClient.DeletAsync($"GroupChatUser/{id}");
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
