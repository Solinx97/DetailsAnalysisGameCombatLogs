using CombatAnalysis.EnhancedWebApp.Server.Consts;
using CombatAnalysis.EnhancedWebApp.Server.Enums;
using CombatAnalysis.EnhancedWebApp.Server.Interfaces;
using CombatAnalysis.EnhancedWebApp.Server.Models.GameLogs;
using CombatAnalysis.EnhancedWebApp.Server.Models.GameLogs.Chart;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace CombatAnalysis.EnhancedWebApp.Server.Controllers.GameLogs.CombatPlayerData;

[Route("api/v1/[controller]")]
[ApiController]
public class HealDoneController : ControllerBase
{
    private readonly IHttpClientHelper _httpClient;
    private readonly ILogger<HealDoneController> _logger;

    public HealDoneController(IOptions<Cluster> cluster, IHttpClientHelper httpClient, ILogger<HealDoneController> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
        _httpClient.APIUrl = cluster.Value.CombatParser;
    }

    [HttpGet("getByCombatPlayerId")]
    public async Task<IActionResult> GetByCombatPlayerId(int combatPlayerId, int page, int pageSize)
    {
        try
        {
            var response = await _httpClient.GetAsync($"HealDone/getByCombatPlayerId?combatPlayerId={combatPlayerId}&page={page}&pageSize={pageSize}");
            response.EnsureSuccessStatusCode();

            var healDones = await response.Content.ReadFromJsonAsync<IEnumerable<HealDoneModel>>();

            return Ok(healDones);
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
    public async Task<IActionResult> Count(int combatPlayerId, string target, string creator, string spell, string from, string to)
    {
        try
        {
            var response = await _httpClient.GetAsync($"HealDone/count?combatPlayerId={combatPlayerId}&target={target}&creator={creator}&spell={spell}&from={from}&to={to}");
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
    public async Task<IActionResult> GetAll(int combatPlayerId, string target, string creator, string spell, string from, string to, int page, int pageSize)
    {
        try
        {
            var response = await _httpClient.GetAsync($"HealDone/getAll?combatPlayerId={combatPlayerId}&target={target}&creator={creator}&spell={spell}&from={from}&to={to}&page={page}&pageSize={pageSize}");
            response.EnsureSuccessStatusCode();

            var healDones = await response.Content.ReadFromJsonAsync<IEnumerable<DamageDoneModel>>();

            return Ok(healDones);
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

    [HttpGet("getCombatPlayerChart/{combatPlayerId}")]
    public async Task<IActionResult> GetCombatPlayerChart(int combatPlayerId)
    {
        try
        {
            var response = await _httpClient.GetAsync($"HealDone/getCombatPlayerChart/{combatPlayerId}");
            response.EnsureSuccessStatusCode();

            var healDones = await response.Content.ReadFromJsonAsync<IEnumerable<ChartGenericModel>>();

            return Ok(healDones);
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

    [HttpGet("getGenericChart/{combatId}")]
    public async Task<IActionResult> GetGenericChart(int combatId)
    {
        try
        {
            var response = await _httpClient.GetAsync($"HealDone/getGenericChart/{combatId}");
            response.EnsureSuccessStatusCode();

            var healDones = await response.Content.ReadFromJsonAsync<Dictionary<string, ChartGenericModel[]>>();

            return Ok(healDones);
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
    public async Task<IActionResult> GetUniqueFilterValues(int combatPlayerId, DetailsFilterType filter)
    {
        try
        {
            string filterActionName;
            switch (filter)
            {
                case DetailsFilterType.None:
                    return BadRequest();
                case DetailsFilterType.Target:
                    filterActionName = "getUniqueTargets";
                    break;
                case DetailsFilterType.Spell:
                    filterActionName = "getUniqueSpells";
                    break;
                default:
                    return BadRequest();
            }

            var response = await _httpClient.GetAsync($"HealDone/{filterActionName}/{combatPlayerId}");
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<IEnumerable<string>>();

            return Ok(result);
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

    [HttpGet("getValueByTarget")]
    public async Task<IActionResult> GetValueByTarget(int combatPlayerId, string target)
    {
        try
        {
            var response = await _httpClient.GetAsync($"HealDone/getValueByTarget?combatPlayerId={combatPlayerId}&target={target}");
            response.EnsureSuccessStatusCode();

            var valueByTarget = await response.Content.ReadAsStringAsync();

            return Ok(valueByTarget);
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
