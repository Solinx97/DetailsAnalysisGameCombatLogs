using CombatAnalysis.WebApp.Attributes;
using CombatAnalysis.WebApp.Consts;
using CombatAnalysis.WebApp.Enums;
using CombatAnalysis.WebApp.Extensions;
using CombatAnalysis.WebApp.Interfaces;
using CombatAnalysis.WebApp.Models.Response;
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
    }

    [RequireAccessToken]
    [HttpPut]
    public async Task<IActionResult> Edit(AppUserModel model)
    {
        if (!Request.Cookies.TryGetValue(AuthenticationCookie.AccessToken.ToString(), out var refreshToken))
        {
            return Unauthorized();
        }

        var responseMessage = await _httpClient.PutAsync("Account", JsonContent.Create(model), refreshToken, Port.UserApi);
        if (responseMessage.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            return Unauthorized();
        }
        else if (responseMessage.IsSuccessStatusCode)
        {
            var response = await responseMessage.Content.ReadFromJsonAsync<ResponseFromAccount>();
            return Ok(response.User);
        }

        return BadRequest();
    }

    [RequireAccessToken]
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        if (!Request.Cookies.TryGetValue(AuthenticationCookie.AccessToken.ToString(), out var accessToken))
        {
            return Unauthorized();
        }

        var responseMessage = await _httpClient.GetAsync("Account", accessToken, Port.UserApi);
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

    [RequireAccessToken]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(string id)
    {
        if (!Request.Cookies.TryGetValue(AuthenticationCookie.AccessToken.ToString(), out var accessToken))
        {
            return Unauthorized();
        }

        var responseMessage = await _httpClient.GetAsync($"Account/{id}", accessToken, Port.UserApi);
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

    [RequireAccessToken]
    [HttpGet("find/{identityUserId}")]
    public async Task<IActionResult> FindByIdentityUserId(string identityUserId)
    {
        if (!Request.Cookies.TryGetValue(AuthenticationCookie.AccessToken.ToString(), out var accessToken))
        {
            return Unauthorized();
        }

        var responseMessage = await _httpClient.GetAsync($"Account/find/{identityUserId}", accessToken, Port.UserApi);
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
        var responseMessage = await _httpClient.GetAsync($"Account/checkIfUserExist/{email}", Port.UserApi);
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
        HttpContext.Response.Cookies.Delete(AuthenticationCookie.RefreshToken.ToString());
        HttpContext.Response.Cookies.Delete(AuthenticationCookie.AccessToken.ToString());

        return Ok();
    }
}
