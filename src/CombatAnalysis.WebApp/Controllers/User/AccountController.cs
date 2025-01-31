using CombatAnalysis.WebApp.Attributes;
using CombatAnalysis.WebApp.Consts;
using CombatAnalysis.WebApp.Enums;
using CombatAnalysis.WebApp.Interfaces;
using CombatAnalysis.WebApp.Models.User;
using Microsoft.AspNetCore.Mvc;

namespace CombatAnalysis.WebApp.Controllers.User;

[Route("api/v1/[controller]")]
[ApiController]
public class AccountController : ControllerBase
{
    private readonly IHttpClientHelper _httpClient;

    public AccountController(IHttpClientHelper httpClient)
    {
        _httpClient = httpClient;
        _httpClient.APIUrl = Cluster.User;
    }

    [ServiceFilter(typeof(RequireAccessTokenAttribute))]
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var responseMessage = await _httpClient.GetAsync("Account");
        if (responseMessage.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            return Unauthorized();
        }
        else if (responseMessage.IsSuccessStatusCode)
        {
            var users = await responseMessage.Content.ReadFromJsonAsync<IEnumerable<AppUserModel>>();

            return Ok(users);
        }

        return BadRequest();
    }

    [ServiceFilter(typeof(RequireAccessTokenAttribute))]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(string id)
    {
        var responseMessage = await _httpClient.GetAsync($"Account/{id}");
        if (responseMessage.StatusCode == System.Net.HttpStatusCode.NoContent)
        {
            return NoContent();
        }
        else if (responseMessage.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            return Unauthorized();
        }
        else if (responseMessage.IsSuccessStatusCode)
        {
            var user = await responseMessage.Content.ReadFromJsonAsync<AppUserModel>();

            return Ok(user);
        }

        return BadRequest();
    }

    [ServiceFilter(typeof(RequireAccessTokenAttribute))]
    [HttpGet("find/{identityUserId}")]
    public async Task<IActionResult> FindByIdentityUserId(string identityUserId)
    {
        var responseMessage = await _httpClient.GetAsync($"Account/find/{identityUserId}");
        if (responseMessage.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            return Unauthorized();
        }
        else if (responseMessage.IsSuccessStatusCode)
        {
            var user = await responseMessage.Content.ReadFromJsonAsync<AppUserModel>();

            return Ok(user);
        }

        return BadRequest();
    }

    [HttpGet("checkIfUserExist/{email}")]
    public async Task<IActionResult> CheckIfUserExist(string email)
    {
        var responseMessage = await _httpClient.GetAsync($"Account/checkIfUserExist/{email}");
        if (responseMessage.IsSuccessStatusCode)
        {
            var userIsExist = await responseMessage.Content.ReadFromJsonAsync<bool>();

            return Ok(userIsExist);
        }

        return BadRequest();
    }

    [HttpPost("logout")]
    public IActionResult Logout()
    {
        HttpContext.Response.Cookies.Delete(nameof(AuthenticationCookie.RefreshToken), new CookieOptions
        {
            Domain = Authentication.CookieDomain,
            Path = "/",
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.None,
        });
        HttpContext.Response.Cookies.Delete(nameof(AuthenticationCookie.AccessToken), new CookieOptions
        {
            Domain = Authentication.CookieDomain,
            Path = "/",
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.None,
        });

        return Ok();
    }

    [ServiceFilter(typeof(RequireRefreshTokenAttribute))]
    [HttpPut]
    public async Task<IActionResult> Update(AppUserModel model)
    {
        var responseMessage = await _httpClient.PutAsync("Account", JsonContent.Create(model));
        if (responseMessage.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            return Unauthorized();
        }
        else if (responseMessage.IsSuccessStatusCode)
        {
            var user = await responseMessage.Content.ReadFromJsonAsync<AppUserModel>();

            return Ok(user);
        }

        return BadRequest();
    }
}
