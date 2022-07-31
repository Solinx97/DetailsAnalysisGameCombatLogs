using CombatAnalysis.WebApp.Interfaces;
using CombatAnalysis.WebApp.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace CombatAnalysis.WebApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GeneralAnalysisController : ControllerBase
    {
        private readonly IHttpClientHelper _httpClient;

        public GeneralAnalysisController(IHttpClientHelper httpClient)
        {
            _httpClient = httpClient;
        }

        [HttpGet]
        public async Task<IEnumerable<CombatModel>> Get()
        {
            var responseMessage = await _httpClient.GetAsync("Combat/FindByCombatLogId/1");
            var combats = await responseMessage.Content.ReadFromJsonAsync<IEnumerable<CombatModel>>();

            return combats;
        }
    }
}
