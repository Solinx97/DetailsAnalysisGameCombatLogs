namespace CombatAnalysis.Hubs.Interfaces;

public interface IHttpClientHelper
{
    HttpClient Client { get; set; }

    string BaseAddress { get; set; }

    Task<HttpResponseMessage> PostAsync(string requestAddress, JsonContent content, HttpContext context);

    Task<HttpResponseMessage> GetAsync(string requestAddress, HttpContext context);

    Task<HttpResponseMessage> PutAsync(string requestAddress, JsonContent content, HttpContext context);

    Task<HttpResponseMessage> DeletAsync(string requestAddress, HttpContext context);
}
