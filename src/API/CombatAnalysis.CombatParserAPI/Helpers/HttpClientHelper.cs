﻿using CombatAnalysis.CombatParserAPI.Interfaces;

namespace CombatAnalysis.CombatParserAPI.Helpers;

internal class HttpClientHelper : IHttpClientHelper
{
    private const string BaseAddressApi = "api/v1/";

    public HttpClientHelper()
    {
        Client = new HttpClient
        {
            BaseAddress = new Uri("https://localhost:5001")
        };
    }

    public HttpClient Client { get; set; }

    public async Task<HttpResponseMessage> PostAsync(string requestUri, JsonContent content)
    {
        var result = await Client.PostAsync(BaseAddressApi + requestUri, content);

        return result;
    }

    public async Task<HttpResponseMessage> GetAsync(string requestUri)
    {
        var result = await Client.GetAsync(BaseAddressApi + requestUri);

        return result;
    }

    public async Task<HttpResponseMessage> PutAsync(string requestUri, JsonContent content)
    {
        var result = await Client.PutAsync(BaseAddressApi + requestUri, content);

        return result;
    }

    public async Task<HttpResponseMessage> DeletAsync(string requestUri)
    {
        var result = await Client.DeleteAsync(BaseAddressApi + requestUri);

        return result;
    }
}
