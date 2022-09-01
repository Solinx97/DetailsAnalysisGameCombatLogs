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
    public class DamageDoneController : ControllerBase
    {
        private readonly IHttpClientHelper _httpClient;

        public DamageDoneController(IHttpClientHelper httpClient)
        {
            _httpClient = httpClient;
        }

        [HttpGet("{id}")]
        public async Task<IEnumerable<DamageDoneModel>> GetById(int id)
        {
            var responseMessage = await _httpClient.GetAsync($"DamageDone/FindByCombatPlayerId/{id}");
            var damageDones = await responseMessage.Content.ReadFromJsonAsync<IEnumerable<DamageDoneModel>>();

            return damageDones;
        }
    }
}
