using CombatAnalysis.EnhancedWebApp.Server.Consts;
using CombatAnalysis.EnhancedWebApp.Server.Enums;
using CombatAnalysis.EnhancedWebApp.Server.Helpers;
using CombatAnalysis.EnhancedWebApp.Server.Interfaces.HttpClients;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace CombatAnalysis.EnhancedWebApp.Server.Controllers.WoWGameData;

[Route("api/v1/[controller]")]
[ApiController]
public class BattleNetIdentityController(IOptions<BattleNet> battleNet, IOptions<Authentication> authentication, IWoWGameDataAuthApiClient httpClient) : ControllerBase
{
    private readonly IWoWGameDataAuthApiClient _httpClient = httpClient;
    private readonly Authentication _authentication = authentication.Value;
    private readonly BattleNet _battleNet = battleNet.Value;

    [HttpPost]
    public async Task<IActionResult> GetToken(CancellationToken cancellationToken)
    {
        var token = await _httpClient.GetTokenAsync(cancellationToken);

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
    public async Task<IActionResult> Authorization()
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
    public async Task<IActionResult> AuthorizationCodeExchange(string authorizationCode, CancellationToken cancellationToken)
    {
        var token = await _httpClient.AuthorizationCodeExchangeAsync(authorizationCode, cancellationToken);

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
