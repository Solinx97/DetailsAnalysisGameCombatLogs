using System.Net.Http.Json;

namespace CombatAnalysis.Core.Interfaces;

public interface IHttpClientHelper
{
    public string BaseAddressApi { get; }

    HttpClient Client { get; set; }

    public string BaseAddress { get; set; }

    Task<HttpResponseMessage> PostAsync(string requestAddress, JsonContent content, CancellationToken cancellationToken);

    Task<HttpResponseMessage> GetAsync(string requestAddress, CancellationToken cancellationToken);

    Task<HttpResponseMessage> PutAsync(string requestAddress, JsonContent content, CancellationToken cancellationToken);

    Task<HttpResponseMessage> DeletAsync(string requestAddress, CancellationToken cancellationToken);
}
