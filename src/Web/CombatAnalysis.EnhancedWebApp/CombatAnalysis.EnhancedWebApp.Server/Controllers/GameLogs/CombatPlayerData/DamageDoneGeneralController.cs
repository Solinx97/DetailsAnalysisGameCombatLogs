using CombatAnalysis.EnhancedWebApp.Server.Consts;
using CombatAnalysis.EnhancedWebApp.Server.Interfaces;
using CombatAnalysis.EnhancedWebApp.Server.Models.GameLogs.CombatPlayerData;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace CombatAnalysis.EnhancedWebApp.Server.Controllers.GameLogs.CombatPlayerData;

[Route("api/v1/[controller]")]
[ApiController]
public class DamageDoneGeneralController : ControllerBase
{
    private readonly IHttpClientHelper _httpClient;
    private readonly ILogger<DamageDoneGeneralController> _logger;

    public DamageDoneGeneralController(IOptions<Cluster> cluster, IHttpClientHelper httpClient, ILogger<DamageDoneGeneralController> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
        _httpClient.APIUrl = cluster.Value.CombatParser;
    }

    [HttpGet("getByUnitId/{unitId}")]
    public async Task<IActionResult> GetByUnitId(string unitId)
    {
        try
        {
            var response = await _httpClient.GetAsync($"DamageDoneGeneral/getByUnitId/{unitId}");
            response.EnsureSuccessStatusCode();

            var damageDoneGenerals = await response.Content.ReadFromJsonAsync<IEnumerable<DamageDoneGeneralModel>>();

            return Ok(damageDoneGenerals);
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

    [HttpGet("getDamageTakenByUnitId/{unitId}")]
    public async Task<IActionResult> GetDamageTakenByUnitId(string unitId)
    {
        try
        {
            var response = await _httpClient.GetAsync($"DamageDoneGeneral/getDamageTakenByUnitId/{unitId}");
            response.EnsureSuccessStatusCode();

            var damageTakenGenerals = await response.Content.ReadFromJsonAsync<IEnumerable<DamageDoneGeneralModel>>();

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
