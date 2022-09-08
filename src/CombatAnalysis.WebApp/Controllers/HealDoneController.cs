using CombatAnalysis.WebApp.Consts;
using CombatAnalysis.WebApp.Interfaces;
using CombatAnalysis.WebApp.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace CombatAnalysis.WebApp.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class HealDoneController : ControllerBase
    {
        private readonly IHttpClientHelper _httpClient;

        public HealDoneController(IHttpClientHelper httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = Port.CombatParserApi;
        }

        [HttpGet("{id}")]
        public async Task<IEnumerable<HealDoneModel>> GetById(int id)
        {
            var responseMessage = await _httpClient.GetAsync($"HealDone/FindByCombatPlayerId/{id}");
            var healDones = await responseMessage.Content.ReadFromJsonAsync<IEnumerable<HealDoneModel>>();

            return healDones;
        }
    }
}
