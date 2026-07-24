using CombatAnalysis.EnhancedWebApp.Server.Consts;
using CombatAnalysis.EnhancedWebApp.Server.Interfaces;
using CombatAnalysis.EnhancedWebApp.Server.Models.GameLogs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace CombatAnalysis.EnhancedWebApp.Server.Controllers.GameLogs.CombatPlayerData;

[Route("api/v1/[controller]")]
[ApiController]
public class HealDoneGeneralController : ControllerBase
{
    private readonly IHttpClientHelper _httpClient;
    private readonly ILogger<HealDoneGeneralController> _logger;

    public HealDoneGeneralController(IOptions<Cluster> cluster, IHttpClientHelper httpClient, ILogger<HealDoneGeneralController> logger)
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
            var response = await _httpClient.GetAsync($"HealDoneGeneral/getByCombatPlayerId/{combatPlayerId}");
            response.EnsureSuccessStatusCode();

            var healDoneGenerals = await response.Content.ReadFromJsonAsync<IEnumerable<HealDoneGeneralModel>>();

            return Ok(healDoneGenerals);
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
