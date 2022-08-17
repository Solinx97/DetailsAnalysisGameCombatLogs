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
    public class DamageTakenController : ControllerBase
    {
        private readonly IHttpClientHelper _httpClient;

        public DamageTakenController(IHttpClientHelper httpClient)
        {
            _httpClient = httpClient;
        }

        [HttpGet("{id}")]
        public async Task<IEnumerable<DamageTakenGeneralModel>> GetById(int id)
        {
            var responseMessage = await _httpClient.GetAsync($"DamageTakenGeneral/FindByCombatPlayerId/{id}");
            var damageTakenGeneral = await responseMessage.Content.ReadFromJsonAsync<IEnumerable<DamageTakenGeneralModel>>();

            return damageTakenGeneral;
        }
    }
}
