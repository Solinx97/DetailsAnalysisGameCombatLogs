using CombatAnalysis.WebApp.Attributes;
using CombatAnalysis.WebApp.Consts;
using CombatAnalysis.WebApp.Enums;
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
        _httpClient.APIUrl = Cluster.User;
    }

    [ServiceFilter(typeof(RequireAccessTokenAttribute))]
    [HttpGet]
    public async Task<IActionResult> RefreshAccessToken()
    {
        try
        {
            if (!HttpContext.Request.Cookies.TryGetValue(nameof(AuthenticationCookie.AccessToken), out var accessToken))
            {
                throw new ArgumentNullException(nameof(accessToken));
            }

            var identityUserId = AccessTokenHelper.GetUserIdFromToken(accessToken);
            var response = await _httpClient.GetAsync($"Account/find/{identityUserId}");
            response.EnsureSuccessStatusCode();

            var user = await response.Content.ReadFromJsonAsync<AppUserModel>();
            return Ok(user);
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, ex.Message);

            return BadRequest();
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Authentication refresh was failed.");

            return BadRequest($"Authentication refresh was failed. Error: {ex.Message}");
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

        var uri = $"{Servers.Identity}{identityPath}?grantType={AuthenticationGrantType.Code}" +
            $"&clientId={AuthenticationClient.ClientId}&redirectUri={Authentication.RedirectUri}" +
            $"&scope={AuthenticationClient.Scope}&state={state}&codeChallengeMethod={Authentication.CodeChallengeMethod}" +
            $"&codeChallenge={codeChallenge}";

        var identityRedirect = new IdentityRedirect
        {
            Uri = uri
        };

        HttpContext.Response.Cookies.Append(nameof(AuthenticationCookie.CodeVerifier), codeVerifier, new CookieOptions
        {
            Domain = Authentication.CookieDomain,
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.None,
        });
        HttpContext.Response.Cookies.Append(nameof(AuthenticationCookie.State), state, new CookieOptions
        {
            Domain = Authentication.CookieDomain,
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.None,
        });

        return Ok(identityRedirect);
    }

    [HttpGet("verifyEmail")]
    public IActionResult VerifyEmail(string identityPath, string email)
    {
        var uri = $"{Servers.Identity}{identityPath}?email={email}&redirectUri={Authentication.RedirectUri}";

        var identityRedirect = new IdentityRedirect
        {
            Uri = uri
        };

        return Ok(identityRedirect);
    }

    [HttpGet("stateValidate")]
    public IActionResult StateValidate(string state)
    {
        if (!HttpContext.Request.Cookies.TryGetValue(nameof(AuthenticationCookie.State), out var stateValue))
        {
            return BadRequest();
        }

        HttpContext.Response.Cookies.Delete(nameof(AuthenticationCookie.State), new CookieOptions
        {
            Domain = Authentication.CookieDomain,
            Path = "/",
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.None,
        });

        if (stateValue == state)
        {
            return Ok();
        }

        return BadRequest();
    }
}
