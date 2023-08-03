using CombatAnalysis.Identity.Interfaces;
using CombatAnalysis.Identity.Services;
using Microsoft.AspNetCore.Mvc;

namespace CombatAnalysis.UserApi.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class AuthenticationController : ControllerBase
{
    private readonly IIdentityTokenService _tokenService;

    public AuthenticationController(IIdentityTokenService tokenService)
    {
        _tokenService = tokenService;
    }

    [HttpGet("validateRefreshToken/{refreshToken}")]
    public IActionResult ValidateToken(string refreshToken)
    {
        //var claimsByRefreshToken = _tokenService.ValidateToken(refreshToken, JWTSecretService.RefreshSecretKey, out var _);
        //if (!claimsByRefreshToken.Any())
        //{
        //    return BadRequest();
        //}
        //else
        //{
        //    return Ok();
        //}

        return Ok();
    }

    [HttpGet("validateAccessToken/{accessToken}")]
    public IActionResult ValidateAccessToken(string accessToken)
    {
        //var claimsByRefreshToken = _tokenService.ValidateToken(accessToken, JWTSecretService.AccessSecretKey, out var _);
        //if (!claimsByRefreshToken.Any())
        //{
        //    return BadRequest();
        //}
        //else
        //{
        //    return Ok();
        //}
        return Ok();
    }

    [HttpGet("find/{refreshToken}")]
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

    [HttpGet("check/{userId}")]
    public async Task Check(string userId)
    {
        await _tokenService.CheckRefreshTokensByUserAsync(userId);
    }
}
