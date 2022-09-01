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
    public class ResourceRecoveryGeneralController : ControllerBase
    {
        private readonly IHttpClientHelper _httpClient;

        public ResourceRecoveryGeneralController(IHttpClientHelper httpClient)
        {
            _httpClient = httpClient;
        }

        [HttpGet("{id}")]
        public async Task<IEnumerable<ResourceRecoveryGeneralModel>> GetById(int id)
        {
            var responseMessage = await _httpClient.GetAsync($"ResourceRecoveryGeneral/FindByCombatPlayerId/{id}");
            var resourceRecoveryGenerals = await responseMessage.Content.ReadFromJsonAsync<IEnumerable<ResourceRecoveryGeneralModel>>();

            return resourceRecoveryGenerals;
        }
    }
}
