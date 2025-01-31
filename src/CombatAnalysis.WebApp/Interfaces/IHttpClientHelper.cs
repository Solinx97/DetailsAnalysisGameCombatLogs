namespace CombatAnalysis.WebApp.Interfaces;

public interface IHttpClientHelper
{
    string APIUrl { get; set; }

    void AddAuthorizationHeader(string scheme, string parameter);

    Task<HttpResponseMessage> PostAsync(string requestAddress, JsonContent content);

    Task<HttpResponseMessage> GetAsync(string requestAddress);

    Task<HttpResponseMessage> PutAsync(string requestAddress, JsonContent content);

    Task<HttpResponseMessage> DeletAsync(string requestAddress);
}
