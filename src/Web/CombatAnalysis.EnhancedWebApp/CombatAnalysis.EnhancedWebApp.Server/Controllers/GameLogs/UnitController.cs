using CombatAnalysis.EnhancedWebApp.Server.Consts;
using CombatAnalysis.EnhancedWebApp.Server.Interfaces;
using CombatAnalysis.EnhancedWebApp.Server.Models.GameLogs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace CombatAnalysis.EnhancedWebApp.Server.Controllers.GameLogs;

[Route("api/v1/[controller]")]
[ApiController]
public class UnitController : ControllerBase
{
    private readonly IHttpClientHelper _httpClient;

    public UnitController(IOptions<Cluster> cluster, IHttpClientHelper httpClient)
    {
        _httpClient = httpClient;
        _httpClient.APIUrl = cluster.Value.CombatParser;
    }

    [HttpGet("getByCombatId/{combatId:int:min(0)}")]
    public async Task<IActionResult> GetByCombatId(int combatId)
    {
        var responseMessage = await _httpClient.GetAsync($"Unit/getByCombatId/{combatId}");
        var units = await responseMessage.Content.ReadFromJsonAsync<IEnumerable<UnitModel>>();

        return Ok(units);
    }

    [HttpGet("getUniqueUnitNames/{combatLogId:int:min(0)}")]
    public async Task<IActionResult> GetUniqueUnitNames(int combatLogId, string bossName)
    {
        var responseMessage = await _httpClient.GetAsync($"Unit/getUniqueUnitNames/{combatLogId}?bossName={bossName}");
        var uniqueNames = await responseMessage.Content.ReadFromJsonAsync<IEnumerable<UniqueUnitNameModel>>();

        return Ok(uniqueNames);
    }
}
