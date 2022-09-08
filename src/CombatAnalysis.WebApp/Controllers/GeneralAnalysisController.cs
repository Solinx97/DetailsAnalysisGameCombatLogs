using CombatAnalysis.WebApp.Consts;
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
            _httpClient.BaseAddress = Port.CombatParserApi;
        }

        [HttpGet("{id}")]
        public async Task<IEnumerable<CombatModel>> GetById(int id)
        {
            var responseMessage = await _httpClient.GetAsync($"Combat/FindByCombatLogId/{id}");
            var combats = await responseMessage.Content.ReadFromJsonAsync<IEnumerable<CombatModel>>();

            return combats;
        }
    }
}
