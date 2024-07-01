using CombatAnalysis.WebApp.Consts;
using CombatAnalysis.WebApp.Extensions;
using CombatAnalysis.WebApp.Interfaces;
using CombatAnalysis.WebApp.Models.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CombatAnalysis.WebApp.Controllers.Identity;

[Route("api/v1/[controller]")]
[ApiController]
public class IdentityController : ControllerBase
{
    private readonly IHttpClientHelper _httpClient;

    public IdentityController(IHttpClientHelper httpClient)
    {
        _httpClient = httpClient;
    }

    [HttpGet]
    public async Task<IActionResult> AuthorizationCodeExchange(string codeVerifier, string authorizationCode)
    {
        var encodedAuthorizationCode = Uri.EscapeDataString(authorizationCode);
        var url = $"Token?grantType=authorization_code&clientId={Authorization.ClientId}&codeVerifier={codeVerifier}&code={encodedAuthorizationCode}&redirectUri={Authorization.RedirectUri}";

        var responseMessage = await _httpClient.GetAsync(url, Port.Identity);
        if (responseMessage.StatusCode == System.Net.HttpStatusCode.InternalServerError)
        {
            return StatusCode(500);
        }

        if (!responseMessage.IsSuccessStatusCode)
        {
            return BadRequest();
        }

        var accessToken = await responseMessage.Content.ReadFromJsonAsync<AccessTokenModel>();
        HttpContext.Response.Cookies.Append("accessToken", accessToken.AccessToken, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Lax,
            Expires = DateTimeOffset.UtcNow.AddMinutes(accessToken.ExpiresInMinutes),
        });
        HttpContext.Response.Cookies.Append("refreshToken", accessToken.RefreshToken, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Lax,
            Expires = DateTimeOffset.UtcNow.AddMinutes(accessToken.ExpiresInMinutes).AddDays(7)
        });

        return Ok();
    }
}
