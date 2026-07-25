using CombatAnalysis.EnhancedWebApp.Server.Consts;
using CombatAnalysis.EnhancedWebApp.Server.Interfaces;
using CombatAnalysis.EnhancedWebApp.Server.Models.GameLogs.CombatPlayerData;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace CombatAnalysis.EnhancedWebApp.Server.Controllers.GameLogs.CombatPlayerData;

[Route("api/v1/[controller]")]
[ApiController]
public class CombatPlayerDeathController : ControllerBase
{
    private readonly IHttpClientHelper _httpClient;
    private readonly ILogger<CombatPlayerDeathController> _logger;

    public CombatPlayerDeathController(IOptions<Cluster> cluster, IHttpClientHelper httpClient, ILogger<CombatPlayerDeathController> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
        _httpClient.APIUrl = cluster.Value.CombatParser;
    }

    [HttpGet("getByCombatPlayerId/{combatPlayerId:int:min(1)}")]
    public async Task<IActionResult> GetByCombatPlayerId(int combatPlayerId)
    {
        try
        {
            var response = await _httpClient.GetAsync($"CombatPlayerDeath/getByCombatPlayerId/{combatPlayerId}");
            response.EnsureSuccessStatusCode();

            var playerDeaths = await response.Content.ReadFromJsonAsync<IEnumerable<CombatPlayerDeathModel>>();

            return Ok(playerDeaths);
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "HTTP request error: {Message}", ex.Message);

            return BadRequest();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred: {Message}", ex.Message);

            return BadRequest();
        }
    }
}
