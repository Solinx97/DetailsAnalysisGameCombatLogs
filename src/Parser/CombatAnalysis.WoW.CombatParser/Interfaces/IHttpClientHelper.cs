using System.Net.Http.Json;

namespace CombatAnalysis.WoW.CombatParser.Interfaces;

public interface IHttpClientHelper
{
    string BaseAddress { get; set; }

    string BaseAddressApi { get; set; }

    void AddAuthorizationHeader(string scheme, string parameter);

    Task<HttpResponseMessage> PostAsync(string requestAddress, JsonContent content);

    Task<HttpResponseMessage> PostAsync(string requestUri, StringContent content);

    Task<HttpResponseMessage> GetAsync(string requestAddress);

    Task<HttpResponseMessage> PutAsync(string requestAddress, JsonContent content);

    Task<HttpResponseMessage> PatchAsync(string requestUri, JsonContent content);

    Task<HttpResponseMessage> DeletAsync(string requestAddress);
}
