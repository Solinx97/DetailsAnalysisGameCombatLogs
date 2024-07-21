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
    private readonly ILogger<IdentityController> _logger;

    public IdentityController(IHttpClientHelper httpClient, ILogger<IdentityController> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> AuthorizationCodeExchange(string codeVerifier, string authorizationCode)
    {
        try
        {
            var encodedAuthorizationCode = Uri.EscapeDataString(authorizationCode);
            var url = $"Token?grantType={AuthenticationGrantType.Authorization}&clientId={Authentication.ClientId}&codeVerifier={codeVerifier}&code={encodedAuthorizationCode}&redirectUri={Authentication.RedirectUri}";

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
            if (token == null)
            {
                return BadRequest();
            }

            HttpContext.Response.Cookies.Append(AuthenticationTokenType.AccessToken.ToString(), token.AccessToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = false,
                SameSite = SameSiteMode.Lax,
                Expires = token.Expires,
            });
            HttpContext.Response.Cookies.Append(AuthenticationTokenType.RefreshToken.ToString(), token.RefreshToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = false,
                SameSite = SameSiteMode.Lax,
                Expires = token.Expires.AddDays(Authentication.RefreshTokenExpiresDays)
            });

            var identityUserId = AccessTokenHelper.GetUserIdFromToken(token.AccessToken);

            return Ok(identityUserId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during authorization code exchange");

            return BadRequest(ex.Message);
        }
    }
}
