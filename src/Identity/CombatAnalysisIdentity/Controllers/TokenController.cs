using CombatAnalysis.Identity.DTO;
using CombatAnalysis.Identity.Interfaces;
using CombatAnalysis.Identity.Security;
using CombatAnalysisIdentity.Consts;
using Microsoft.AspNetCore.Mvc;

namespace CombatAnalysisIdentity.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class TokenController : ControllerBase
{
    private readonly IOAuthCodeFlowService _oAuthCodeFlowService;
    private readonly ILogger<TokenController> _logger;

    public TokenController(IOAuthCodeFlowService oAuthCodeFlowService, ILogger<TokenController> logger)
    {
        _oAuthCodeFlowService = oAuthCodeFlowService;
        _logger = logger;
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

            if (!grantType.Equals(AuthenticationGrantType.Authorization))
            {
                return BadRequest();
            }

            var codeChallengeValidated = await _oAuthCodeFlowService.ValidateCodeChallengeAsync(clientId, codeVerifier, code, redirectUri);
            if (!codeChallengeValidated)
            {
                return BadRequest();
            }

            var (authorizationCode, userId) = _oAuthCodeFlowService.DecryptAuthorizationCode(code, Authentication.IssuerSigningKey);

            var token = GenerateToken(clientId, userId);

            return Ok(token);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while getting access token");

            return BadRequest(ex.Message);
        }
    }

    [HttpGet("refresh")]
    public async Task<IActionResult> RefreshAccessToken(string grantType, string refreshToken, string clientId)
    {
        try
        {
            if (string.IsNullOrEmpty(grantType)
                || string.IsNullOrEmpty(refreshToken)
                || string.IsNullOrEmpty(clientId))
            {
                return BadRequest();
            }

            if (!grantType.Equals(AuthenticationGrantType.RefreshToken))
            {
                return BadRequest();
            }

            var userId = await _oAuthCodeFlowService.ValidateRefreshTokenAsync(refreshToken, clientId);
            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest("Invalid refresh token");
            }

            var token = GenerateAccessToken(clientId, userId, refreshToken);

            return Ok(token);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while refresh access token");

            return BadRequest(ex.Message);
        }
    }

    private AccessTokenDto GenerateToken(string clientId, string userId)
    {
        var accessToken = _oAuthCodeFlowService.GenerateToken(clientId, userId);
        var refreshToken = _oAuthCodeFlowService.GenerateToken(clientId);
        _oAuthCodeFlowService.CreateRefreshTokenAsync(refreshToken, Authentication.RefreshTokenExpiresDays, clientId, userId);

        var token = new AccessTokenDto
        {
            AccessToken = accessToken,
            TokenType = "Bearer",
            Expires = DateTimeOffset.UtcNow.AddMinutes(Authentication.AccessTokenExpiresMins),
            RefreshToken = refreshToken
        };

        return token;
    }

    private AccessTokenDto GenerateAccessToken(string clientId, string userId, string refreshToken)
    {
        var accessToken = _oAuthCodeFlowService.GenerateToken(clientId, userId);

        var token = new AccessTokenDto
        {
            AccessToken = accessToken,
            TokenType = "Bearer",
            Expires = DateTimeOffset.UtcNow.AddMinutes(Authentication.AccessTokenExpiresMins),
            RefreshToken = refreshToken
        };

        return token;
    }
}
