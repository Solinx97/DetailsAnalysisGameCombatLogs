using CombatAnalysis.EnhancedWebApp.Server.Consts;
using CombatAnalysis.EnhancedWebApp.Server.Enums;
using CombatAnalysis.EnhancedWebApp.Server.Interfaces;
using CombatAnalysis.EnhancedWebApp.Server.Models.GameLogs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace CombatAnalysis.EnhancedWebApp.Server.Controllers.GameLogs;

[Route("api/v1/[controller]")]
[ApiController]
public class DamageTakenController : ControllerBase
{
    private readonly IHttpClientHelper _httpClient;
    private readonly ILogger<DamageTakenController> _logger;

    public DamageTakenController(IOptions<Cluster> cluster, IHttpClientHelper httpClient, ILogger<DamageTakenController> logger)
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
            var response = await _httpClient.GetAsync($"DamageTaken/getByCombatPlayerId?combatPlayerId={combatPlayerId}&page={page}&pageSize={pageSize}");
            response.EnsureSuccessStatusCode();

            var DamageTakenModels = await response.Content.ReadFromJsonAsync<IEnumerable<DamageTakenModel>>();

            return Ok(DamageTakenModels);
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
            var response = await _httpClient.GetAsync($"DamageTaken/count?combatPlayerId={combatPlayerId}&target={target}&creator={creator}&spell={spell}&from={from}&to={to}");
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
            var response = await _httpClient.GetAsync($"DamageTaken/getAll?combatPlayerId={combatPlayerId}&target={target}&creator={creator}&spell={spell}&from={from}&to={to}&page={page}&pageSize={pageSize}");
            response.EnsureSuccessStatusCode();

            var damageDones = await response.Content.ReadFromJsonAsync<IEnumerable<DamageDoneModel>>();

            return Ok(damageDones);
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
                case DetailsFilterType.Creator:
                    filterActionName = "getUniqueCreators";
                    break;
                case DetailsFilterType.Spell:
                    filterActionName = "getUniqueSpells";
                    break;
                default:
                    return BadRequest();
            }

            var response = await _httpClient.GetAsync($"DamageTaken/{filterActionName}/{combatPlayerId}");
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
}
