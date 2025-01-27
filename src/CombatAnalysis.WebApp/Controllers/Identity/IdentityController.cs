using CombatAnalysis.WebApp.Consts;
using CombatAnalysis.WebApp.Enums;
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
        _httpClient.APIUrl = API.Identity;
    }

    [HttpGet]
    public async Task<IActionResult> AuthorizationCodeExchange(string authorizationCode)
    {
        try
        {
            if (!HttpContext.Request.Cookies.TryGetValue(AuthenticationCookie.CodeVerifier.ToString(), out var codeVerifier))
            {
                return BadRequest();
            }

            HttpContext.Response.Cookies.Delete(AuthenticationCookie.CodeVerifier.ToString());

            var encodedAuthorizationCode = Uri.EscapeDataString(authorizationCode);
            var url = $"Token?grantType={AuthenticationGrantType.Authorization}&clientId={Authentication.ClientId}&codeVerifier={codeVerifier}&code={encodedAuthorizationCode}&redirectUri={Authentication.RedirectUri}";

            var responseMessage = await _httpClient.GetAsync(url);
            if (responseMessage.StatusCode == System.Net.HttpStatusCode.InternalServerError)
            {
                return StatusCode(500);
            }
            else if (!responseMessage.IsSuccessStatusCode)
            {
                return BadRequest();
            }

            var token = await responseMessage.Content.ReadFromJsonAsync<AccessTokenModel>();
            if (token == null)
            {
                return BadRequest();
            }

            HttpContext.Response.Cookies.Append(nameof(AuthenticationCookie.AccessToken), token.AccessToken, new CookieOptions
            {
                Domain = Authentication.CookieDomain,
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = token.Expires,
            });
            HttpContext.Response.Cookies.Append(nameof(AuthenticationCookie.RefreshToken), token.RefreshToken, new CookieOptions
            {
                Domain = Authentication.CookieDomain,
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = token.Expires.AddDays(Authentication.RefreshTokenExpiresDays)
            });

            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during authorization code exchange");

            return BadRequest(ex.Message);
        }
    }

    [HttpGet("userPrivacy/{id}")]
    public async Task<IActionResult> GetUserPrivacy(string id)
    {
        var responseMessage = await _httpClient.GetAsync($"Identity/{id}");
        if (responseMessage.StatusCode == System.Net.HttpStatusCode.InternalServerError)
        {
            return StatusCode(500);
        }
        else if (!responseMessage.IsSuccessStatusCode)
        {
            return BadRequest();
        }

        var email = await responseMessage.Content.ReadAsStringAsync();

        return Ok(email);
    }
}
