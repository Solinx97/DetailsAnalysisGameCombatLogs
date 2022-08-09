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
    public class DetailsSpecificalCombatController : ControllerBase
    {
        private readonly IHttpClientHelper _httpClient;

        public DetailsSpecificalCombatController(IHttpClientHelper httpClient)
        {
            _httpClient = httpClient;
        }

        [HttpGet("{id}")]
        public async Task<IEnumerable<CombatPlayerDataModel>> GetById(int id)
        {
            var responseMessage = await _httpClient.GetAsync($"CombatPlayer/FindByCombatId/{id}");
            var combatPlayers = await responseMessage.Content.ReadFromJsonAsync<IEnumerable<CombatPlayerDataModel>>();

            return combatPlayers;
        }
    }
}
