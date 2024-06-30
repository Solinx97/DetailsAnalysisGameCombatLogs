using CombatAnalysis.Identity.Interfaces;
using CombatAnalysis.Identity.Security;
using Microsoft.AspNetCore.Mvc;

namespace CombatAnalysisIdentity.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class TokenController : ControllerBase
{
    private readonly IOAuthCodeFlowService _oAuthCodeFlowService;

    public TokenController(IOAuthCodeFlowService oAuthCodeFlowService)
    {
        _oAuthCodeFlowService = oAuthCodeFlowService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAccessToken(string grantType, string clientId, string codeVerifier, string code, string redirectUri)
    {
        try
        {
            if (string.IsNullOrEmpty(grantType)
                || string.IsNullOrEmpty(clientId) 
                || string.IsNullOrEmpty(codeVerifier)
                || string.IsNullOrEmpty(code)
                || string.IsNullOrEmpty(redirectUri))
            {
                return BadRequest();
            }

            var codeChallengeValidated = await _oAuthCodeFlowService.ValidateCodeChallengeAsync(codeVerifier, code);
            if (!codeChallengeValidated)
            {
                return BadRequest();
            }

            var (authorizationCode, userId) = _oAuthCodeFlowService.DecryptAuthorizationCode(code, Encryption.EnctyptionKey);

            var refreshToken = _oAuthCodeFlowService.GenerateAccessToken(clientId, userId);

            return Ok(refreshToken);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
