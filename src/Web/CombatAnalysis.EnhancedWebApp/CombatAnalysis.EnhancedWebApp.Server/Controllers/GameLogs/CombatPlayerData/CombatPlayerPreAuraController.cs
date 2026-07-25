using CombatAnalysis.EnhancedWebApp.Server.Consts;
using CombatAnalysis.EnhancedWebApp.Server.Interfaces;
using CombatAnalysis.EnhancedWebApp.Server.Models.GameLogs.CombatPlayerData;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace CombatAnalysis.EnhancedWebApp.Server.Controllers.GameLogs.CombatPlayerData;

[Route("api/v1/[controller]")]
[ApiController]
public class CombatPlayerPreAuraController : ControllerBase
{
    private readonly IHttpClientHelper _httpClient;

    public CombatPlayerPreAuraController(IOptions<Cluster> cluster, IHttpClientHelper httpClient)
    {
        _httpClient = httpClient;
        _httpClient.APIUrl = cluster.Value.CombatParser;
    }

    [HttpGet("getByCombatId")]
    public async Task<IActionResult> GetByCombatId(int combatId, int combatPlayerId)
    {
        var responseMessage = await _httpClient.GetAsync($"CombatPlayerPreAura/getByCombatId?combatId={combatId}&combatPlayerId={combatPlayerId}");
        var preAuras = await responseMessage.Content.ReadFromJsonAsync<IEnumerable<CombatPlayerPreAuraModel>>();

        return Ok(preAuras);
    }
}
