using CombatAnalysis.WebApp.Consts;
using CombatAnalysis.WebApp.Interfaces;
using CombatAnalysis.WebApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace CombatAnalysis.WebApp.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class DamageTakenGeneralController : ControllerBase
{
    private readonly IHttpClientHelper _httpClient;
    private readonly ILogger<DamageTakenGeneralController> _logger;

    public DamageTakenGeneralController(IHttpClientHelper httpClient, ILogger<DamageTakenGeneralController> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
        _httpClient.BaseAddress = Port.CombatParserApi;
    }

    [HttpGet("getByCombatPlayerId/{combatPlayerId:int:min(1)}")]
    public async Task<IActionResult> GetByCombatPlayerId(int combatPlayerId)
    {
        try
        {
            var response = await _httpClient.GetAsync($"DamageTakenGeneral/getByCombatPlayerId/{combatPlayerId}");
            response.EnsureSuccessStatusCode();

            var damageTakenGenerals = await response.Content.ReadFromJsonAsync<IEnumerable<DamageTakenGeneralModel>>();

            return Ok(damageTakenGenerals);
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
