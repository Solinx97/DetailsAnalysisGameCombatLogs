using CombatAnalysis.EnhancedWebApp.Server.Consts;
using CombatAnalysis.EnhancedWebApp.Server.Interfaces;
using CombatAnalysis.EnhancedWebApp.Server.Models.GameLogs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace CombatAnalysis.EnhancedWebApp.Server.Controllers.GameLogs;

[Route("api/v1/[controller]")]
[ApiController]
public class UnitCastController : ControllerBase
{
    private readonly IHttpClientHelper _httpClient;

    public UnitCastController(IOptions<Cluster> cluster, IHttpClientHelper httpClient)
    {
        _httpClient = httpClient;
        _httpClient.APIUrl = cluster.Value.CombatParser;
    }

    [HttpGet("getByCombatUnitId/{combatUnitId}")]
    public async Task<IActionResult> GetByCombatId(string combatUnitId)
    {
        var responseMessage = await _httpClient.GetAsync($"UnitCast/getByCombatId/{combatUnitId}");
        var casts = await responseMessage.Content.ReadFromJsonAsync<IDictionary<string, IEnumerable<UnitCastModel>>>();

        return Ok(casts);
    }
}
