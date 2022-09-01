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
    public class HealDoneGeneralController : ControllerBase
    {
        private readonly IHttpClientHelper _httpClient;

        public HealDoneGeneralController(IHttpClientHelper httpClient)
        {
            _httpClient = httpClient;
        }

        [HttpGet("{id}")]
        public async Task<IEnumerable<HealDoneGeneralModel>> GetById(int id)
        {
            var responseMessage = await _httpClient.GetAsync($"HealDoneGeneral/FindByCombatPlayerId/{id}");
            var healDoneGenerals = await responseMessage.Content.ReadFromJsonAsync<IEnumerable<HealDoneGeneralModel>>();

            return healDoneGenerals;
        }
    }
}
