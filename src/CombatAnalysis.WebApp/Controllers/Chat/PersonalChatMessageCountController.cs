using CombatAnalysis.WebApp.Attributes;
using CombatAnalysis.WebApp.Consts;
using CombatAnalysis.WebApp.Interfaces;
using CombatAnalysis.WebApp.Models.Chat;
using Microsoft.AspNetCore.Mvc;

namespace CombatAnalysis.WebApp.Controllers.Chat;

[ServiceFilter(typeof(RequireAccessTokenAttribute))]
[Route("api/v1/[controller]")]
[ApiController]
public class PersonalChatMessageCountController : ControllerBase
{
    private readonly IHttpClientHelper _httpClient;

    public PersonalChatMessageCountController(IHttpClientHelper httpClient)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = Port.ChatApi;
    }

    [HttpGet("find")]
    public async Task<IActionResult> Find(int chatId, string userId)
    {
        var responseMessage = await _httpClient.GetAsync($"PersonalChatMessageCount");
        if (responseMessage.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            return Unauthorized();
        }
        else if (!responseMessage.IsSuccessStatusCode)
        {
            return BadRequest();
        }

        var personalChatMessagesCount = await responseMessage.Content.ReadFromJsonAsync<IEnumerable<PersonalChatMessageCountModel>>();
        var myPersonalChatMessagesCount = personalChatMessagesCount.Where(x => x.ChatId == chatId && x.AppUserId == userId).FirstOrDefault();

        return Ok(myPersonalChatMessagesCount);
    }

    [HttpPost]
    public async Task<IActionResult> Create(PersonalChatMessageCountModel message)
    {
        var responseMessage = await _httpClient.PostAsync("PersonalChatMessageCount", JsonContent.Create(message));
        if (responseMessage.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            return Unauthorized();
        }
        else if (responseMessage.IsSuccessStatusCode)
        {
            var personalChatMessage = await responseMessage.Content.ReadFromJsonAsync<PersonalChatMessageCountModel>();
            return Ok(personalChatMessage);
        }

        return BadRequest();
    }

    [HttpPut]
    public async Task<IActionResult> Update(PersonalChatMessageCountModel message)
    {
        var responseMessage = await _httpClient.PutAsync("PersonalChatMessageCount", JsonContent.Create(message));
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
        var responseMessage = await _httpClient.DeletAsync($"PersonalChatMessageCount/{id}");
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
