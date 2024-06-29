using CombatAnalysis.Identity.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CombatAnalysisIdentity.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class TokenController : ControllerBase
{
    private readonly IIdentityTokenService _tokenService;

    public TokenController(IIdentityTokenService tokenService)
    {
        _tokenService = tokenService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAccessToken(string grantType, string clientId, string codeVerifier, string code, string redirectUri)
    {
        if (string.IsNullOrEmpty(grantType) || string.IsNullOrEmpty(code))
        {
            return BadRequest();
        }

        var refreshToken = await _tokenService.GenerateTokensAsync(string.Empty);

        return Ok(refreshToken);
    }
}
