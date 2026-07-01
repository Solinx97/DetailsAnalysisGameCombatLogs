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

    [HttpGet("getByFilter")]
    public async Task<IActionResult> GetByFilter(int combatPlayerId, DetailsFilterType filter, string creator, string spell, int page, int pageSize)
    {
        try
        {
            return filter switch
            {
                DetailsFilterType.None => await GetByCombatPlayerId(combatPlayerId, page, pageSize),
                DetailsFilterType.Target => await GetByCreator(combatPlayerId, creator, page, pageSize),
                DetailsFilterType.Spell => await GetBySpell(combatPlayerId, spell, page, pageSize),
                DetailsFilterType.Creator => throw new NotImplementedException("Creator filter not implemented yet"),
                DetailsFilterType.All => await GetByAll(combatPlayerId, creator, spell, page, pageSize),
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

    [HttpGet("countByFilter")]
    public async Task<IActionResult> CountByFilter(int combatPlayerId, DetailsFilterType filter, string creator, string spell)
    {
        try
        {
            return filter switch
            {
                DetailsFilterType.None => await Count(combatPlayerId),
                DetailsFilterType.Target => await CountByCreator(combatPlayerId, creator),
                DetailsFilterType.Spell => await CountBySpell(combatPlayerId, spell),
                DetailsFilterType.Creator => throw new NotImplementedException("Count by creator not implemented yet"),
                DetailsFilterType.All => await CountByAll(combatPlayerId, creator, spell),
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

    public async Task<IActionResult> CountByCreator(int combatPlayerId, string creator)
    {
        try
        {
            var response = await _httpClient.GetAsync($"DamageTaken/countByCreator?combatPlayerId={combatPlayerId}&creator={creator}");
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
            var response = await _httpClient.GetAsync($"DamageTaken/countBySpell?combatPlayerId={combatPlayerId}&spell={spell}");
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

    public async Task<IActionResult> CountByAll(int combatPlayerId, string creator, string spell)
    {
        try
        {
            var response = await _httpClient.GetAsync($"DamageTaken/countByAll?combatPlayerId={combatPlayerId}&creator={creator}&spell={spell}");
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

    private async Task<IActionResult> GetByCreator(int combatPlayerId, string creator, int page, int pageSize)
    {
        try
        {
            var response = await _httpClient.GetAsync($"DamageTaken/getByCreator?combatPlayerId={combatPlayerId}&creator={creator}&page={page}&pageSize={pageSize}");
            response.EnsureSuccessStatusCode();

            var damageTakens = await response.Content.ReadFromJsonAsync<IEnumerable<DamageTakenModel>>();

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

    private async Task<IActionResult> GetBySpell(int combatPlayerId, string spell, int page, int pageSize)
    {
        try
        {
            var response = await _httpClient.GetAsync($"DamageTaken/getBySpell?combatPlayerId={combatPlayerId}&spell={spell}&page={page}&pageSize={pageSize}");
            response.EnsureSuccessStatusCode();

            var damageTakens = await response.Content.ReadFromJsonAsync<IEnumerable<DamageTakenModel>>();

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

    private async Task<IActionResult> GetByAll(int combatPlayerId, string creator, string spell, int page, int pageSize)
    {
        try
        {
            var response = await _httpClient.GetAsync($"DamageTaken/getByAll?combatPlayerId={combatPlayerId}&creator={creator}&spell={spell}&page={page}&pageSize={pageSize}");
            response.EnsureSuccessStatusCode();

            var damageTaknes = await response.Content.ReadFromJsonAsync<IEnumerable<DamageTakenModel>>();

            return Ok(damageTaknes);
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
