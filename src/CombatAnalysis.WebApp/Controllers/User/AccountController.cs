using CombatAnalysis.WebApp.Consts;
using CombatAnalysis.WebApp.Extensions;
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
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginModel model)
    {
        var responseMessage = await _httpClient.PostAsync("Account", JsonContent.Create(model), Port.UserApi);
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
        var responseMessage = await _httpClient.PostAsync("Account/registration", JsonContent.Create(model), Port.UserApi);
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

    [HttpPut]
    public async Task<IActionResult> Edit(AppUserModel model)
    {
        if (!HttpContext.Request.Cookies.TryGetValue("refreshToken", out var refreshToken))
        {
            return Unauthorized();
        }

        await _httpClient.PutAsync("Account", refreshToken, JsonContent.Create(model), Port.UserApi);

        return Ok();
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        if (!HttpContext.Request.Cookies.TryGetValue("refreshToken", out var refreshToken))
        {
            return Unauthorized();
        }

        var responseMessage = await _httpClient.GetAsync("Account", refreshToken, Port.UserApi);
        if (responseMessage.StatusCode == System.Net.HttpStatusCode.OK)
        {
            var users = await responseMessage.Content.ReadFromJsonAsync<IEnumerable<AppUserModel>>();
            return Ok(users);
        }

        return BadRequest();
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(string id)
    {
        if (!HttpContext.Request.Cookies.TryGetValue("refreshToken", out var refreshToken))
        {
            return Unauthorized();
        }

        var responseMessage = await _httpClient.GetAsync($"Account/{id}", refreshToken, Port.UserApi);
        if (responseMessage.StatusCode == System.Net.HttpStatusCode.OK)
        {
            var user = await responseMessage.Content.ReadFromJsonAsync<AppUserModel>();
            return Ok(user);
        }

        return BadRequest();
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

        if (Request.Cookies.TryGetValue("refreshToken", out var refreshToken))
        {
            var reponse = await _httpClient.GetAsync($"Account/logout/{refreshToken}", Port.UserApi);
            if (reponse.IsSuccessStatusCode)
            {
                HttpContext.Response.Cookies.Delete("accessToken");
                HttpContext.Response.Cookies.Delete("refreshToken");
            }
            else
            {
                return BadRequest();
            }
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
