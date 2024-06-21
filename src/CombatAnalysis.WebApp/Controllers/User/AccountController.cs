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
        if (!responseMessage.IsSuccessStatusCode)
        {
            return BadRequest();
        }

        var response = await responseMessage.Content.ReadFromJsonAsync<ResponseFromAccount>();
        HttpContext.Response.Cookies.Append("refreshToken", response.RefreshToken, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Lax,
            Expires = DateTimeOffset.UtcNow.AddMinutes(TokenExpires.RefreshExpiresTimeInMinutes),
        });

        var dontLogoutValue = HttpContext.Request.Cookies?["dontLogout"];
        await Authenticate(response.User.Email, dontLogoutValue == "true");

        return Ok(response.User);
    }

    [HttpPost("registration")]
    public async Task<IActionResult> Register(RegisterModel model)
    {
        var responseMessage = await _httpClient.PostAsync("Account/registration", JsonContent.Create(model), Port.UserApi);
        if (!responseMessage.IsSuccessStatusCode)
        {
            return BadRequest();
        }

        var login = new LoginModel { Email = model.Email, Password = model.Password };
        return await Login(login);
    }

    [HttpPut]
    public async Task<IActionResult> Edit(AppUserModel model)
    {
        if (!HttpContext.Request.Cookies.TryGetValue("refreshToken", out var refreshToken))
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

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        if (!HttpContext.Request.Cookies.TryGetValue("refreshToken", out var refreshToken))
        {
            return Unauthorized();
        }

        var responseMessage = await _httpClient.GetAsync("Account", refreshToken, Port.UserApi);
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

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(string id)
    {
        if (!HttpContext.Request.Cookies.TryGetValue("refreshToken", out var refreshToken))
        {
            return Unauthorized();
        }

        var responseMessage = await _httpClient.GetAsync($"Account/{id}", refreshToken, Port.UserApi);
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
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        HttpContext.Response.Cookies.Delete("refreshToken");

        return Ok();
    }

    private async Task Authenticate(string email, bool dontLogout)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimsIdentity.DefaultNameClaimType, email)
        };

        var claimsIdentity = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(claimsIdentity),
            new AuthenticationProperties
            {
                IsPersistent = dontLogout,
                ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(TokenExpires.RefreshExpiresTimeInMinutes)
            });
    }
}
