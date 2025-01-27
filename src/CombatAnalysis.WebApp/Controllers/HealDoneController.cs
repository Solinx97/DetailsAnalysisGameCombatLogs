using CombatAnalysis.WebApp.Consts;
using CombatAnalysis.WebApp.Enums;
using CombatAnalysis.WebApp.Interfaces;
using CombatAnalysis.WebApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace CombatAnalysis.WebApp.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class HealDoneController : ControllerBase
{
    private readonly IHttpClientHelper _httpClient;
    private readonly ILogger<HealDoneController> _logger;

    public HealDoneController(IHttpClientHelper httpClient, ILogger<HealDoneController> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
        _httpClient.APIUrl = Cluster.CombatParser;
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

    [HttpGet("count/{combatPlayerId}")]
    public async Task<IActionResult> Count(int combatPlayerId)
    {
        try
        {
            var response = await _httpClient.GetAsync($"HealDone/count/{combatPlayerId}");
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

            var response = await _httpClient.GetAsync($"HealDone/{filterActionName}/{combatPlayerId}");
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
                default:
                    return BadRequest();
            }

            var response = await _httpClient.GetAsync($"HealDone/{filterActionName}?combatPlayerId={combatPlayerId}&{filterName}={filterValue}&page={page}&pageSize={pageSize}");
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

            var response = await _httpClient.GetAsync($"HealDone/{filterActionName}?combatPlayerId={combatPlayerId}&{filterName}={filterValue}");
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
}
