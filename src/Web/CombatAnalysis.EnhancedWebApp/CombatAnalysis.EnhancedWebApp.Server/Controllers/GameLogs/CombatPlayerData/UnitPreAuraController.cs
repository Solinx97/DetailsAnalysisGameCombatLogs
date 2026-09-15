using CombatAnalysis.EnhancedWebApp.Server.Consts;
using CombatAnalysis.EnhancedWebApp.Server.Interfaces;
using CombatAnalysis.EnhancedWebApp.Server.Models.GameLogs.CombatPlayerData;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace CombatAnalysis.EnhancedWebApp.Server.Controllers.GameLogs.CombatPlayerData;

[Route("api/v1/[controller]")]
[ApiController]
public class UnitPreAuraController : ControllerBase
{
    private readonly IHttpClientHelper _httpClient;

    public UnitPreAuraController(IOptions<Cluster> cluster, IHttpClientHelper httpClient)
    {
        _httpClient = httpClient;
        _httpClient.APIUrl = cluster.Value.CombatParser;
    }

    [HttpGet("getByCombatId/{combatId:int:min(1)}")]
    public async Task<IActionResult> GetByCombatId(int combatId, string unitId)
    {
        var responseMessage = await _httpClient.GetAsync($"UnitPreAura/getByCombatId/{combatId}?unitId={unitId}");
        var preAuras = await responseMessage.Content.ReadFromJsonAsync<IEnumerable<UnitPreAuraModel>>();

        return Ok(preAuras);
    }
}
