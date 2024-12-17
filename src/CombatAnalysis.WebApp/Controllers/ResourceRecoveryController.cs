using CombatAnalysis.WebApp.Consts;
using CombatAnalysis.WebApp.Interfaces;
using CombatAnalysis.WebApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace CombatAnalysis.WebApp.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class ResourceRecoveryController : ControllerBase
{
    private readonly IHttpClientHelper _httpClient;

    public ResourceRecoveryController(IHttpClientHelper httpClient)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = Port.CombatParserApi;
    }

    [HttpGet("getByCombatPlayerId")]
    public async Task<IActionResult> GetByCombatPlayerId(int combatPlayerId, int page, int pageSize)
    {
        var responseMessage = await _httpClient.GetAsync($"ResourceRecovery/getByCombatPlayerId?combatPlayerId={combatPlayerId}&page={page}&pageSize={pageSize}");
        var resourceRecoveries = await responseMessage.Content.ReadFromJsonAsync<IEnumerable<ResourceRecoveryModel>>();

        return Ok(resourceRecoveries);
    }

    [HttpGet("getUniqueCreators/{combatPlayerId}")]
    public async Task<IActionResult> GetUniqueCreators(int combatPlayerId)
    {
        var responseMessage = await _httpClient.GetAsync($"ResourceRecovery/getUniqueCreators/{combatPlayerId}");
        var uniqueTargets = await responseMessage.Content.ReadFromJsonAsync<IEnumerable<string>>();

        return Ok(uniqueTargets);
    }

    [HttpGet("count/{combatPlayerId}")]
    public async Task<IActionResult> Count(int combatPlayerId)
    {
        var responseMessage = await _httpClient.GetAsync($"ResourceRecovery/count/{combatPlayerId}");
        var count = await responseMessage.Content.ReadFromJsonAsync<int>();

        return Ok(count);
    }

    [HttpGet("getByCreator")]
    public async Task<IActionResult> GetByCreator(int combatPlayerId, string creator, int page, int pageSize)
    {
        if (creator.Equals("-1"))
        {
            return await GetByCombatPlayerId(combatPlayerId, page, pageSize);
        }

        var responseMessage = await _httpClient.GetAsync($"ResourceRecovery/getByCreator?combatPlayerId={combatPlayerId}&creator={creator}&page={page}&pageSize={pageSize}");
        var damageDones = await responseMessage.Content.ReadFromJsonAsync<IEnumerable<DamageDoneModel>>();

        return Ok(damageDones);
    }

    [HttpGet("countByCreator")]
    public async Task<IActionResult> CountByCreator(int combatPlayerId, string creator)
    {
        if (creator.Equals("-1"))
        {
            return await Count(combatPlayerId);
        }

        var responseMessage = await _httpClient.GetAsync($"ResourceRecovery/countByCreator?combatPlayerId={combatPlayerId}&creator={creator}");
        var count = await responseMessage.Content.ReadFromJsonAsync<int>();

        return Ok(count);
    }
}
