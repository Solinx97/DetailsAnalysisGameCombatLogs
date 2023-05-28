using CombatAnalysis.BL.Interfaces;
using CombatAnalysis.WebApp.Consts;
using CombatAnalysis.WebApp.Interfaces;
using CombatAnalysis.WebApp.Models.Response;
using CombatAnalysis.WebApp.Models.User;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CombatAnalysis.WebApp.Controllers.User;

[Route("api/v1/[controller]")]
[ApiController]
public class AccountController : ControllerBase
{
    private readonly IHttpClientHelper _httpClient;

    public AccountController(IHttpClientHelper httpClient)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = Port.UserApi;
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginModel model)
    {
        _httpClient.BaseAddress = Port.UserApi;

        var responseMessage = await _httpClient.PostAsync("Account", JsonContent.Create(model));
        if (responseMessage.StatusCode != System.Net.HttpStatusCode.OK)
        {
            return NotFound();
        }

        var response = await responseMessage.Content.ReadFromJsonAsync<ResponseFromAccount>();
        HttpContext.Response.Cookies.Append("accessToken", response.AccessToken, new CookieOptions
        {
            HttpOnly = true,
            Expires = DateTimeOffset.UtcNow.AddMinutes(TokenExpires.AccessExpiresTimeInMinutes),
        });
        HttpContext.Response.Cookies.Append("refreshToken", response.RefreshToken, new CookieOptions
        {
            HttpOnly = true,
            Expires = DateTimeOffset.UtcNow.AddHours(TokenExpires.RefreshExpiresTimeInHours),
        });

        await Authenticate(response.User.Email);

        return Ok(response.User);
    }

    [HttpPost("registration")]
    public async Task<IActionResult> Register(RegisterModel model)
    {
        _httpClient.BaseAddress = Port.UserApi;

        var responseMessage = await _httpClient.PostAsync("Account/registration", JsonContent.Create(model));
        if (responseMessage.StatusCode != System.Net.HttpStatusCode.OK)
        {
            return BadRequest();

        }

        if (responseMessage.Content.Headers.ContentLength == 0)
        {
            return Ok();
        }

        var response = await responseMessage.Content.ReadFromJsonAsync<ResponseFromAccount>();
        HttpContext.Response.Cookies.Append("accessToken", response.AccessToken, new CookieOptions
        {
            HttpOnly = true,
            Expires = DateTimeOffset.UtcNow.AddMinutes(TokenExpires.AccessExpiresTimeInMinutes),
        });
        HttpContext.Response.Cookies.Append("refreshToken", response.RefreshToken, new CookieOptions
        {
            HttpOnly = true,
            Expires = DateTimeOffset.UtcNow.AddHours(TokenExpires.RefreshExpiresTimeInHours),
        });

        await Authenticate(response.User.Email);

        return Ok(response.User);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var responseMessage = await _httpClient.GetAsync("Account");
        if (responseMessage.StatusCode == System.Net.HttpStatusCode.OK)
        {
            var users = await responseMessage.Content.ReadFromJsonAsync<IEnumerable<UserModel>>();
            return Ok(users);
        }

        return BadRequest();
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(string id)
    {
        var responseMessage = await _httpClient.GetAsync($"Account/{id}");
        if (responseMessage.StatusCode == System.Net.HttpStatusCode.OK)
        {
            var user = await responseMessage.Content.ReadFromJsonAsync<UserModel>();
            return Ok(user);
        }

        return BadRequest();
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        _httpClient.BaseAddress = Port.UserApi;

        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

        HttpContext.Response.Cookies.Delete("accessToken");
        HttpContext.Response.Cookies.Delete("refreshToken");

        if (Request.Cookies.TryGetValue("refreshToken", out var refreshToken))
        {
            await _httpClient.GetAsync($"Account/logout/{refreshToken}");
        }

        return Ok();
    }

    private async Task Authenticate(string email)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimsIdentity.DefaultNameClaimType, email)
        };

        var claimsIdentity = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
    }
}
