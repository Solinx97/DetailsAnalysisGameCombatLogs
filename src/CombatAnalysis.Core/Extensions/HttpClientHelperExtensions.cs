using CombatAnalysis.Core.Interfaces;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace CombatAnalysis.Core.Extensions;

public static class HttpClientHelperExtensions
{
    public static async Task<HttpResponseMessage> PostAsync(this IHttpClientHelper clientHelper, string requestUri, JsonContent content, string baseAddress)
    {
        var result = await clientHelper.Client.PostAsync($"{baseAddress}{clientHelper.BaseAddressApi}{requestUri}", content);

        return result;
    }

    public static async Task<HttpResponseMessage> PostAsync(this IHttpClientHelper clientHelper, string requestUri, JsonContent content, string token, string baseAddress)
    {
        clientHelper.Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var result = await clientHelper.Client.PostAsync($"{baseAddress}{clientHelper.BaseAddressApi}{requestUri}", content);

        return result;
    }

    public static async Task<HttpResponseMessage> GetAsync(this IHttpClientHelper clientHelper, string requestUri, string baseAddress)
    {
        var result = await clientHelper.Client.GetAsync($"{baseAddress}{clientHelper.BaseAddressApi}{requestUri}");

        return result;
    }

    public static async Task<HttpResponseMessage> GetAsync(this IHttpClientHelper clientHelper, string requestUri, string token, string baseAddress)
    {
        clientHelper.Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var result = await clientHelper.Client.GetAsync($"{baseAddress}{clientHelper.BaseAddressApi}{requestUri}");

        return result;
    }

    public static async Task<HttpResponseMessage> PutAsync(this IHttpClientHelper clientHelper, string requestUri, JsonContent content, string baseAddress)
    {
        var result = await clientHelper.Client.PutAsync($"{baseAddress}{clientHelper.BaseAddressApi}{requestUri}", content);

        return result;
    }

    public static async Task<HttpResponseMessage> PutAsync(this IHttpClientHelper clientHelper, string requestUri, JsonContent content, string token, string baseAddress)
    {
        clientHelper.Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var result = await clientHelper.Client.PutAsync($"{baseAddress}{clientHelper.BaseAddressApi}{requestUri}", content);

        return result;
    }

    public static async Task<HttpResponseMessage> DeletAsync(this IHttpClientHelper clientHelper, string requestUri, string baseAddress)
    {
        var result = await clientHelper.Client.DeleteAsync($"{baseAddress}{clientHelper.BaseAddressApi}{requestUri}");

        return result;
    }

    public static async Task<HttpResponseMessage> DeletAsync(this IHttpClientHelper clientHelper, string requestUri, string token, string baseAddress)
    {
        clientHelper.Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var result = await clientHelper.Client.DeleteAsync($"{baseAddress}{clientHelper.BaseAddressApi}{requestUri}");

        return result;
    }
}
