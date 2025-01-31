using CombatAnalysis.WebApp.Consts;
using CombatAnalysis.WebApp.Interfaces;
using CombatAnalysis.WebApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace CombatAnalysis.WebApp.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class ResourceRecoveryGeneralController : ControllerBase
{
    private readonly IHttpClientHelper _httpClient;
    private readonly ILogger<ResourceRecoveryGeneralController> _logger;

    public ResourceRecoveryGeneralController(IHttpClientHelper httpClient, ILogger<ResourceRecoveryGeneralController> logger)
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
            var response = await _httpClient.GetAsync($"ResourceRecoveryGeneral/getByCombatPlayerId/{combatPlayerId}");
            response.EnsureSuccessStatusCode();

            var resourceRecoveryGenerals = await response.Content.ReadFromJsonAsync<IEnumerable<ResourceRecoveryGeneralModel>>();

            return Ok(resourceRecoveryGenerals);
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
