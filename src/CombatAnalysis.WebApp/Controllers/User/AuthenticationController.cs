using CombatAnalysis.WebApp.Consts;
using CombatAnalysis.WebApp.Extensions;
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
        if (!HttpContext.Request.Cookies.TryGetValue("refreshToken", out var refreshToken))
        {
            return Unauthorized();
        }

        var email = HttpContext.User.Identity.Name;
        var responseMessage = await _httpClient.GetAsync($"account/find/{email}", refreshToken, Port.UserApi);
        if (responseMessage.StatusCode == System.Net.HttpStatusCode.OK)
        {
            var user = await responseMessage.Content.ReadFromJsonAsync<AppUserModel>();
            return Ok(user);
        }


        return BadRequest();
    }
}
