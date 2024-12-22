using CombatAnalysis.Core.Interfaces;
using System.Net.Http.Json;

namespace CombatAnalysis.Core.Helpers;

internal class HttpClientHelper : IHttpClientHelper
{
    public HttpClientHelper()
    {
        Client = new HttpClient
        {
            Timeout = TimeSpan.FromSeconds(500),
        };

        BaseAddressApi = "api/v1/";
    }

    public string BaseAddressApi { get; }

    public HttpClient Client { get; set; }

    public string BaseAddress { get; set; } = string.Empty;

    public async Task<HttpResponseMessage> PostAsync(string requestUri, JsonContent content, CancellationToken cancellationToken)
    {
        var result = await Client.PostAsync($"{BaseAddress}{BaseAddressApi}{requestUri}", content, cancellationToken);

        return result;
    }

    public async Task<HttpResponseMessage> GetAsync(string requestUri, CancellationToken cancellationToken)
    {
        var result = await Client.GetAsync($"{BaseAddress}{BaseAddressApi}{requestUri}", cancellationToken);

        return result;
    }

    public async Task<HttpResponseMessage> PutAsync(string requestUri, JsonContent content, CancellationToken cancellationToken)
    {
        var result = await Client.PutAsync($"{BaseAddress}{BaseAddressApi}{requestUri}", content, cancellationToken);

        return result;
    }

    public async Task<HttpResponseMessage> DeletAsync(string requestUri, CancellationToken cancellationToken)
    {
        var result = await Client.DeleteAsync($"{BaseAddress}{BaseAddressApi}{requestUri}", cancellationToken);

        return result;
    }
}
