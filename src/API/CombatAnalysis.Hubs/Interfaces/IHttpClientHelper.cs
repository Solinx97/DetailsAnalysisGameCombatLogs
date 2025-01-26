namespace CombatAnalysis.Hubs.Interfaces;

public interface IHttpClientHelper
{
    Task<HttpResponseMessage> PostAsync(string requestAddress, JsonContent content);

    Task<HttpResponseMessage> GetAsync(string requestAddress);

    Task<HttpResponseMessage> PutAsync(string requestAddress, JsonContent content);

    Task<HttpResponseMessage> DeletAsync(string requestAddress);
}
