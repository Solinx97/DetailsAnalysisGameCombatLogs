using CombatAnalysis.WebApp.Consts;
using CombatAnalysis.WebApp.Enums;
using CombatAnalysis.WebApp.Interfaces;
using CombatAnalysis.WebApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace CombatAnalysis.WebApp.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class DamageTakenController : ControllerBase
{
    private readonly IHttpClientHelper _httpClient;
    private readonly ILogger<DamageTakenController> _logger;

    public DamageTakenController(IHttpClientHelper httpClient, ILogger<DamageTakenController> logger)
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
            var response = await _httpClient.GetAsync($"DamageTaken/getByCombatPlayerId?combatPlayerId={combatPlayerId}&page={page}&pageSize={pageSize}");
            response.EnsureSuccessStatusCode();

            var damageTakens = await response.Content.ReadFromJsonAsync<IEnumerable<DamageDoneModel>>();

            return Ok(damageTakens);
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
            var response = await _httpClient.GetAsync($"DamageTaken/count/{combatPlayerId}");
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
                case DetailsFilterType.Creator:
                    filterName = "creator";
                    filterActionName = "getByCreator";
                    break;
                case DetailsFilterType.Spell:
                    filterName = "spell";
                    filterActionName = "getBySpell";
                    break;
                default:
                    return BadRequest();
            }

            var response = await _httpClient.GetAsync($"DamageTaken/{filterActionName}?combatPlayerId={combatPlayerId}&{filterName}={filterValue}&page={page}&pageSize={pageSize}");
            response.EnsureSuccessStatusCode();

            var damageTakens = await response.Content.ReadFromJsonAsync<IEnumerable<DamageDoneModel>>();

            return Ok(damageTakens);
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
                case DetailsFilterType.Creator:
                    filterName = "creator";
                    filterActionName = "countByCreator";
                    break;
                case DetailsFilterType.Spell:
                    filterName = "spell";
                    filterActionName = "countBySpell";
                    break;
                default:
                    return BadRequest();
            }

            var response = await _httpClient.GetAsync($"DamageTaken/{filterActionName}?combatPlayerId={combatPlayerId}&{filterName}={filterValue}");
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
