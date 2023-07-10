namespace CombatAnalysis.WebApp.Interfaces
{
    public interface IHttpClientHelper
    {
        public string BaseAddressApi { get; }

        HttpClient Client { get; set; }

        string BaseAddress { get; set; }

        Task<HttpResponseMessage> PostAsync(string requestAddress, JsonContent content);

        Task<HttpResponseMessage> GetAsync(string requestAddress);

        Task<HttpResponseMessage> PutAsync(string requestAddress, JsonContent content);

        Task<HttpResponseMessage> DeletAsync(string requestAddress);
    }
}
