using CombatAnalysis.Hubs.Consts;
using CombatAnalysis.Hubs.Enums;
using CombatAnalysis.Hubs.Interfaces;
using System.Net.Http.Headers;

namespace CombatAnalysis.Hubs.Helpers;

internal class HttpClientHelper : IHttpClientHelper
{
    private const string _baseAddressApi = "api/v1/";
    private readonly IHttpContextAccessor _httpContextAccessor;

    public HttpClientHelper(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;

        Client = new HttpClient();

    }

    public HttpClient Client { get; set; }

    public string BaseAddress { get; set; } = Port.ChatApi;

    public async Task<HttpResponseMessage> PostAsync(string requestUri, JsonContent content)
    {
        AddAuthorizationHeader();

        var result = await Client.PostAsync($"{BaseAddress}{_baseAddressApi}{requestUri}", content);

        return result;
    }

    public async Task<HttpResponseMessage> GetAsync(string requestUri)
    {
        AddAuthorizationHeader();

        var result = await Client.GetAsync($"{BaseAddress}{_baseAddressApi}{requestUri}");

        return result;
    }

    public async Task<HttpResponseMessage> PutAsync(string requestUri, JsonContent content)
    {
        AddAuthorizationHeader();

        var result = await Client.PutAsync($"{BaseAddress}{_baseAddressApi}{requestUri}", content);

        return result;
    }

    public async Task<HttpResponseMessage> DeletAsync(string requestUri)
    {
        AddAuthorizationHeader();

        var result = await Client.DeleteAsync($"{BaseAddress}{_baseAddressApi}{requestUri}");

        return result;
    }

    private void AddAuthorizationHeader()
    {
        var context = _httpContextAccessor.HttpContext;
        if (context == null)
        {
            throw new ArgumentNullException(nameof(context));
        }

        if (!context.Request.Cookies.TryGetValue(nameof(AuthenticationCookie.RefreshToken), out var _))
        {
            throw new UnauthorizedAccessException($"{nameof(AuthenticationCookie.RefreshToken)} token is missing.");
        }

        if (!context.Request.Cookies.TryGetValue(nameof(AuthenticationCookie.AccessToken), out var accessToken))
        {
            throw new UnauthorizedAccessException($"{nameof(AuthenticationCookie.AccessToken)} token is missing.");
        }

        context.Items[nameof(AuthenticationCookie.AccessToken)] = accessToken;
        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
    }
}
