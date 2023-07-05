using CombatAnalysis.WebApp.Consts;
using CombatAnalysis.WebApp.Interfaces;
using CombatAnalysis.WebApp.Models.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CombatAnalysis.WebApp.Controllers.User;

[Route("api/v1/[controller]")]
[ApiController]
public class AuthenticationController : ControllerBase
{
    private readonly IHttpClientHelper _httpClient;

    public AuthenticationController(IHttpClientHelper httpClient)
    {
        _httpClient = httpClient;
    }

    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> Refresh()
    {
        if (!HttpContext.Request.Cookies.TryGetValue("refreshToken", out var refreshToken))
        {
            return Unauthorized();
        }

        _httpClient.BaseAddress = Port.UserApi;
        var response = await _httpClient.GetAsync($"Authentication/validateRefreshToken/{refreshToken}");
        if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
        {
            Response.Cookies.Delete("accessToken");
            Response.Cookies.Delete("refreshToken");

            return Unauthorized();
        }

        _httpClient.BaseAddress = Port.UserApi;
        response = await _httpClient.GetAsync($"Authentication/find/{refreshToken}");
        if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            Response.Cookies.Delete("accessToken");
            Response.Cookies.Delete("refreshToken");

            return Unauthorized();
        }

        var email = HttpContext.User.Identity.Name;

        _httpClient.BaseAddress = Port.UserApi;
        var responseMessage = await _httpClient.GetAsync($"Account/find/{email}");
        if (responseMessage.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            return NotFound();
        }

        if (responseMessage.StatusCode == System.Net.HttpStatusCode.OK)
        {
            var user = await responseMessage.Content.ReadFromJsonAsync<AppUserModel>();
            if (user == null)
            {
                await _httpClient.GetAsync($"Authentication/check/{user.Id}");
            }

            return await CheckAccessToken(user);
        }


        return BadRequest();
    }

    private async Task<IActionResult> CheckAccessToken(AppUserModel user)
    {
        if (!HttpContext.Request.Cookies.TryGetValue("accessToken", out var accessToken))
        {
            Response.Cookies.Delete("refreshToken");

            return Unauthorized();
        }

        var response = await _httpClient.GetAsync($"Authentication/validateAccessToken/{accessToken}");
        if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
        {
            Response.Cookies.Delete("accessToken");
            Response.Cookies.Delete("refreshToken");

            return Unauthorized();
        }

        return Ok(user);
    }
}
