using CombatAnalysis.UploadingLogsApp.Enums;
using CombatAnalysis.UploadingLogsApp.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;

namespace CombatAnalysis.UploadingLogsApp.Extensions;

internal static class HttpClientHelperExtensions
{
    private static IMemoryCache? _memoryCache;

    public static void Initialize(IMemoryCache? cache)
    {
        _memoryCache = cache;
    }

    public static async Task<HttpResponseMessage> PostAsync(this IHttpClientHelper clientHelper, string requestUri, JsonContent content, CancellationToken cancellationToken, bool isAuth = false)
    {
        if (isAuth)
        {
            GetAccessToken(clientHelper);
        }

        var result = await clientHelper.Client.PostAsync($"{clientHelper.BaseAddress}{clientHelper.BaseAddressApi}{requestUri}", content, cancellationToken);

        return result;
    }

    public static async Task<HttpResponseMessage> PostAsync(this IHttpClientHelper clientHelper, string requestUri, StringContent content, string baseAddress, bool isAuth = false)
    {
        if (isAuth)
        {
            GetAccessToken(clientHelper);
        }

        var result = await clientHelper.Client.PostAsync($"{baseAddress}{clientHelper.BaseAddressApi}{requestUri}", content);

        return result;
    }

    public static async Task<HttpResponseMessage> GetAsync(this IHttpClientHelper clientHelper, string requestUri, string baseAddress, bool isAuth = false)
    {
        if (isAuth)
        {
            GetAccessToken(clientHelper);
        }

        var result = await clientHelper.Client.GetAsync($"{baseAddress}{clientHelper.BaseAddressApi}{requestUri}");

        return result;
    }

    public static async Task<HttpResponseMessage> GetAsync(this IHttpClientHelper clientHelper, string requestUri, string baseAddress, CancellationToken cancellationToken, bool isAuth = false)
    {
        if (isAuth)
        {
            GetAccessToken(clientHelper);
        }

        var result = await clientHelper.Client.GetAsync($"{baseAddress}{clientHelper.BaseAddressApi}{requestUri}", cancellationToken);

        return result;
    }

    public static async Task<HttpResponseMessage> PutAsync(this IHttpClientHelper clientHelper, string requestUri, JsonContent content, string baseAddress, bool isAuth = false)
    {
        if (isAuth)
        {
            GetAccessToken(clientHelper);
        }

        var result = await clientHelper.Client.PutAsync($"{baseAddress}{clientHelper.BaseAddressApi}{requestUri}", content);

        return result;
    }

    public static async Task<HttpResponseMessage> DeletAsync(this IHttpClientHelper clientHelper, string requestUri, string baseAddress, bool isAuth = false)
    {
        if (isAuth)
        {
            GetAccessToken(clientHelper);
        }

        var result = await clientHelper.Client.DeleteAsync($"{baseAddress}{clientHelper.BaseAddressApi}{requestUri}");

        return result;
    }

    private static void GetAccessToken(IHttpClientHelper clientHelper)
    {
        if (_memoryCache == null || !_memoryCache.TryGetValue<string>(nameof(MemoryCacheValue.RefreshToken), out _))
        {
            return;
        }

        if (_memoryCache != null && _memoryCache.TryGetValue<string>(nameof(MemoryCacheValue.AccessToken), out var accessToken))
        {
            clientHelper.Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        }
    }
}
