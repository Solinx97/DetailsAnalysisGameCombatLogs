using CombatAnalysis.WebApp.Attributes;
using CombatAnalysis.WebApp.Consts;
using CombatAnalysis.WebApp.Enums;
using CombatAnalysis.WebApp.Extensions;
using CombatAnalysis.WebApp.Helpers;
using CombatAnalysis.WebApp.Interfaces;
using CombatAnalysis.WebApp.Models.Authentication;
using CombatAnalysis.WebApp.Models.User;
using Microsoft.AspNetCore.Mvc;

namespace CombatAnalysis.WebApp.Controllers.Identity;

[Route("api/v1/[controller]")]
[ApiController]
public class AuthenticationController : ControllerBase
{
    private readonly IHttpClientHelper _httpClient;
    private readonly ILogger<AuthenticationController> _logger;

    public AuthenticationController(IHttpClientHelper httpClient, ILogger<AuthenticationController> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    [RequireAccessToken]
    [HttpGet]
    public async Task<IActionResult> RefreshAccessToken()
    {
        try
        {
            var accessToken = HttpContext.Items[AuthenticationCookie.AccessToken.ToString()] as string;

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
            _logger.LogError(ex, "Authentication refresh was failed.");

            return BadRequest($"Authentication refresh was failed. Error: {ex.Message}");
        }
    }

    [HttpGet("authorization")]
    public IActionResult Authorization(string identityPath)
    {
        var codeVerifier = PKCEHelper.GenerateCodeVerifier();
        var state = PKCEHelper.GenerateCodeVerifier();
        var codeChallenge = PKCEHelper.GenerateCodeChallenge(codeVerifier);

        var uri = $"{Authentication.IdentityServer}{identityPath}?grantType={AuthenticationGrantType.Code}" +
            $"&clientTd={Authentication.ClientId}&redirectUri={Authentication.RedirectUri}" +
            $"&scope={Authentication.ClientScope}&state={state}&codeChallengeMethod={Authentication.CodeChallengeMethod}" +
            $"&codeChallenge={codeChallenge}";

        var identityRedirect = new IdentityRedirect
        {
            Uri = uri
        };

        HttpContext.Response.Cookies.Append(AuthenticationCookie.CodeVerifier.ToString(), codeVerifier, new CookieOptions
        {
            HttpOnly = true,
            Secure = false,
            SameSite = SameSiteMode.Lax,
        });
        HttpContext.Response.Cookies.Append(AuthenticationCookie.State.ToString(), state, new CookieOptions
        {
            HttpOnly = true,
            Secure = false,
            SameSite = SameSiteMode.Lax,
        });

        return Ok(identityRedirect);
    }

    [HttpGet("verifyEmail")]
    public IActionResult VerifyEmail(string identityPath, string email)
    {
        var uri = $"{Authentication.IdentityServer}{identityPath}?email={email}&redirectUri={Authentication.RedirectUri}";

        var identityRedirect = new IdentityRedirect
        {
            Uri = uri
        };

        return Ok(identityRedirect);
    }

    [HttpGet("stateValidate")]
    public IActionResult StateValidate(string state)
    {
        if (!HttpContext.Request.Cookies.TryGetValue(AuthenticationCookie.State.ToString(), out var stateValue))
        {
            return BadRequest();
        }

        HttpContext.Response.Cookies.Delete(AuthenticationCookie.State.ToString());

        if (stateValue == state)
        {

            return Ok();
        }

        return BadRequest();
    }
}
