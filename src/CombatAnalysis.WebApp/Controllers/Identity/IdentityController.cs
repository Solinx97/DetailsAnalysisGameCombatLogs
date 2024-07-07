using CombatAnalysis.WebApp.Consts;
using CombatAnalysis.WebApp.Enums;
using CombatAnalysis.WebApp.Extensions;
using CombatAnalysis.WebApp.Helpers;
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
        try
        {
            var encodedAuthorizationCode = Uri.EscapeDataString(authorizationCode);
            var url = $"Token?grantType=authorization_code&clientId={Authentication.ClientId}&codeVerifier={codeVerifier}&code={encodedAuthorizationCode}&redirectUri={Authentication.RedirectUri}";

            var responseMessage = await _httpClient.GetAsync(url, Port.Identity);
            if (responseMessage.StatusCode == System.Net.HttpStatusCode.InternalServerError)
            {
                return StatusCode(500);
            }

            if (!responseMessage.IsSuccessStatusCode)
            {
                return BadRequest();
            }

            var token = await responseMessage.Content.ReadFromJsonAsync<AccessTokenModel>();
            HttpContext.Response.Cookies.Append(AuthenticationTokenType.AccessToken.ToString(), token.AccessToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Lax,
                Expires = DateTimeOffset.UtcNow.AddMinutes(token.ExpiresInMinutes),
            });
            HttpContext.Response.Cookies.Append(AuthenticationTokenType.RefreshToken.ToString(), token.RefreshToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Lax,
                Expires = DateTimeOffset.UtcNow.AddMinutes(token.ExpiresInMinutes).AddDays(7)
            });

            var identityUserId = AccessTokenHelper.GetUserIdFromToken(token.AccessToken);

            return Ok(identityUserId);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
