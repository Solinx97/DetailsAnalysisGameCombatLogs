using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace CombatAnalysis.WebApp.Interfaces
{
    public interface IHttpClientHelper
    {
        HttpClient Client { get; set; }

        string BaseAddress { get; set; }

        Task<HttpResponseMessage> PostAsync(string requestAddress, JsonContent content);

        Task<HttpResponseMessage> GetAsync(string requestAddress);

        Task<HttpResponseMessage> PutAsync(string requestAddress, JsonContent content);

        Task<HttpResponseMessage> DeletAsync(string requestAddress);
    }
}
