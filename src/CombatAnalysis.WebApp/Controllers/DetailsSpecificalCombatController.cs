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

        [HttpGet("combatPlayersByCombatId/{id}")]
        public async Task<IEnumerable<CombatPlayerDataModel>> GetCombatPlayersByCombatId(int id)
        {
            var responseMessage = await _httpClient.GetAsync($"CombatPlayer/FindByCombatId/{id}");
            var combatPlayers = await responseMessage.Content.ReadFromJsonAsync<IEnumerable<CombatPlayerDataModel>>();

            return combatPlayers;
        }

        [HttpGet("combatPlayerById/{id}")]
        public async Task<CombatPlayerDataModel> GetCombatPlayerById(int id)
        {
            var responseMessage = await _httpClient.GetAsync($"CombatPlayer/{id}");
            var combatPlayer = await responseMessage.Content.ReadFromJsonAsync<CombatPlayerDataModel>();

            return combatPlayer;
        }

        [HttpGet("combatById/{id}")]
        public async Task<CombatModel> GetCombatById(int id)
        {
            var responseMessage = await _httpClient.GetAsync($"Combat/{id}");
            var combat = await responseMessage.Content.ReadFromJsonAsync<CombatModel>();

            return combat;
        }
    }
}
