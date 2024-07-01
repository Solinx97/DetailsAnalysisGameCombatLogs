using CombatAnalysis.Identity.DTO;
using CombatAnalysis.Identity.Interfaces;
using CombatAnalysis.Identity.Security;
using Microsoft.AspNetCore.Mvc;
using System.Text;

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

            var codeChallengeValidated = await _oAuthCodeFlowService.ValidateCodeChallengeAsync(clientId, codeVerifier, code, redirectUri);
            if (!codeChallengeValidated)
            {
                return BadRequest();
            }

            var (authorizationCode, userId) = _oAuthCodeFlowService.DecryptAuthorizationCode(code, Encryption.EnctyptionKey);

            var token = GenerateAceessToken(clientId, userId);

            return Ok(token);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    private AccessTokenDto GenerateAceessToken(string clientId, string userId)
    {
        var refreshToken = _oAuthCodeFlowService.GenerateToken(clientId, userId);
        var accessToken = _oAuthCodeFlowService.GenerateToken(clientId, userId);

        var token = new AccessTokenDto
        {
            AccessToken = accessToken,
            TokenType = "Bearer",
            ExpiresInMinutes = 60,
            RefreshToken = refreshToken
        };

        return token;
    }
}
