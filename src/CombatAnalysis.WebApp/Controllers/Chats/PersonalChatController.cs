using CombatAnalysis.WebApp.Consts;
using CombatAnalysis.WebApp.Interfaces;
using CombatAnalysis.WebApp.Models.Chats;
using Microsoft.AspNetCore.Mvc;

namespace CombatAnalysis.WebApp.Controllers.Chats;

[Route("api/v1/[controller]")]
[ApiController]
public class PersonalChatController : ControllerBase
{
    private readonly IHttpClientHelper _httpClient;

    public PersonalChatController(IHttpClientHelper httpClient)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = Port.ChatApi;
    }

    [HttpGet("{userId}")]
    public async Task<IActionResult> GetByUserId(string userId)
    {
        var responseMessage = await _httpClient.GetAsync($"PersonalChat");
        if (responseMessage.StatusCode == System.Net.HttpStatusCode.OK)
        {
            var personalChats = await responseMessage.Content.ReadFromJsonAsync<IEnumerable<PersonalChatModel>>();
            var myPersonalChats = personalChats.Where(x => x.InitiatorId == userId || x.CompanionId == userId).ToList();

            return Ok(myPersonalChats);
        }

        return BadRequest();
    }

    [HttpPost]
    public async Task<IActionResult> Create(PersonalChatModel chat)
    {
        var responseMessage = await _httpClient.PostAsync("PersonalChat", JsonContent.Create(chat));
        if (responseMessage.StatusCode == System.Net.HttpStatusCode.OK)
        {
            return Ok();
        }

        return BadRequest();
    }

    [HttpPut]
    public async Task<IActionResult> Update(PersonalChatModel chat)
    {
        var responseMessage = await _httpClient.PutAsync("PersonalChat", JsonContent.Create(chat));
        if (responseMessage.StatusCode == System.Net.HttpStatusCode.OK)
        {
            return Ok();
        }

        return BadRequest();
    }

    [HttpDelete("{chatId:int:min(1)}")]
    public async Task<IActionResult> Delete(int chatId)
    {
        var responseMessage = await _httpClient.DeletAsync($"PersonalChat/{chatId}");
        if (responseMessage.StatusCode == System.Net.HttpStatusCode.OK)
        {
            return Ok();
        }

        return BadRequest();
    }
}
