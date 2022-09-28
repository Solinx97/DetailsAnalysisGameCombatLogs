using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace CombatAnalysis.Core.Interfaces
{
    public interface IHttpClientHelper
    {
        HttpClient Client { get; set; }

        public string BaseAddress { get; set; }

        Task<HttpResponseMessage> PostAsync(string requestAddress, JsonContent content);

        Task<HttpResponseMessage> GetAsync(string requestAddress);

        Task<HttpResponseMessage> PutAsync(string requestAddress, JsonContent content);

        Task<HttpResponseMessage> DeletAsync(string requestAddress);
    }
}
