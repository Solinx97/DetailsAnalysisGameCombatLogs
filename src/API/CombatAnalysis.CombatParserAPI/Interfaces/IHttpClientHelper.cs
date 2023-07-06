namespace CombatAnalysis.CombatParserAPI.Interfaces;

public interface IHttpClientHelper
{
    HttpClient Client { get; set; }

    Task<HttpResponseMessage> PostAsync(string requestAddress, JsonContent content);

    Task<HttpResponseMessage> GetAsync(string requestAddress);

    Task<HttpResponseMessage> PutAsync(string requestAddress, JsonContent content);

    Task<HttpResponseMessage> DeletAsync(string requestAddress);
}
