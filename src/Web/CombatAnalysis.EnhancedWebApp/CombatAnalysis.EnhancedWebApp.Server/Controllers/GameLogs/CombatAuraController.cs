using CombatAnalysis.EnhancedWebApp.Server.Consts;
using CombatAnalysis.EnhancedWebApp.Server.Interfaces;
using CombatAnalysis.EnhancedWebApp.Server.Models.GameLogs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace CombatAnalysis.EnhancedWebApp.Server.Controllers.GameLogs;

[Route("api/v1/[controller]")]
[ApiController]
public class CombatAuraController : ControllerBase
{
    private readonly IHttpClientHelper _httpClient;

    public CombatAuraController(IOptions<Cluster> cluster, IHttpClientHelper httpClient)
    {
        _httpClient = httpClient;
        _httpClient.APIUrl = cluster.Value.CombatParser;
    }

    [HttpGet("getByCombatId/{combatId:int:min(1)}")]
    public async Task<IActionResult> GetByCombatId(int combatId)
    {
        var responseMessage = await _httpClient.GetAsync($"CombatAura/getByCombatId/{combatId}");
        var combatAuras = await responseMessage.Content.ReadFromJsonAsync<IEnumerable<CombatAuraModel>>();

        return Ok(combatAuras);
    }

    [HttpGet("{id:int:min(1)}")]
    public async Task<IActionResult> GetById(int id)
    {
        var responseMessage = await _httpClient.GetAsync($"CombatAura/{id}");
        var combatAura = await responseMessage.Content.ReadFromJsonAsync<CombatAuraModel>();

        return Ok(combatAura);
    }
}
