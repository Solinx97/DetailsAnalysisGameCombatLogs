using CombatAnalysis.WebApp.Interfaces;

namespace CombatAnalysis.WebApp.Helpers;

internal class HttpClientHelper : IHttpClientHelper
{
    private const string _baseAddressApi = "api/v1/";

    private readonly HttpClient _client;

    public HttpClientHelper()
    {
        _client = new HttpClient();
    }

    public string APIUrl { get; set; } = string.Empty;

    public void AddAuthorizationHeader(string scheme, string parameter)
    {
        _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(scheme, parameter);
    }

    public async Task<HttpResponseMessage> PostAsync(string requestUri, JsonContent content)
    {
        var result = await _client.PostAsync($"{APIUrl}{_baseAddressApi}{requestUri}", content);

        return result;
    }

    public async Task<HttpResponseMessage> GetAsync(string requestUri)
    {
        var result = await _client.GetAsync($"{APIUrl}{_baseAddressApi}{requestUri}");

        return result;
    }

    public async Task<HttpResponseMessage> PutAsync(string requestUri, JsonContent content)
    {
        var result = await _client.PutAsync($"{APIUrl}{_baseAddressApi}{requestUri}", content);

        return result;
    }

    public async Task<HttpResponseMessage> DeletAsync(string requestUri)
    {
        var result = await _client.DeleteAsync($"{APIUrl}{_baseAddressApi}{requestUri}");

        return result;
    }
}
