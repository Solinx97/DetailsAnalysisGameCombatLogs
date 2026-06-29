using CombatAnalysis.EnhancedWebApp.Server.Consts;
using CombatAnalysis.EnhancedWebApp.Server.Enums;
using CombatAnalysis.EnhancedWebApp.Server.Interfaces;
using CombatAnalysis.EnhancedWebApp.Server.Models.GameLogs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace CombatAnalysis.EnhancedWebApp.Server.Controllers.GameLogs;

[Route("api/v1/[controller]")]
[ApiController]
public class DamageDoneController : ControllerBase
{
    private readonly IHttpClientHelper _httpClient;
    private readonly ILogger<DamageDoneController> _logger;

    public DamageDoneController(IOptions<Cluster> cluster, IHttpClientHelper httpClient, ILogger<DamageDoneController> logger)
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
            var response = await _httpClient.GetAsync($"DamageDone/getByCombatPlayerId?combatPlayerId={combatPlayerId}&page={page}&pageSize={pageSize}");
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

    [HttpGet("count/{combatPlayerId}")]
    public async Task<IActionResult> Count(int combatPlayerId)
    {
        try
        {
            var response = await _httpClient.GetAsync($"DamageDone/count/{combatPlayerId}");
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

            var response = await _httpClient.GetAsync($"DamageDone/{filterActionName}/{combatPlayerId}");
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

    [HttpGet("getDamageByEachTarget/{combatId}")]
    public async Task<IActionResult> GetDamageByEachTarget(int combatId)
    {
        try
        {
            var response = await _httpClient.GetAsync($"DamageDone/getDamageByEachTarget/{combatId}");
            response.EnsureSuccessStatusCode();

            var damageByEachTarget = await response.Content.ReadFromJsonAsync<IEnumerable<List<CombatTargetModel>>>();

            return Ok(damageByEachTarget);
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

    [HttpGet("getByFilter")]
    public async Task<IActionResult> GetByFilter(int combatPlayerId, DetailsFilterType filter, string filterValue, int page, int pageSize)
    {
        try
        {
            string filterName;
            string filterActionName;
            switch (filter)
            {
                case DetailsFilterType.None:
                    return await GetByCombatPlayerId(combatPlayerId, page, pageSize);
                case DetailsFilterType.Target:
                    filterName = "target";
                    filterActionName = "getByTarget";
                    break;
                case DetailsFilterType.Spell:
                    filterName = "spell";
                    filterActionName = "getBySpell";
                    break;
                case DetailsFilterType.All:
                    var splitValue = filterValue.Split(';');
                    if (splitValue.Length != 2)
                    {
                        return BadRequest();
                    }
                    else
                    {
                        return await GetByAll(combatPlayerId, splitValue[0], splitValue[1], page, pageSize);
                    }
                default:
                    return BadRequest();
            }

            var response = await _httpClient.GetAsync($"DamageDone/{filterActionName}?combatPlayerId={combatPlayerId}&{filterName}={filterValue}&page={page}&pageSize={pageSize}");
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

    [HttpGet("getValueByTarget")]
    public async Task<IActionResult> GetValueByTarget(int combatPlayerId, string target)
    {
        try
        {
            var response = await _httpClient.GetAsync($"DamageDone/getValueByTarget?combatPlayerId={combatPlayerId}&target={target}");
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

    [HttpGet("countByFilter")]
    public async Task<IActionResult> CountByFilter(int combatPlayerId, DetailsFilterType filter, string filterValue)
    {
        try
        {
            string filterName;
            string filterActionName;
            switch (filter)
            {
                case DetailsFilterType.None:
                    return await Count(combatPlayerId);
                case DetailsFilterType.Target:
                    filterName = "target";
                    filterActionName = "countByTarget";
                    break;
                case DetailsFilterType.Spell:
                    filterName = "spell";
                    filterActionName = "countBySpell";
                    break;
                default:
                    return BadRequest();
            }

            var response = await _httpClient.GetAsync($"DamageDone/{filterActionName}?combatPlayerId={combatPlayerId}&{filterName}={filterValue}");
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

    private async Task<IActionResult> GetByAll(int combatPlayerId, string target, string spell, int page, int pageSize)
    {
        try
        {
            var response = await _httpClient.GetAsync($"DamageDone/getByAll?combatPlayerId={combatPlayerId}&target={target}&spell={spell}&page={page}&pageSize={pageSize}");
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
}
