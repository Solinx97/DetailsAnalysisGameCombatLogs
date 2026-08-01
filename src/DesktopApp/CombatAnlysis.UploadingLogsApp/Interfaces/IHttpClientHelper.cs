using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;

namespace CombatAnalysis.UploadingLogsApp.Interfaces;

public interface IHttpClientHelper
{
    public string BaseAddress { get; set; }

    public string BaseAddressApi { get; set; }

    HttpClient Client { get; set; }

    Task<HttpResponseMessage> PostAsync(string requestAddress, JsonContent content, CancellationToken cancellationToken);

    Task<HttpResponseMessage> PostAsync(string requestUri, StringContent content, CancellationToken cancellationToken);

    Task<HttpResponseMessage> GetAsync(string requestAddress, CancellationToken cancellationToken);

    Task<HttpResponseMessage> PutAsync(string requestAddress, JsonContent content, CancellationToken cancellationToken);

    Task<HttpResponseMessage> PatchAsync(string requestUri, JsonContent content, CancellationToken cancellationToken);

    Task<HttpResponseMessage> DeletAsync(string requestAddress, CancellationToken cancellationToken);
}
