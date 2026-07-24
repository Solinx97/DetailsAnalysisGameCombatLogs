using CombatAnalysis.EnhancedWebApp.Server.Consts;
using CombatAnalysis.EnhancedWebApp.Server.Interfaces;
using CombatAnalysis.EnhancedWebApp.Server.Models.GameLogs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace CombatAnalysis.EnhancedWebApp.Server.Controllers.GameLogs.CombatPlayerData;

[Route("api/v1/[controller]")]
[ApiController]
public class CombatPlayerPositionController : ControllerBase
{
    private readonly IHttpClientHelper _httpClient;

    public CombatPlayerPositionController(IOptions<Cluster> cluster, IHttpClientHelper httpClient)
    {
        _httpClient = httpClient;
        _httpClient.APIUrl = cluster.Value.CombatParser;
    }

    [HttpGet("getByCombatPlayerId/{combatPlayerId:int:min(1)}")]
    public async Task<IActionResult> GetByCombatId(int combatPlayerId)
    {
        var responseMessage = await _httpClient.GetAsync($"CombatPlayerPosition/getByCombatPlayerId/{combatPlayerId}");
        var positions = await responseMessage.Content.ReadFromJsonAsync<IEnumerable<CombatPlayerPositionModel>>();

        return Ok(positions);
    }
}
