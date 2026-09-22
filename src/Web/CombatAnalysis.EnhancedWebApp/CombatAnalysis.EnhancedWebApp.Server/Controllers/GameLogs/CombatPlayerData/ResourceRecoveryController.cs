using CombatAnalysis.EnhancedWebApp.Server.Consts;
using CombatAnalysis.EnhancedWebApp.Server.Enums;
using CombatAnalysis.EnhancedWebApp.Server.Interfaces;
using CombatAnalysis.EnhancedWebApp.Server.Models.GameLogs.Chart;
using CombatAnalysis.EnhancedWebApp.Server.Models.GameLogs.CombatPlayerData;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace CombatAnalysis.EnhancedWebApp.Server.Controllers.GameLogs.CombatPlayerData;

[Route("api/v1/[controller]")]
[ApiController]
public class ResourceRecoveryController : ControllerBase
{
    private readonly IHttpClientHelper _httpClient;
    private readonly ILogger<ResourceRecoveryController> _logger;

    public ResourceRecoveryController(IOptions<Cluster> cluster, IHttpClientHelper httpClient, ILogger<ResourceRecoveryController> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
        _httpClient.APIUrl = cluster.Value.CombatParser;
    }

    [HttpGet("getByUnitId")]
    public async Task<IActionResult> GetByUnitId(string unitId, int page, int pageSize)
    {
        try
        {
            var response = await _httpClient.GetAsync($"ResourceRecovery/getByCombatPlayerId?unitId={unitId}&page={page}&pageSize={pageSize}");
            response.EnsureSuccessStatusCode();

            var resourceRecoveries = await response.Content.ReadFromJsonAsync<IEnumerable<ResourceRecoveryModel>>();

            return Ok(resourceRecoveries);
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

    [HttpGet("count")]
    public async Task<IActionResult> Count(string unitId, string target, string creator, string spell, string from, string to)
    {
        try
        {
            var response = await _httpClient.GetAsync($"ResourceRecovery/count?unitId={unitId}&target={target}&creator={creator}&spell={spell}&from={from}&to={to}");
            response.EnsureSuccessStatusCode();

            var count = await response.Content.ReadFromJsonAsync<int>();

            return Ok(count);
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

    [HttpGet("getAll")]
    public async Task<IActionResult> GetAll(string unitId, string target, string creator, string spell, string from, string to, int page, int pageSize)
    {
        try
        {
            var response = await _httpClient.GetAsync($"ResourceRecovery/getAll?unitId={unitId}&target={target}&creator={creator}&spell={spell}&from={from}&to={to}&page={page}&pageSize={pageSize}");
            response.EnsureSuccessStatusCode();

            var resourceRecoveries = await response.Content.ReadFromJsonAsync<IEnumerable<ResourceRecoveryModel>>();

            return Ok(resourceRecoveries);
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

    [HttpGet("getUnitChart/{unitId}")]
    public async Task<IActionResult> GetUnitChart(string unitId)
    {
        try
        {
            var response = await _httpClient.GetAsync($"ResourceRecovery/getUnitChart/{unitId}");
            response.EnsureSuccessStatusCode();

            var resourceRecoveries = await response.Content.ReadFromJsonAsync<IEnumerable<ChartGenericModel>>();

            return Ok(resourceRecoveries);
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

    [HttpGet("getUniqueFilterValues")]
    public async Task<IActionResult> GetUniqueFilterValues(string unitId, DetailsFilterType filter)
    {
        try
        {
            string filterActionName;
            switch (filter)
            {
                case DetailsFilterType.None:
                    return BadRequest();
                case DetailsFilterType.Creator:
                    filterActionName = "getUniqueCreators";
                    break;
                case DetailsFilterType.Spell:
                    filterActionName = "getUniqueSpells";
                    break;
                default:
                    return BadRequest();
            }

            var response = await _httpClient.GetAsync($"ResourceRecovery/{filterActionName}/{unitId}");
            response.EnsureSuccessStatusCode();

            var uniqueFilterValues = await response.Content.ReadFromJsonAsync<IEnumerable<string>>();

            return Ok(uniqueFilterValues);
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
