using CombatAnalysis.EnhancedWebApp.Server.Consts;
using CombatAnalysis.EnhancedWebApp.Server.Interfaces;
using CombatAnalysis.EnhancedWebApp.Server.Models.GameLogs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace CombatAnalysis.EnhancedWebApp.Server.Controllers.GameLogs;

[Route("api/v1/[controller]")]
[ApiController]
public class UnitHealthController : ControllerBase
{
    private readonly IHttpClientHelper _httpClient;

    public UnitHealthController(IOptions<Cluster> cluster, IHttpClientHelper httpClient)
    {
        _httpClient = httpClient;
        _httpClient.APIUrl = cluster.Value.CombatParser;
    }

    [HttpGet("getByCombatId/{combatId:int:min(1)}")]
    public async Task<IActionResult> GetByCombatId(int combatId)
    {
        var responseMessage = await _httpClient.GetAsync($"UnitHealth/getByCombatId/{combatId}");
        var unitsHealth = await responseMessage.Content.ReadFromJsonAsync<IDictionary<string, IEnumerable<UnitHealthModel>>>();

        return Ok(unitsHealth);
    }
}
