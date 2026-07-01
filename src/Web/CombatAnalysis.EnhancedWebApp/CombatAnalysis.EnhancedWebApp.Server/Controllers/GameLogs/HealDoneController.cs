using CombatAnalysis.EnhancedWebApp.Server.Consts;
using CombatAnalysis.EnhancedWebApp.Server.Enums;
using CombatAnalysis.EnhancedWebApp.Server.Interfaces;
using CombatAnalysis.EnhancedWebApp.Server.Models.GameLogs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace CombatAnalysis.EnhancedWebApp.Server.Controllers.GameLogs;

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

            var damageDones = await response.Content.ReadFromJsonAsync<IEnumerable<HealDoneModel>>();

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
            var response = await _httpClient.GetAsync($"HealDone/getDamageByEachTarget/{combatId}");
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
    public async Task<IActionResult> GetByFilter(int combatPlayerId, DetailsFilterType filter, string target, string spell, int page, int pageSize)
    {
        try
        {
            return filter switch
            {
                DetailsFilterType.None => await GetByCombatPlayerId(combatPlayerId, page, pageSize),
                DetailsFilterType.Target => await GetByTarget(combatPlayerId, target, page, pageSize),
                DetailsFilterType.Spell => await GetBySpell(combatPlayerId, spell, page, pageSize),
                DetailsFilterType.Creator => throw new NotImplementedException("Creator filter not implemented yet"),
                DetailsFilterType.All => await GetByAll(combatPlayerId, target, spell, page, pageSize),
                _ => BadRequest(),
            };
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

    [HttpGet("countByFilter")]
    public async Task<IActionResult> CountByFilter(int combatPlayerId, DetailsFilterType filter, string target, string spell)
    {
        try
        {
            return filter switch
            {
                DetailsFilterType.None => await Count(combatPlayerId),
                DetailsFilterType.Target => await CountByTarget(combatPlayerId, target),
                DetailsFilterType.Spell => await CountBySpell(combatPlayerId, spell),
                DetailsFilterType.Creator => throw new NotImplementedException("Count by creator not implemented yet"),
                DetailsFilterType.All => await CountByAll(combatPlayerId, target, spell),
                _ => BadRequest(),
            };
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

    public async Task<IActionResult> CountByTarget(int combatPlayerId, string target)
    {
        try
        {
            var response = await _httpClient.GetAsync($"HealDone/countByTarget?combatPlayerId={combatPlayerId}&target={target}");
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

    public async Task<IActionResult> CountBySpell(int combatPlayerId, string spell)
    {
        try
        {
            var response = await _httpClient.GetAsync($"HealDone/countBySpell?combatPlayerId={combatPlayerId}&spell={spell}");
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

    public async Task<IActionResult> CountByAll(int combatPlayerId, string target, string spell)
    {
        try
        {
            var response = await _httpClient.GetAsync($"HealDone/countByAll?combatPlayerId={combatPlayerId}&target={target}&spell={spell}");
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

    private async Task<IActionResult> GetByTarget(int combatPlayerId, string target, int page, int pageSize)
    {
        try
        {
            var response = await _httpClient.GetAsync($"HealDone/getByTarget?combatPlayerId={combatPlayerId}&target={target}&page={page}&pageSize={pageSize}");
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

    private async Task<IActionResult> GetBySpell(int combatPlayerId, string spell, int page, int pageSize)
    {
        try
        {
            var response = await _httpClient.GetAsync($"HealDone/getBySpell?combatPlayerId={combatPlayerId}&spell={spell}&page={page}&pageSize={pageSize}");
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

    private async Task<IActionResult> GetByAll(int combatPlayerId, string target, string spell, int page, int pageSize)
    {
        try
        {
            var response = await _httpClient.GetAsync($"HealDone/getByAll?combatPlayerId={combatPlayerId}&target={target}&spell={spell}&page={page}&pageSize={pageSize}");
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
}
