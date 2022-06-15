using CombatAnalysis.Core.Consts;
using CombatAnalysis.Core.Interfaces;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace CombatAnalysis.Core.Core
{
    internal class HttpClientHelper : IHttpClientHelper
    {
        public HttpClientHelper()
        {
            Client = new HttpClient
            {
                BaseAddress = new Uri(Port.CombatParserApi)
            };
        }

        public HttpClient Client { get; set; }

        public async Task<HttpResponseMessage> PostAsync(string requestUri, JsonContent content)
        {
            var result = await Client.PostAsync(requestUri, content);

            return result;
        }

        public async Task<HttpResponseMessage> GetAsync(string requestUri)
        {
            var result = await Client.GetAsync(requestUri);

            return result;
        }

        public async Task<HttpResponseMessage> PutAsync(string requestUri, JsonContent content)
        {
            var result = await Client.PutAsync(requestUri, content);

            return result;
        }

        public async Task<HttpResponseMessage> DeletAsync(string requestUri)
        {
            var result = await Client.DeleteAsync(requestUri);

            return result;
        }
    }
}
