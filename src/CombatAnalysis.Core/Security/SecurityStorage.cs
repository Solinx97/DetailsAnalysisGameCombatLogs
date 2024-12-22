using CombatAnalysis.Core.Consts;
using CombatAnalysis.Core.Enums;
using CombatAnalysis.Core.Extensions;
using CombatAnalysis.Core.Helpers;
using CombatAnalysis.Core.Interfaces;
using CombatAnalysis.Core.Models.User;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System.Net.Http.Json;

namespace CombatAnalysis.Core.Security;

internal class SecurityStorage
{
    private readonly ILogger _logger;
    private readonly IMemoryCache _memoryCache;
    private readonly IHttpClientHelper _httpClient;

    public SecurityStorage(IMemoryCache memoryCache, IHttpClientHelper httpClient, ILogger logger)
    {
        _memoryCache = memoryCache;
        _httpClient = httpClient;
        _logger = logger;
    }

    private static readonly string _directoryPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "CombatAnalysis");
    private static readonly string _refreshTokenFilePath = Path.Combine(_directoryPath, "refreshToken.dat");
    private static readonly string _accessTokenFilePath = Path.Combine(_directoryPath, "accessToken.dat");

    public void SaveTokens(string refreshToken, string accessToken)
    {
        try
        {
            if (!Directory.Exists(_directoryPath))
            {
                Directory.CreateDirectory(_directoryPath);
            }

            var encryptedRefreshToken = AESEncryption.EncryptStringToBytes(refreshToken);
            File.WriteAllBytes(_refreshTokenFilePath, encryptedRefreshToken);

            var encryptedAccessToken = AESEncryption.EncryptStringToBytes(accessToken);
            File.WriteAllBytes(_accessTokenFilePath, encryptedAccessToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
        }
    }

    public void RemoveTokens()
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

    public async Task<AppUserModel?> GetUserAsync()
    {
        try
        {
            if (!File.Exists(_refreshTokenFilePath))
            {
                return null;
            }

            var encryptedData = File.ReadAllBytes(_refreshTokenFilePath);
            var decryptedData = AESEncryption.DecryptStringFromBytes(encryptedData);
            _memoryCache.Set(nameof(MemoryCacheValue.RefreshToken), decryptedData, new MemoryCacheEntryOptions { Size = 10 });

            encryptedData = File.ReadAllBytes(_accessTokenFilePath);
            decryptedData = AESEncryption.DecryptStringFromBytes(encryptedData);
            _memoryCache.Set(nameof(MemoryCacheValue.AccessToken), decryptedData, new MemoryCacheEntryOptions { Size = 10 });

            var user = await GetUserByAccessTokenAsync(decryptedData);
            _memoryCache.Set(nameof(MemoryCacheValue.User), user, new MemoryCacheEntryOptions { Size = 50 });

            return user;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);

            return null;
        }
    }

    private async Task<AppUserModel?> GetUserByAccessTokenAsync(string accessToken)
    {
        try
        {
            var identityUserId = AccessTokenHelper.GetUserIdFromToken(accessToken);
            if (identityUserId == null)
            {
                throw new ArgumentNullException(nameof(identityUserId));
            }

            var response = await _httpClient.GetAsync($"Account/find/{identityUserId}", accessToken, Port.UserApi);
            response.EnsureSuccessStatusCode();

            var user = await response.Content.ReadFromJsonAsync<AppUserModel>();
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            return user;
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, ex.Message);

            return null;
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "HTTP request error: {Message}", ex.Message);

            return null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);

            return null;
        }
    }
}
