using AutoMapper;
using CombatAnalysis.Identity.Interfaces;
using CombatAnalysis.Identity.Settings;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CombatAnalysis.Identity.Services;

internal class TokenService : IIdentityTokenService
{
    private readonly IJWTSecret _jwtSecretService;
    private readonly ILogger<TokenService> _logger;
    private readonly IMapper _mapper;
    private readonly TokenSettings _tokenSettings;

    public TokenService(IOptions<TokenSettings> settings, IMapper mapper, 
        ILogger<TokenService> logger, IJWTSecret jwtSecretService)
    {
        _mapper = mapper;
        _tokenSettings = settings.Value;
        _logger = logger;
        _jwtSecretService = jwtSecretService;
    }

    async Task<Tuple<string, string>> IIdentityTokenService.GenerateTokensAsync(IResponseCookies cookies, string userId)
    {
        var secret = await _jwtSecretService.GetSecretAsync();
        if (secret == null)
        {
            return null;
        }

        var accessToken = GenerateToken(secret.AccessSecret, TokenExpires.AccessExpiresTimeInMinutes);
        var refreshToken = GenerateToken(secret.RefreshSecret, TokenExpires.RefreshExpiresTimeInMinutes);

        return new Tuple<string, string>(accessToken, refreshToken);
    }

    IEnumerable<Claim> IIdentityTokenService.ValidateToken(string token, string secretKey, out SecurityToken validatedToken)
    {
        IEnumerable<Claim> claims;
        validatedToken = null;
        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token,
                new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = _tokenSettings.Issuer,
                    ValidateAudience = true,
                    ValidAudience = _tokenSettings.Audience,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
                    ValidAlgorithms = new[] { SecurityAlgorithms.HmacSha256 },
                    ValidateLifetime = true,
                },
                out validatedToken);
            claims = principal.Claims;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error message: {ex.Message}, Method: {nameof(IIdentityTokenService.ValidateToken)}");
            claims = new List<Claim>();
        }

        return claims;
    }

    private string GenerateToken(string secretKey, double tokenExpiresInMinutes)
    {
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Issuer = _tokenSettings.Issuer,
            Audience = _tokenSettings.Audience,
            Expires = DateTime.UtcNow.AddMinutes(tokenExpiresInMinutes),
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
                SecurityAlgorithms.HmacSha256)
        };

        var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();

        var securityToken = jwtSecurityTokenHandler.CreateJwtSecurityToken(tokenDescriptor);
        var token = jwtSecurityTokenHandler.WriteToken(securityToken);

        return token;
    }
}
