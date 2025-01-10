namespace CombatAnalysis.Hubs.Interfaces;

public interface IHttpClientHelper
{
    HttpClient Client { get; set; }

    string BaseAddress { get; set; }

    Task<HttpResponseMessage> PostAsync(string requestAddress, JsonContent content, string token);

    Task<HttpResponseMessage> GetAsync(string requestAddress);

    Task<HttpResponseMessage> PutAsync(string requestAddress, JsonContent content);

    Task<HttpResponseMessage> DeletAsync(string requestAddress);
}
