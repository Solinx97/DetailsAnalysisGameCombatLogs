using CombatAnalysis.WebApp.Consts;
using CombatAnalysis.WebApp.Interfaces;
using CombatAnalysis.WebApp.Models.Response;
using CombatAnalysis.WebApp.Models.User;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CombatAnalysis.WebApp.Controllers
{
    [Route("[controller]")]
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
            var responseMessage = await _httpClient.PostAsync("Account", JsonContent.Create(model));
            if (responseMessage.StatusCode == System.Net.HttpStatusCode.OK)
            {

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
            else
            {
                return BadRequest();
            }
        }

        [HttpPost("registration")]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            var responseMessage = await _httpClient.PostAsync("Account/registration", JsonContent.Create(model));
            if (responseMessage.StatusCode == System.Net.HttpStatusCode.OK)
            {

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
            else
            {
                return BadRequest();
            }
        }


        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
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
}
