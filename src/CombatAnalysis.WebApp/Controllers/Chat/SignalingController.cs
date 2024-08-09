using CombatAnalysis.WebApp.Consts;
using CombatAnalysis.WebApp.Enums;
using CombatAnalysis.WebApp.Extensions;
using CombatAnalysis.WebApp.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CombatAnalysis.WebApp.Controllers.Chat;

[Route("api/v1/[controller]")]
[ApiController]
public class SignalingController : ControllerBase
{
    private readonly IHttpClientHelper _httpClient;

    public SignalingController(IHttpClientHelper httpClient)
    {
        _httpClient = httpClient;
    }

    [HttpGet("connected")]
    public async Task<IActionResult> GetConnectedUsers()
    {
        if (!Request.Cookies.TryGetValue(AuthenticationCookie.AccessToken.ToString(), out var accessToken))
        {
            return Unauthorized();
        }

        var responseMessage = await _httpClient.GetAsync("Signaling/connected", accessToken, Port.ChatApi);
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
