using CombatAnalysis.EnhancedWebApp.Server.Consts;
using CombatAnalysis.EnhancedWebApp.Server.Interfaces;
using CombatAnalysis.EnhancedWebApp.Server.Models.GameLogs.CombatPlayerData;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace CombatAnalysis.EnhancedWebApp.Server.Controllers.GameLogs.CombatPlayerData;

[Route("api/v1/[controller]")]
[ApiController]
public class ResourceRecoveryGeneralController : ControllerBase
{
    private readonly IHttpClientHelper _httpClient;
    private readonly ILogger<ResourceRecoveryGeneralController> _logger;

    public ResourceRecoveryGeneralController(IOptions<Cluster> cluster, IHttpClientHelper httpClient, ILogger<ResourceRecoveryGeneralController> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
        _httpClient.APIUrl = cluster.Value.CombatParser;
    }

    [HttpGet("getByUnitId/{unitId}")]
    public async Task<IActionResult> GetByUnitId(string unitId, int combatId)
    {
        try
        {
            var response = await _httpClient.GetAsync($"ResourceRecoveryGeneral/getByUnitId/{unitId}?combatId={combatId}");
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
