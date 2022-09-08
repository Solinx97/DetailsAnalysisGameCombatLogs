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
    public class MainInformationController : Controller
    {
        private readonly IHttpClientHelper _httpClient;

        public MainInformationController(IHttpClientHelper httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = Port.CombatParserApi;
        }

        [HttpGet]
        public async Task<IEnumerable<CombatLogModel>> Get()
        {
            var responseMessage = await _httpClient.GetAsync("CombatLog");
            var combatLogs = await responseMessage.Content.ReadFromJsonAsync<IEnumerable<CombatLogModel>>();

            return combatLogs;
        }
    }
}
