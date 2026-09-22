using CombatAnalysis.EnhancedWebApp.Server.Interfaces;

namespace CombatAnalysis.EnhancedWebApp.Server.Helpers;

internal class HttpClientHelper : IHttpClientHelper
{
    private const string _baseAddressApi = "api/v1/";

    private readonly HttpClient _client;

    public HttpClientHelper()
    {
        _client = new HttpClient();
    }

    public string APIUrl { get; set; } = string.Empty;

    public string BaseAddressApi { get; set; } = _baseAddressApi;

    public void AddAuthorizationHeader(string scheme, string parameter)
    {
        _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(scheme, parameter);
    }

    public async Task<HttpResponseMessage> PostAsync(string requestUri, JsonContent? content)
    {
        var result = await _client.PostAsync($"{APIUrl}{BaseAddressApi}{requestUri}", content);

        return result;
    }

    public async Task<HttpResponseMessage> PostAsync(string requestUri, StringContent? content)
    {
        var result = await _client.PostAsync($"{APIUrl}{BaseAddressApi}{requestUri}", content);

        return result;
    }

    public async Task<HttpResponseMessage> GetAsync(string requestUri)
    {
        var result = await _client.GetAsync($"{APIUrl}{BaseAddressApi}{requestUri}");

        return result;
    }

    public async Task<HttpResponseMessage> GetAsync(string requestUri, CancellationToken cancellationToken)
    {
        var result = await _client.GetAsync($"{APIUrl}{BaseAddressApi}{requestUri}", cancellationToken);

        return result;
    }

    public async Task<HttpResponseMessage> PutAsync(string requestUri, JsonContent content)
    {
        var result = await _client.PutAsync($"{APIUrl}{BaseAddressApi}{requestUri}", content);

        return result;
    }

    public async Task<HttpResponseMessage> PatchAsync(string requestUri, JsonContent content)
    {
        var result = await _client.PatchAsync($"{APIUrl}{BaseAddressApi}{requestUri}", content);

        return result;
    }

    public async Task<HttpResponseMessage> DeletAsync(string requestUri)
    {
        var result = await _client.DeleteAsync($"{APIUrl}{BaseAddressApi}{requestUri}");

        return result;
    }
}
