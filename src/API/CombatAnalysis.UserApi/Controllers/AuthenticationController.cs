using CombatAnalysis.Identity.Interfaces;
using CombatAnalysis.Identity.Security;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace CombatAnalysis.UserApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IIdentityTokenService _tokenService;

        public AuthenticationController(IIdentityTokenService tokenService)
        {
            _tokenService = tokenService;
        }

        [HttpGet]
        [Route("validateRefreshToken/{refreshToken}")]
        public IActionResult ValidateToken(string refreshToken)
        {
            var claimsByRefreshToken = _tokenService.ValidateToken(refreshToken, JWTSecret.RefreshSecretKey, out var _);
            if (!claimsByRefreshToken.Any())
            {
                return BadRequest();
            }
            else
            {
                return Ok();
            }
        }

        [HttpGet]
        [Route("validateAccessToken/{accessToken}")]
        public IActionResult ValidateAccessToken(string accessToken)
        {
            var claimsByRefreshToken = _tokenService.ValidateToken(accessToken, JWTSecret.AccessSecretKey, out var _);
            if (!claimsByRefreshToken.Any())
            {
                return BadRequest();
            }
            else
            {
                return Ok();
            }
        }

        [HttpGet]
        [Route("find/{refreshToken}")]
        public async Task<IActionResult> FindRefreshTokenAsync(string refreshToken)
        {
            var foundToken = await _tokenService.FindRefreshTokenAsync(refreshToken);
            if (foundToken == null)
            {
                return Unauthorized();
            }
            else
            {
                return Ok();
            }
        }

        [HttpGet]
        [Route("check/{userId}")]
        public async Task Check(string userId)
        {
            await _tokenService.CheckRefreshTokensByUserAsync(userId);
        }
    }
}
