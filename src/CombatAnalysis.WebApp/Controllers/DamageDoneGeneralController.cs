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
    public class DamageDoneGeneralController : ControllerBase
    {
        private readonly IHttpClientHelper _httpClient;

        public DamageDoneGeneralController(IHttpClientHelper httpClient)
        {
            _httpClient = httpClient;
        }

        [HttpGet("{id}")]
        public async Task<IEnumerable<DamageDoneGeneralModel>> GetById(int id)
        {
            var responseMessage = await _httpClient.GetAsync($"DamageDoneGeneral/FindByCombatPlayerId/{id}");
            var damageDoneGenerals = await responseMessage.Content.ReadFromJsonAsync<IEnumerable<DamageDoneGeneralModel>>();

            return damageDoneGenerals;
        }
    }
}
