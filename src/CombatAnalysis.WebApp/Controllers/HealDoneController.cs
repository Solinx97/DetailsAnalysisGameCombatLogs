using CombatAnalysis.WebApp.Consts;
using CombatAnalysis.WebApp.Interfaces;
using CombatAnalysis.WebApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace CombatAnalysis.WebApp.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class HealDoneController : ControllerBase
{
    private readonly IHttpClientHelper _httpClient;

    public HealDoneController(IHttpClientHelper httpClient)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = Port.CombatParserApi;
    }

    [HttpGet("count/{combatPlayerId}")]
    public async Task<IActionResult> Count(int combatPlayerId)
    {
        var responseMessage = await _httpClient.GetAsync($"HealDone/{combatPlayerId}");
        var count = await responseMessage.Content.ReadFromJsonAsync<int>();

        return Ok(count);
    }

    [HttpGet("getByCombatPlayerId")]
    public async Task<IActionResult> GetByCombatPlayerId(int combatPlayerId, int page, int pageSize)
    {
        var responseMessage = await _httpClient.GetAsync($"HealDone/getByCombatPlayerId?combatPlayerId={combatPlayerId}&page={page}&pageSize={pageSize}");
        var healDones = await responseMessage.Content.ReadFromJsonAsync<IEnumerable<HealDoneModel>>();

        return Ok(healDones);
    }
}
