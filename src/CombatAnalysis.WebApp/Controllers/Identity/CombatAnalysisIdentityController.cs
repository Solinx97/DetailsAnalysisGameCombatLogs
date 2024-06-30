using CombatAnalysis.WebApp.Consts;
using CombatAnalysis.WebApp.Extensions;
using CombatAnalysis.WebApp.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CombatAnalysis.WebApp.Controllers.Identity;

[Route("api/v1/[controller]")]
[ApiController]
public class CombatAnalysisIdentityController : ControllerBase
{
    private readonly IHttpClientHelper _httpClient;

    public CombatAnalysisIdentityController(IHttpClientHelper httpClient)
    {
        _httpClient = httpClient;
    }

    [HttpGet]
    public async Task<IActionResult> HandleAuthorizationResponse(string codeVerifier, string authorizationCode)
    {
        var encodedAuthorizationCode = Uri.EscapeDataString(authorizationCode);
        var url = $"Token?grantType=authorization_code&clientId=clientId&codeVerifier={codeVerifier}&code={encodedAuthorizationCode}&redirectUri=https://localhost:44479/";

        var responseMessage = await _httpClient.GetAsync(url, Port.Identity);
        if (responseMessage.StatusCode == System.Net.HttpStatusCode.InternalServerError)
        {
            return StatusCode(500);
        }

        if (!responseMessage.IsSuccessStatusCode)
        {
            return BadRequest();
        }


        var refreshToken = await responseMessage.Content.ReadAsStringAsync();
        HttpContext.Response.Cookies.Append("refreshToken", refreshToken, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Lax,
            Expires = DateTimeOffset.UtcNow.AddMinutes(TokenExpires.RefreshExpiresTimeInMinutes),
        });

        var dontLogoutValue = HttpContext.Request.Cookies?["dontLogout"];

        return Ok();
    }
}
