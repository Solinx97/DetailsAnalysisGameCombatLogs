using CombatAnalysis.EnhancedWebApp.Server.Consts;
using CombatAnalysis.EnhancedWebApp.Server.Interfaces;
using CombatAnalysis.EnhancedWebApp.Server.Models.GameLogs;
using CombatAnalysis.EnhancedWebApp.Server.Models.WoWMidnight;
using CombatAnalysis.EnhancedWebApp.Server.Models.WoWMoPClassic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace CombatAnalysis.EnhancedWebApp.Server.Controllers.GameLogs;

[Route("api/v1/[controller]")]
[ApiController]
public class CombatPlayerController : ControllerBase
{
    private readonly IHttpClientHelper _httpClient;

    public CombatPlayerController(IOptions<Cluster> cluster, IHttpClientHelper httpClient)
    {
        _httpClient = httpClient;
        _httpClient.APIUrl = cluster.Value.CombatParser;
    }

    [HttpGet("getByCombatId/{combatId:int:min(1)}")]
    public async Task<IActionResult> GetByCombatId(int combatId)
    {
        var responseMessage = await _httpClient.GetAsync($"CombatPlayer/getByCombatId/{combatId}");
        var combatPlayers = await responseMessage.Content.ReadFromJsonAsync<IEnumerable<CombatPlayerModel>>();

        return Ok(combatPlayers);
    }

    [HttpGet("{id:int:min(1)}")]
    public async Task<IActionResult> GetById(int id)
    {
        var responseMessage = await _httpClient.GetAsync($"CombatPlayer/{id}");
        var combatPlayer = await responseMessage.Content.ReadFromJsonAsync<CombatPlayerModel>();

        return Ok(combatPlayer);
    }

    [HttpGet("getPlayerStats/{combatPlayerId:int:min(1)}")]
    public async Task<IActionResult> GetPlayerStats(int combatPlayerId, int gameVersion)
    {
        var responseMessage = await _httpClient.GetAsync($"CombatPlayer/getPlayerStats/{combatPlayerId}?gameVersion={gameVersion}");
        IPlayerStatsModel? stats = gameVersion switch
        {
            0 => await responseMessage.Content.ReadFromJsonAsync<WoWMoPClassicPlayerStatsModel>(),
            1 => await responseMessage.Content.ReadFromJsonAsync<WoWMidnightPlayerStatsModel>(),
            _ => throw new ArgumentOutOfRangeException(nameof(gameVersion))
        };

        return Ok(stats);
    }
}
