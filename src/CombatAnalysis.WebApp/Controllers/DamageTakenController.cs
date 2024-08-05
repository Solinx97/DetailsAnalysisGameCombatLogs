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

    [HttpGet("count/{combatPlayerId}")]
    public async Task<IActionResult> Count(int combatPlayerId)
    {
        var responseMessage = await _httpClient.GetAsync($"DamageTaken/{combatPlayerId}");
        var count = await responseMessage.Content.ReadFromJsonAsync<int>();

        return Ok(count);
    }

    [HttpGet("getByCombatPlayerId")]
    public async Task<IActionResult> GetByCombatPlayerId(int combatPlayerId, int page, int pageSize)
    {
        var responseMessage = await _httpClient.GetAsync($"DamageTaken/getByCombatPlayerId?combatPlayerId={combatPlayerId}&page={page}&pageSize={pageSize}");
        var damageTakens = await responseMessage.Content.ReadFromJsonAsync<IEnumerable<DamageTakenModel>>();

        return Ok(damageTakens);
    }
}
