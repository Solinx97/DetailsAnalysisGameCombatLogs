using CombatAnalysis.EnhancedWebApp.Server.Consts;
using CombatAnalysis.EnhancedWebApp.Server.Interfaces;
using CombatAnalysis.EnhancedWebApp.Server.Models.GameLogs.CombatPlayerData;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace CombatAnalysis.EnhancedWebApp.Server.Controllers.GameLogs.CombatPlayerData;

[Route("api/v1/[controller]")]
[ApiController]
public class CombatPlayerAuraController : ControllerBase
{
    private readonly IHttpClientHelper _httpClient;

    public CombatPlayerAuraController(IOptions<Cluster> cluster, IHttpClientHelper httpClient)
    {
        _httpClient = httpClient;
        _httpClient.APIUrl = cluster.Value.CombatParser;
    }

    [HttpGet("getByCombatId")]
    public async Task<IActionResult> GetByCombatId(int combatId, int combatPlayerId)
    {
        var responseMessage = await _httpClient.GetAsync($"CombatPlayerAura/getByCombatId?combatId={combatId}&combatPlayerId={combatPlayerId}");
        var combatAuras = await responseMessage.Content.ReadFromJsonAsync<IEnumerable<CombatPlayerAuraModel>>();

        return Ok(combatAuras);
    }

    [HttpGet("{id:int:min(1)}")]
    public async Task<IActionResult> GetById(int id)
    {
        var responseMessage = await _httpClient.GetAsync($"CombatPlayerAura/{id}");
        var combatAura = await responseMessage.Content.ReadFromJsonAsync<CombatPlayerAuraModel>();

        return Ok(combatAura);
    }
}
