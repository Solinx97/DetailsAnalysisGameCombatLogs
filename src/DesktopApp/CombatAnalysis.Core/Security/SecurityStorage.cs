using CombatAnalysis.Core.Consts;
using CombatAnalysis.Core.Core;
using CombatAnalysis.Core.Enums;
using CombatAnalysis.Core.Extensions;
using CombatAnalysis.Core.Helpers;
using CombatAnalysis.Core.Interfaces;
using CombatAnalysis.Core.Models.Identity;
using CombatAnalysis.Core.Models.User;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System.Net.Http.Json;

namespace CombatAnalysis.Core.Security;

internal class SecurityStorage
{
    private const string RefreshTokenSecret = "refresh.token";
    private const string AccessTokenSecret = "access.token";
    private const string RefreshTokenPurpose = "RefreshTokenProtector";
    private const string AccessTokenPurpose = "AccessTokenProtector";

    private readonly IMemoryCache _memoryCache;
    private readonly IHttpClientHelper _httpClient;
    private readonly ILogger _logger;
    private readonly IDataProtector _refreshTokenProtector;
    private readonly IDataProtector _accessTokenProtector;
    private static readonly string _directoryPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), AppInformation.Name);
    private static readonly string _refreshTokenFilePath = Path.Combine(_directoryPath, RefreshTokenSecret);
    private static readonly string _accessTokenFilePath = Path.Combine(_directoryPath, AccessTokenSecret);

    public SecurityStorage(IMemoryCache memoryCache, IHttpClientHelper httpClient, ILogger logger)
    {
        _memoryCache = memoryCache;
        _httpClient = httpClient;
        _logger = logger;

        var protectionProvider = DataProtectionProvider.Create(AppInformation.Name);
        _refreshTokenProtector = protectionProvider.CreateProtector(RefreshTokenPurpose, RefreshTokenSecret);
        _accessTokenProtector = protectionProvider.CreateProtector(AccessTokenPurpose, AccessTokenSecret);
    }

    public void SaveAccessToken(TokenResponseModel token)
    {
        try
        {
            if (!Directory.Exists(_directoryPath))
            {
                Directory.CreateDirectory(_directoryPath);
            }

            var encryptedRefreshToken = _refreshTokenProtector.Protect(token.RefreshToken);
            File.WriteAllText(_refreshTokenFilePath, encryptedRefreshToken);

            _memoryCache.Set(nameof(MemoryCacheValue.RefreshToken), token.RefreshToken, DateTimeOffset.UtcNow.AddHours(token.RefreshTokenExpiresInHours));

            var encryptedAccessToken = _accessTokenProtector.Protect(token.AccessToken);
            File.WriteAllText(_accessTokenFilePath, encryptedAccessToken);

            _memoryCache.Set(nameof(MemoryCacheValue.AccessToken), token.AccessToken, DateTimeOffset.UtcNow.AddHours(token.ExpiresInHours));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
        }
    }

    public void RemoveAccessToken()
    {
        try
        {
            if (File.Exists(_refreshTokenFilePath))
            {
                File.Delete(_refreshTokenFilePath);
            }

            if (File.Exists(_accessTokenFilePath))
            {
                File.Delete(_accessTokenFilePath);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
        }
    }

    public async Task<AppUserModel?> GetUserAsync(CancellationToken cancellationToken)
    {
        try
        {
            GetAccessToken();

            var user = await GetUserByAccessTokenAsync(cancellationToken);
            _memoryCache.Set(nameof(MemoryCacheValue.User), user, TimeSpan.FromDays(3));

            return user;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);

            return null;
        }
    }

    public void GetAccessToken()
    {
        try
        {
            if (!File.Exists(_accessTokenFilePath) || !File.Exists(_refreshTokenFilePath))
            {
                return;
            }

            var encryptedAccessToken = File.ReadAllText(_accessTokenFilePath);
            var decryptedAccessToken = _accessTokenProtector.Unprotect(encryptedAccessToken);
            _memoryCache.Set(nameof(MemoryCacheValue.AccessToken), decryptedAccessToken, DateTimeOffset.Now.AddMinutes(60));

            var encryptedRefreshToken = File.ReadAllText(_refreshTokenFilePath);
            var decryptedRefresahToken = _refreshTokenProtector.Unprotect(encryptedRefreshToken);
            _memoryCache.Set(nameof(MemoryCacheValue.RefreshToken), decryptedRefresahToken, DateTimeOffset.Now.AddDays(3));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
        }
    }

    private async Task<AppUserModel?> GetUserByAccessTokenAsync(CancellationToken cancellationToken)
    {
        try
        {
            if (!_memoryCache.TryGetValue<string>(nameof(MemoryCacheValue.AccessToken), out var accessToken))
            {
                ArgumentException.ThrowIfNullOrEmpty(accessToken, nameof(accessToken));
            }

            var identityUserId = AccessTokenHelper.GetUserIdFromToken(accessToken);
            ArgumentException.ThrowIfNullOrEmpty(identityUserId, nameof(identityUserId));

            _httpClient.BaseAddressApi = "api/v1/";
            var response = await _httpClient.GetAsync($"User/find/{identityUserId}", API.UserApi, cancellationToken, true);
            response.EnsureSuccessStatusCode();

            var user = await response.Content.ReadFromJsonAsync<AppUserModel>();
            ArgumentNullException.ThrowIfNull(user, nameof(user));

            return user;
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, ex.Message);

            return null;
        }
        catch (ArgumentException ex)
        {
            _logger.LogError(ex, ex.Message);

            return null;
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "HTTP request error: {Message}", ex.Message);

            return null;
        }
    }
}
