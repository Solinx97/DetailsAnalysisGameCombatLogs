using CombatAnalysis.EnhancedWebApp.Server.Consts;
using CombatAnalysis.EnhancedWebApp.Server.Enums;
using CombatAnalysis.EnhancedWebApp.Server.Helpers;
using CombatAnalysis.EnhancedWebApp.Server.Interfaces;
using CombatAnalysis.EnhancedWebApp.Server.Models.WoWGameData;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Text;

namespace CombatAnalysis.EnhancedWebApp.Server.Controllers.WoWGameData;

[Route("api/v1/[controller]")]
[ApiController]
public class BattleNetIdentityController : ControllerBase
{
    private readonly IHttpClientHelper _httpClient;
    private readonly Authentication _authentication;
    private readonly BattleNet _battleNet;

    public BattleNetIdentityController(IOptions<BattleNet> battleNet, IOptions<Authentication> authentication, IHttpClientHelper httpClient)
    {
        _authentication = authentication.Value;
        _battleNet = battleNet.Value;

        _httpClient = httpClient;
        _httpClient.APIUrl = battleNet.Value.BattleNetAutAPI;
        _httpClient.BaseAddressApi = "";
    }

    [HttpPost]
    public async Task<IActionResult> GetToken()
    {
        var credentials = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{_battleNet.ClientId}:{_battleNet.clientSecret}"));

        var content = new FormUrlEncodedContent(new Dictionary<string, string>
        {
            ["grant_type"] = "client_credentials"
        });

        _httpClient.AddAuthorizationHeader("Basic", credentials);
        var responseMessage = await _httpClient.PostAsync("token", content);
        responseMessage.EnsureSuccessStatusCode();

        var token = await responseMessage.Content.ReadFromJsonAsync<BattleNetTokenResponse>();
        ArgumentNullException.ThrowIfNull(token, nameof(token));

        HttpContext.Response.Cookies.Append(nameof(AuthenticationCookie.BattleNetAccessToken), token.AccessToken, new CookieOptions
        {
            Domain = _authentication.CookieDomain,
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.None,
            Expires = DateTimeOffset.UtcNow.AddSeconds(token.ExpiresIn),
        });

        return NoContent();
    }

    [HttpPost("authorization")]
    public async Task<IActionResult> AuthorizationCodeFlow()
    {
        var state = PKCEHelper.GenerateCodeVerifier();
        ArgumentException.ThrowIfNullOrEmpty(state, nameof(state));

        var uri = $"{_battleNet.BattleNetAutAPI}authorize" +
            "?response_type=code" +
            $"&scope={Uri.EscapeDataString("openid wow.profile")}" +
            $"&state={state}" +
            $"&redirect_uri={Uri.EscapeDataString(_battleNet.RedirectUri)}" +
            $"&client_id={Uri.EscapeDataString(_battleNet.ClientId)}";

        HttpContext.Response.Cookies.Append(nameof(AuthenticationCookie.BattleNetState), state, new CookieOptions
        {
            Domain = _authentication.CookieDomain,
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.None,
        });

        return Ok(new { uri });
    }

    [HttpPost("codeExchange")]
    public async Task<IActionResult> AuthorizationCodeExchange(string authorizationCode)
    {
        var credentials = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{_battleNet.ClientId}:{_battleNet.clientSecret}"));

        var content = new FormUrlEncodedContent(new Dictionary<string, string>
        {
            ["redirect_uri"] = _battleNet.RedirectUri,
            ["grant_type"] = "authorization_code",
            ["code"] = authorizationCode,
        });

        _httpClient.AddAuthorizationHeader("Basic", credentials);
        var responseMessage = await _httpClient.PostAsync("token", content);
        responseMessage.EnsureSuccessStatusCode();

        var token = await responseMessage.Content.ReadFromJsonAsync<BattleNetTokenResponse>();
        ArgumentNullException.ThrowIfNull(token, nameof(token));

        HttpContext.Response.Cookies.Append(nameof(AuthenticationCookie.BattleNetAuthorizationAccessToken), token.AccessToken, new CookieOptions
        {
            Domain = _authentication.CookieDomain,
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.None,
            Expires = DateTimeOffset.UtcNow.AddSeconds(token.ExpiresIn),
        });

        return NoContent();
    }

    [HttpGet("stateValidate")]
    public IActionResult StateValidate(string state)
    {
        ArgumentException.ThrowIfNullOrEmpty(state, nameof(state));

        if (!HttpContext.Request.Cookies.TryGetValue(nameof(AuthenticationCookie.BattleNetState), out var stateValue))
        {
            ArgumentException.ThrowIfNullOrEmpty(stateValue, nameof(stateValue));
        }

        HttpContext.Response.Cookies.Delete(nameof(AuthenticationCookie.BattleNetState));

        if (stateValue == state)
        {
            return NoContent();
        }

        return BadRequest();
    }

    [HttpGet("isAuthorized")]
    public IActionResult IsAuthorized()
    {
        var cookieName = nameof(AuthenticationCookie.BattleNetAuthorizationAccessToken);

        var exists = Request.Cookies.ContainsKey(cookieName);

        return Ok(new
        {
            authenticated = exists
        });
    }

    [HttpGet("disconnect")]
    public IActionResult Disconnect()
    {
        HttpContext.Response.Cookies.Delete(nameof(AuthenticationCookie.BattleNetAuthorizationAccessToken));

        return NoContent();
    }
}
