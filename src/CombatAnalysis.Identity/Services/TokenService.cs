using AutoMapper;
using CombatAnalysis.DAL.Entities.Authentication;
using CombatAnalysis.DAL.Interfaces;
using CombatAnalysis.Identity.DTO;
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
    private readonly ITokenRepository<RefreshToken> _refreshTokenRepository;
    private readonly ITokenRepository<AccessToken> _accessTokenrepository;
    private readonly IJWTSecret _jwtSecretService;
    private readonly ILogger<TokenService> _logger;
    private readonly IMapper _mapper;
    private readonly TokenSettings _tokenSettings;

    private const int AccessExpiresTimeInMinutes = 30;
    private const int RefreshExpiresTimeInHours = 12;

    public TokenService(IOptions<TokenSettings> settings, ITokenRepository<RefreshToken> refreshTokenRepository,
        ITokenRepository<AccessToken> accessTokenrepository, IMapper mapper, 
        ILogger<TokenService> logger, IJWTSecret jwtSecretService)
    {
        _refreshTokenRepository = refreshTokenRepository;
        _accessTokenrepository = accessTokenrepository;
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

        var accessToken = GenerateToken(secret.AccessSecret);
        var refreshToken = GenerateToken(secret.RefreshSecret);

        await SaveAccessTokenAsync(accessToken, userId);
        await SaveRefreshTokenAsync(refreshToken, userId);

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

    async Task<RefreshTokenDto> IIdentityTokenService.FindRefreshTokenAsync(string refreshToken)
    {
        var token = await _refreshTokenRepository.GetByTokenAsync(refreshToken);
        var map = _mapper.Map<RefreshTokenDto>(token);

        return map;
    }

    async Task IIdentityTokenService.CheckRefreshTokensByUserAsync(string userId)
    {
        var tokens = await _refreshTokenRepository.GetAllByUserAsync(userId);
        foreach (var token in tokens)
        {
            var map = _mapper.Map<RefreshTokenDto>(token);
            if (DateTimeOffset.Now.UtcDateTime > token.Expires)
            {
                await RemoveRefreshTokenAsync(map);
            }
        }
    }

    public async Task<int> RemoveRefreshTokenAsync(RefreshTokenDto refreshToken)
    {
        var map = _mapper.Map<RefreshToken>(refreshToken);
        var result = await _refreshTokenRepository.DeleteAsync(map);

        return result;
    }

    private string GenerateToken(string accessKey)
    {
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Issuer = _tokenSettings.Issuer,
            Audience = _tokenSettings.Audience,
            Expires = DateTime.UtcNow.AddMinutes(AccessExpiresTimeInMinutes),
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(accessKey)),
                SecurityAlgorithms.HmacSha256),
        };

        var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();

        var securityToken = jwtSecurityTokenHandler.CreateJwtSecurityToken(tokenDescriptor);
        var token = jwtSecurityTokenHandler.WriteToken(securityToken);

        return token;
    }

    private async Task SaveRefreshTokenAsync(string refreshToken, string userId)
    {
        await RemoveUserRefreshTokensAsync(userId);
        var refreshTokenModel = new RefreshToken
        {   
            Id = Guid.NewGuid().ToString(),
            Token = refreshToken,
            Expires = DateTimeOffset.UtcNow.AddHours(RefreshExpiresTimeInHours).UtcDateTime,
            UserId = userId
        };

        await _refreshTokenRepository.CreateAsync(refreshTokenModel);
    }

    private async Task SaveAccessTokenAsync(string accessToken, string userId)
    {
        await RemoveUserAccessTokensAsync(userId);
        var refreshTokenModel = new AccessToken
        {
            Id = Guid.NewGuid().ToString(),
            Token = accessToken,
            Expires = DateTimeOffset.UtcNow.AddHours(RefreshExpiresTimeInHours).UtcDateTime,
            UserId = userId
        };

        await _accessTokenrepository.CreateAsync(refreshTokenModel);
    }

    private async Task RemoveUserRefreshTokensAsync(string userId)
    {
        var tokens = await _refreshTokenRepository.GetAllByUserAsync(userId);
        if (tokens == null)
        {
            return;
        }

        foreach (var token in tokens)
        {
            await _refreshTokenRepository.DeleteAsync(token);
        }
    }

    private async Task RemoveUserAccessTokensAsync(string userId)
    {
        var tokens = await _accessTokenrepository.GetAllByUserAsync(userId);
        if (tokens == null)
        {
            return;
        }

        foreach (var token in tokens)
        {
            await _accessTokenrepository.DeleteAsync(token);
        }
    }
}
