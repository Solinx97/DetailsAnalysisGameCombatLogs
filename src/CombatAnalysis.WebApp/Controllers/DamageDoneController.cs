using CombatAnalysis.WebApp.Consts;
using CombatAnalysis.WebApp.Interfaces;
using CombatAnalysis.WebApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace CombatAnalysis.WebApp.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class DamageDoneController : ControllerBase
{
    private readonly IHttpClientHelper _httpClient;

    public DamageDoneController(IHttpClientHelper httpClient)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = Port.CombatParserApi;
    }

    [HttpGet("count/{combatPlayerId}")]
    public async Task<IActionResult> Count(int combatPlayerId)
    {
        var responseMessage = await _httpClient.GetAsync($"DamageDone/{combatPlayerId}");
        var count = await responseMessage.Content.ReadFromJsonAsync<int>();

        return Ok(count);
    }

    [HttpGet("getByCombatPlayerId")]
    public async Task<IActionResult> GetByCombatPlayerId(int combatPlayerId, int page, int pageSize)
    {
        var responseMessage = await _httpClient.GetAsync($"DamageDone/getByCombatPlayerId?combatPlayerId={combatPlayerId}&page={page}&pageSize={pageSize}");
        var damageDones = await responseMessage.Content.ReadFromJsonAsync<IEnumerable<DamageDoneModel>>();

        return Ok(damageDones);
    }
}
