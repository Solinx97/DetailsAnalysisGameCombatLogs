using CombatAnalysis.Hubs.Consts;
using CombatAnalysis.Hubs.Enums;
using CombatAnalysis.Hubs.Interfaces;
using System.Net.Http.Headers;

namespace CombatAnalysis.Hubs.Helpers;

internal class HttpClientHelper : IHttpClientHelper
{
    private const string _baseAddressApi = "api/v1/";

    private readonly HttpClient _client;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public HttpClientHelper(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;

        _client = new HttpClient {
            BaseAddress = new Uri($"{Cluster.Chat}{_baseAddressApi}")
        };
    }

    public async Task<HttpResponseMessage> PostAsync(string requestUri, JsonContent content)
    {
        AddAuthorizationHeader();

        var responseMessage = await _client.PostAsync(requestUri, content);

        return responseMessage;
    }

    public async Task<HttpResponseMessage> GetAsync(string requestUri)
    {
        AddAuthorizationHeader();

        var responseMessage = await _client.GetAsync(requestUri);

        return responseMessage;
    }

    public async Task<HttpResponseMessage> PutAsync(string requestUri, JsonContent content)
    {
        AddAuthorizationHeader();

        var responseMessage = await _client.PutAsync(requestUri, content);

        return responseMessage;
    }

    public async Task<HttpResponseMessage> DeletAsync(string requestUri)
    {
        AddAuthorizationHeader();

        var responseMessage = await _client.DeleteAsync(requestUri);

        return responseMessage;
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

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
    }
}
