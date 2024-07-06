using CombatAnalysis.WebApp.Consts;
using CombatAnalysis.WebApp.Enums;
using CombatAnalysis.WebApp.Extensions;
using CombatAnalysis.WebApp.Helpers;
using CombatAnalysis.WebApp.Interfaces;
using CombatAnalysis.WebApp.Models.User;
using Microsoft.AspNetCore.Mvc;

namespace CombatAnalysis.WebApp.Controllers.User;

[Route("api/v1/[controller]")]
[ApiController]
public class AuthenticationController : ControllerBase
{
    private readonly IHttpClientHelper _httpClient;

    public AuthenticationController(IHttpClientHelper httpClient)
    {
        _httpClient = httpClient;
    }

    [HttpGet]
    public async Task<IActionResult> Refresh()
    {
        try
        {
            if (!Request.Cookies.TryGetValue(AuthenticationTokenType.AccessToken.ToString(), out var accessToken))
            {
                return Ok();
            }

            var identityUserId = AccessTokenHelper.GetUserIdFromToken(accessToken);
            var responseMessage = await _httpClient.GetAsync($"Account/find/{identityUserId}", accessToken, Port.UserApi);
            if (responseMessage.IsSuccessStatusCode)
            {
                var user = await responseMessage.Content.ReadFromJsonAsync<AppUserModel>();
                return Ok(user);
            }

            return BadRequest();
        }
        catch (Exception ex)
        {
            return BadRequest($"Authentication refresh was failed. Error: {ex.Message}");
        }
    }
}
