using CombatAnalysis.Core.Interfaces;
using System.Net.Http.Json;

namespace CombatAnalysis.Core.Helpers;

internal class HttpClientHelper : IHttpClientHelper
{
    public HttpClientHelper()
    {
        Client = new HttpClient();
    }

    public HttpClient Client { get; set; }

    public string BaseAddress { get; set; }

    public async Task<HttpResponseMessage> PostAsync(string requestUri, JsonContent content)
    {
        var result = await Client.PostAsync($"{BaseAddress}{requestUri}", content);

        return result;
    }

    public async Task<HttpResponseMessage> GetAsync(string requestUri)
    {
        var result = await Client.GetAsync($"{BaseAddress}{requestUri}");

        return result;
    }

    public async Task<HttpResponseMessage> PutAsync(string requestUri, JsonContent content)
    {
        var result = await Client.PutAsync($"{BaseAddress}{requestUri}", content);

        return result;
    }

    public async Task<HttpResponseMessage> DeletAsync(string requestUri)
    {
        var result = await Client.DeleteAsync($"{BaseAddress}{requestUri}");

        return result;
    }
}
