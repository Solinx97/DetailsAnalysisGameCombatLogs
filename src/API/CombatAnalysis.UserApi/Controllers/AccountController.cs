using CombatAnalysis.DAL.Data;
using CombatAnalysis.DAL.Entities.User;
using CombatAnalysis.Identity.Interfaces;
using CombatAnalysis.UserApi.Models;
using CombatAnalysis.UserApi.Models.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace CombatAnalysis.UserApi.Controllers
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
                var tokens = await _tokenService.GenerateTokensAsync(HttpContext.Response.Cookies, user.Id);
                var response = new ResponseFromAccount(user, tokens.Item1, tokens.Item2);

                return Ok(response);
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

                var tokens = await _tokenService.GenerateTokensAsync(HttpContext.Response.Cookies, newUser.Id);
                var response = new ResponseFromAccount(newUser, tokens.Item1, tokens.Item2);

                return Ok(response);
            }
            else
            {
                return BadRequest();
            }
        }


        [HttpGet("logout/{refreshToken}")]
        public async Task<IActionResult> Logout(string refreshToken)
        {
            var refreshTokenModel = await _tokenService.FindRefreshTokenAsync(refreshToken);
            await _tokenService.RemoveRefreshTokenAsync(refreshTokenModel);

            return Ok();
        }
    }
}
