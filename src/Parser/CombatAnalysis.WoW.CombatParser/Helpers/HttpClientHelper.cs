using CombatAnalysis.WoW.CombatParser.Interfaces;
using System.Net.Http.Json;

namespace CombatAnalysis.WoW.CombatParser.Helpers;

internal class HttpClientHelper : IHttpClientHelper
{
    private readonly HttpClient _client;

    public HttpClientHelper()
    {
        _client = new HttpClient();
    }

    public string BaseAddress { get; set; } = string.Empty;

    public string BaseAddressApi { get; set; } = "api/v1/";

    public void AddAuthorizationHeader(string scheme, string parameter)
    {
        _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(scheme, parameter);
    }

    public async Task<HttpResponseMessage> PostAsync(string requestUri, JsonContent content)
    {
        var result = await _client.PostAsync($"{BaseAddress}{BaseAddressApi}{requestUri}", content);

        return result;
    }

    public async Task<HttpResponseMessage> PostAsync(string requestUri, StringContent content)
    {
        var result = await _client.PostAsync($"{BaseAddress}{BaseAddressApi}{requestUri}", content);

        return result;
    }

    public async Task<HttpResponseMessage> GetAsync(string requestUri)
    {
        var result = await _client.GetAsync($"{BaseAddress}{BaseAddressApi}{requestUri}");

        return result;
    }

    public async Task<HttpResponseMessage> PutAsync(string requestUri, JsonContent content)
    {
        var result = await _client.PutAsync($"{BaseAddress}{BaseAddressApi}{requestUri}", content);

        return result;
    }

    public async Task<HttpResponseMessage> PatchAsync(string requestUri, JsonContent content)
    {
        var result = await _client.PatchAsync($"{BaseAddress}{BaseAddressApi}{requestUri}", content);

        return result;
    }

    public async Task<HttpResponseMessage> DeletAsync(string requestUri)
    {
        var result = await _client.DeleteAsync($"{BaseAddress}{BaseAddressApi}{requestUri}");

        return result;
    }
}
