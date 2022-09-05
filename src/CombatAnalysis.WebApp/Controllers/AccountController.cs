using CombatAnalysis.DAL.Data;
using CombatAnalysis.DAL.Entities.User;
using CombatAnalysis.Identity.Interfaces;
using CombatAnalysis.WebApp.Models.User;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CombatAnalysis.WebApp.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly CombatAnalysisContext _dbContext;
        private readonly IIdentityTokenService _tokenService;

        public AccountController(CombatAnalysisContext dbContext, IIdentityTokenService tokenService)
        {
            _dbContext = dbContext;
            _tokenService = tokenService;
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model)
        {
            var user = await _dbContext.User.FirstOrDefaultAsync(u => u.Email == model.Email && u.Password == model.Password);
            if (user != null)
            {
                await Authenticate(user.Email);
                await _tokenService.GenerateTokensAsync(HttpContext.Response.Cookies, user.Id);

                return Ok(user);
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPost("registration")]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            var user = await _dbContext.User.FirstOrDefaultAsync(u => u.Email == model.Email);
            if (user == null)
            {
                var newUser = new User { Id = Guid.NewGuid().ToString(), Email = model.Email, Password = model.Password };
                _dbContext.User.Add(newUser);
                await _dbContext.SaveChangesAsync();

                await Authenticate(model.Email);
                await _tokenService.GenerateTokensAsync(HttpContext.Response.Cookies, newUser.Id);

                return Ok(user);
            }
            else
            {
                return BadRequest();
            }
        }


        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            if (Request.Cookies.TryGetValue("refreshToken", out var refreshToken))
            {
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

                HttpContext.Response.Cookies.Delete("accessToken");
                HttpContext.Response.Cookies.Delete("refreshToken");

                var refreshTokenModel = await _tokenService.FindRefreshTokenAsync(refreshToken);
                await _tokenService.RemoveRefreshTokenAsync(refreshTokenModel);

                return Ok();
            }

            return Unauthorized();
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
