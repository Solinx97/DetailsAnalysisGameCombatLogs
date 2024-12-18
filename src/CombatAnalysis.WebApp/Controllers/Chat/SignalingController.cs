using CombatAnalysis.WebApp.Attributes;
using CombatAnalysis.WebApp.Consts;
using CombatAnalysis.WebApp.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CombatAnalysis.WebApp.Controllers.Chat;

[ServiceFilter(typeof(RequireAccessTokenAttribute))]
[Route("api/v1/[controller]")]
[ApiController]
public class SignalingController : ControllerBase
{
    private readonly IHttpClientHelper _httpClient;

    public SignalingController(IHttpClientHelper httpClient)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = Port.ChatApi;
    }

    [HttpGet("connected/{roomId}")]
    public async Task<IActionResult> GetConnectedUsers(int roomId)
    {
        var responseMessage = await _httpClient.GetAsync($"Signaling/connected/{roomId}");
        if (responseMessage.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            return Unauthorized();
        }
        else if (responseMessage.IsSuccessStatusCode)
        {
            var usersId = await responseMessage.Content.ReadFromJsonAsync<IEnumerable<string>>();

            return Ok(usersId);
        }

        return BadRequest();
    }
}
