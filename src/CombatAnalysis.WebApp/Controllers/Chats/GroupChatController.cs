using CombatAnalysis.WebApp.Consts;
using CombatAnalysis.WebApp.Interfaces;
using CombatAnalysis.WebApp.Models.Chats;
using Microsoft.AspNetCore.Mvc;

namespace CombatAnalysis.WebApp.Controllers.Chats;

[Route("api/v1/[controller]")]
[ApiController]
public class GroupChatController : ControllerBase
{
    private readonly IHttpClientHelper _httpClient;

    public GroupChatController(IHttpClientHelper httpClient)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = Port.ChatApi;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var responseMessage = await _httpClient.GetAsync($"GroupChat/{id}");
        if (responseMessage.StatusCode == System.Net.HttpStatusCode.OK)
        {
            var groupChats = await responseMessage.Content.ReadFromJsonAsync<GroupChatModel>();

            return Ok(groupChats);
        }


        return BadRequest();
    }

    [HttpPut]
    public async Task<IActionResult> Update(GroupChatModel chat)
    {
        var responseMessage = await _httpClient.PutAsync($"GroupChat", JsonContent.Create(chat));
        if (responseMessage.StatusCode == System.Net.HttpStatusCode.OK)
        {
            return Ok();
        }

        return BadRequest();
    }
}
