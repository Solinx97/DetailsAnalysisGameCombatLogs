using CombatAnalysis.WebApp.Consts;
using CombatAnalysis.WebApp.Interfaces;
using CombatAnalysis.WebApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace CombatAnalysis.WebApp.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class DamageTakenController : ControllerBase
{
    private readonly IHttpClientHelper _httpClient;

    public DamageTakenController(IHttpClientHelper httpClient)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = Port.CombatParserApi;
    }

    [HttpGet("getByCombatPlayerId")]
    public async Task<IActionResult> GetByCombatPlayerId(int combatPlayerId, int page, int pageSize)
    {
        var responseMessage = await _httpClient.GetAsync($"DamageTaken/getByCombatPlayerId?combatPlayerId={combatPlayerId}&page={page}&pageSize={pageSize}");
        var damageTakens = await responseMessage.Content.ReadFromJsonAsync<IEnumerable<DamageTakenModel>>();

        return Ok(damageTakens);
    }

    [HttpGet("count/{combatPlayerId}")]
    public async Task<IActionResult> Count(int combatPlayerId)
    {
        var responseMessage = await _httpClient.GetAsync($"DamageTaken/count/{combatPlayerId}");
        var count = await responseMessage.Content.ReadFromJsonAsync<int>();

        return Ok(count);
    }

    [HttpGet("getUniqueTargets/{combatPlayerId}")]
    public async Task<IActionResult> GetUniqueTargets(int combatPlayerId)
    {
        var responseMessage = await _httpClient.GetAsync($"DamageTaken/getUniqueTargets/{combatPlayerId}");
        var uniqueTargets = await responseMessage.Content.ReadFromJsonAsync<IEnumerable<string>>();

        return Ok(uniqueTargets);
    }

    [HttpGet("countByTarget")]
    public async Task<IActionResult> CountByTarget(int combatPlayerId, string target)
    {
        if (target.Equals("-1"))
        {
            return await Count(combatPlayerId);
        }

        var responseMessage = await _httpClient.GetAsync($"DamageTaken/countByTarget?combatPlayerId={combatPlayerId}&target={target}");
        var count = await responseMessage.Content.ReadFromJsonAsync<int>();

        return Ok(count);
    }

    [HttpGet("getByTarget")]
    public async Task<IActionResult> GetByTarget(int combatPlayerId, string target, int page, int pageSize)
    {
        if (target.Equals("-1"))
        {
            return await GetByCombatPlayerId(combatPlayerId, page, pageSize);
        }

        var responseMessage = await _httpClient.GetAsync($"DamageTaken/getByTarget?combatPlayerId={combatPlayerId}&target={target}&page={page}&pageSize={pageSize}");
        var damageDones = await responseMessage.Content.ReadFromJsonAsync<IEnumerable<DamageDoneModel>>();

        return Ok(damageDones);
    }
}
