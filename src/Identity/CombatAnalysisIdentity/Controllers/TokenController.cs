using CombatAnalysis.Identity.DTO;
using CombatAnalysis.Identity.Interfaces;
using CombatAnalysis.Identity.Security;
using CombatAnalysisIdentity.Consts;
using Microsoft.AspNetCore.Mvc;

namespace CombatAnalysisIdentity.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class TokenController(IOAuthCodeFlowService oAuthCodeFlowService, ILogger<TokenController> logger) : ControllerBase
{
    private readonly IOAuthCodeFlowService _oAuthCodeFlowService = oAuthCodeFlowService;
    private readonly ILogger<TokenController> _logger = logger;

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

            var decodedAuthorizationCode = Uri.UnescapeDataString(code).Replace(' ', '+');
            var codeChallengeValidated = await _oAuthCodeFlowService.ValidateCodeChallengeAsync(clientId, codeVerifier, decodedAuthorizationCode, redirectUri);
            if (!codeChallengeValidated)
            {
                return BadRequest();
            }

            var (authorizationCode, userId) = _oAuthCodeFlowService.DecryptAuthorizationCode(decodedAuthorizationCode, Authentication.IssuerSigningKey);

            var token = await GenerateTokenAsync(clientId, userId);

            return Ok(token);
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, ex.Message);

            return BadRequest(ex.Message);
        }
        catch (ArgumentException ex)
        {
            _logger.LogError(ex, ex.Message);

            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);

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

            var token = GenerateNewTokenWhenRefresh(clientId, userId, refreshToken);

            return Ok(token);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while refresh access token");

            return BadRequest(ex.Message);
        }
    }

    private async Task<AccessTokenDto> GenerateTokenAsync(string clientId, string userId)
    {
        var accessToken = _oAuthCodeFlowService.GenerateToken(clientId, userId);
        var refreshToken = _oAuthCodeFlowService.GenerateToken(clientId);

        await _oAuthCodeFlowService.SaveRefreshTokenAsync(refreshToken, Authentication.RefreshTokenExpiresDays, clientId, userId);

        var token = new AccessTokenDto
        {
            AccessToken = accessToken,
            TokenType = "Bearer",
            Expires = DateTimeOffset.UtcNow.AddMinutes(Authentication.AccessTokenExpiresMins),
            RefreshToken = refreshToken
        };

        return token;
    }

    private AccessTokenDto GenerateNewTokenWhenRefresh(string clientId, string userId, string refreshToken)
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
