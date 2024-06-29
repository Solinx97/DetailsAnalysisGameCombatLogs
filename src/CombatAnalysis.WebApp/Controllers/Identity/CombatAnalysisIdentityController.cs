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
        try
        {
            var url = $"Token?grantType=authorization_code&clientId=clientId&codeVerifier={codeVerifier}&code={authorizationCode}&redirectUri=https://localhost:44479/";

            var responseMessage = await _httpClient.GetAsync(url, Port.Identity);
            if (responseMessage.StatusCode == System.Net.HttpStatusCode.InternalServerError)
            {
                return StatusCode(500);
            }

            if (!responseMessage.IsSuccessStatusCode)
            {
                return BadRequest();
            }
        }
        catch (Exception ex)
        {
            const string message = "Error while handling authorization response";
            var t = 123;
        }


        //var response = await responseMessage.Content.ReadFromJsonAsync<ResponseFromAccount>();
        //HttpContext.Response.Cookies.Append("refreshToken", response.RefreshToken, new CookieOptions
        //{
        //    HttpOnly = true,
        //    Secure = true,
        //    SameSite = SameSiteMode.Lax,
        //    Expires = DateTimeOffset.UtcNow.AddMinutes(TokenExpires.RefreshExpiresTimeInMinutes),
        //});

        var dontLogoutValue = HttpContext.Request.Cookies?["dontLogout"];
        //await Authenticate(response.User.Email, dontLogoutValue == "true");

        return Ok();
    }
}
