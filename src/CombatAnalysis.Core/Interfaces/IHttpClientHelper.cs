using System.Net.Http.Json;

namespace CombatAnalysis.Core.Interfaces;

public interface IHttpClientHelper
{
    public string BaseAddressApi { get; }

    HttpClient Client { get; set; }

    public string BaseAddress { get; set; }

    Task<HttpResponseMessage> PostAsync(string requestAddress, JsonContent content);

    Task<HttpResponseMessage> GetAsync(string requestAddress);

    Task<HttpResponseMessage> PutAsync(string requestAddress, JsonContent content);

    Task<HttpResponseMessage> DeletAsync(string requestAddress);
}
