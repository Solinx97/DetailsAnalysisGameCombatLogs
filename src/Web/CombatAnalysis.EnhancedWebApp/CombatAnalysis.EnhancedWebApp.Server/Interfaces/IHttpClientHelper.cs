namespace CombatAnalysis.EnhancedWebApp.Server.Interfaces;

public interface IHttpClientHelper
{
    string APIUrl { get; set; }

    string BaseAddressApi { get; set; }

    void AddAuthorizationHeader(string scheme, string parameter);

    Task<HttpResponseMessage> SendAsync(HttpRequestMessage message);

    Task<HttpResponseMessage> PostAsync(string requestAddress, JsonContent? content);

    Task<HttpResponseMessage> PostAsync(string requestUri, StringContent? content);

    Task<HttpResponseMessage> PostAsync(string requestUri, FormUrlEncodedContent? content);

    Task<HttpResponseMessage> GetAsync(string requestAddress);

    Task<HttpResponseMessage> GetAsync(string requestAddress, CancellationToken cancellationToken);

    Task<HttpResponseMessage> PutAsync(string requestAddress, JsonContent content);

    Task<HttpResponseMessage> PatchAsync(string requestUri, JsonContent content);

    Task<HttpResponseMessage> DeletAsync(string requestAddress);
}
