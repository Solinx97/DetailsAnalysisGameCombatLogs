using CombatAnalysis.WebApp.Consts;
using CombatAnalysis.WebApp.Interfaces;
using CombatAnalysis.WebApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace CombatAnalysis.WebApp.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class PlayerDeathController : ControllerBase
{
    private readonly IHttpClientHelper _httpClient;
    private readonly ILogger<PlayerDeathController> _logger;

    public PlayerDeathController(IHttpClientHelper httpClient, ILogger<PlayerDeathController> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
        _httpClient.APIUrl = Cluster.CombatParser;
    }

    [HttpGet("getByCombatPlayerId/{combatPlayerId:int:min(1)}")]
    public async Task<IActionResult> GetByCombatPlayerId(int combatPlayerId)
    {
        try
        {
            var response = await _httpClient.GetAsync($"PlayerDeath/getByCombatPlayerId/{combatPlayerId}");
            response.EnsureSuccessStatusCode();

            var playerDeaths = await response.Content.ReadFromJsonAsync<IEnumerable<PlayerDeathModel>>();

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
