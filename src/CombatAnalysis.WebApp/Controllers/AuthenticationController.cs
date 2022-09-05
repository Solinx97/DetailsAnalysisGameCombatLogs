using CombatAnalysis.DAL.Data;
using CombatAnalysis.Identity.Interfaces;
using CombatAnalysis.Identity.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CombatAnalysis.WebApp.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IIdentityTokenService _tokenService;
        private readonly CombatAnalysisContext _dbContext;

        public AuthenticationController(IIdentityTokenService tokenService, CombatAnalysisContext dbContext)
        {
            _tokenService = tokenService;
            _dbContext = dbContext;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> Refresh()
        {
            if (!HttpContext.Request.Cookies.TryGetValue("refreshToken", out var refreshToken))
            {
                return Unauthorized();
            }

            var claims = _tokenService.ValidateToken(refreshToken, JWTSecret.RefreshSecretKey, out var validateToken);
            if (!claims.Any())
            {
                return Unauthorized();
            }

            var foundToken = await _tokenService.FindRefreshTokenAsync(refreshToken);
            if (foundToken == null)
            {
                Response.Cookies.Delete("accessToken");
                Response.Cookies.Delete("refreshToken");

                return Unauthorized();
            }

            var isExpiresed = DateTimeOffset.Now.UtcDateTime > validateToken.ValidTo;
            if (isExpiresed)
            {
                Response.Cookies.Delete("accessToken");
                Response.Cookies.Delete("refreshToken");
                await _tokenService.RemoveRefreshTokenAsync(foundToken);

                return Unauthorized();
            }

            var isAuthenticated = HttpContext.User.Identity.IsAuthenticated;
            if (!isAuthenticated)
            {
                return Unauthorized();
            }

            var email = HttpContext.User.Identity.Name;
            var user = await _dbContext.User.FirstOrDefaultAsync(u => u.Email == email);

            await _tokenService.CheckRefreshTokensByUserAsync(user.Id);

            if (!HttpContext.Request.Cookies.TryGetValue("accessToken", out var accessToken))
            {
                await _tokenService.GenerateTokensAsync(HttpContext.Response.Cookies, user.Id);
            }

            return Ok(user);
        }
    }
}
