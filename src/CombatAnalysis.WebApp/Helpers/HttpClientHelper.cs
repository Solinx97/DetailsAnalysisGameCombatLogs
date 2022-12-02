using CombatAnalysis.WebApp.Interfaces;

namespace CombatAnalysis.WebApp.Helpers;

internal class HttpClientHelper : IHttpClientHelper
{
    private const string BaseAddressApi = "api/v1/";

    public HttpClientHelper()
    {
        Client = new HttpClient();
    }

    public HttpClient Client { get; set; }

    public string BaseAddress { get; set; }

    public async Task<HttpResponseMessage> PostAsync(string requestUri, JsonContent content)
    {
        var result = await Client.PostAsync($"{BaseAddress}{BaseAddressApi}{requestUri}", content);

        return result;
    }

    public async Task<HttpResponseMessage> GetAsync(string requestUri)
    {
        var result = await Client.GetAsync($"{BaseAddress}{BaseAddressApi}{requestUri}");

        return result;
    }

    public async Task<HttpResponseMessage> PutAsync(string requestUri, JsonContent content)
    {
        var result = await Client.PutAsync($"{BaseAddress}{BaseAddressApi}{requestUri}", content);

        return result;
    }

    public async Task<HttpResponseMessage> DeletAsync(string requestUri)
    {
        var result = await Client.DeleteAsync($"{BaseAddress}{BaseAddressApi}{requestUri}");

        return result;
    }
}
