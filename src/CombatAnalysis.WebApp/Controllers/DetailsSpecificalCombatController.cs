using CombatAnalysis.WebApp.Consts;
using CombatAnalysis.WebApp.Interfaces;
using CombatAnalysis.WebApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace CombatAnalysis.WebApp.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class DetailsSpecificalCombatController : ControllerBase
{
    private readonly IHttpClientHelper _httpClient;

    public DetailsSpecificalCombatController(IHttpClientHelper httpClient)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = Port.CombatParserApi;
    }

    [HttpGet("combatPlayersByCombatId/{id}")]
    public async Task<IActionResult> GetCombatPlayersByCombatId(int id)
    {
        var responseMessage = await _httpClient.GetAsync($"CombatPlayer/FindByCombatId/{id}");
        var combatPlayers = await responseMessage.Content.ReadFromJsonAsync<IEnumerable<CombatPlayerModel>>();

        return Ok(combatPlayers);
    }

    [HttpGet("combatPlayerById/{id}")]
    public async Task<IActionResult> GetCombatPlayerById(int id)
    {
        var responseMessage = await _httpClient.GetAsync($"CombatPlayer/{id}");
        var combatPlayer = await responseMessage.Content.ReadFromJsonAsync<CombatPlayerModel>();

        return Ok(combatPlayer);
    }

    [HttpGet("combatById/{id}")]
    public async Task<IActionResult> GetCombatById(int id)
    {
        var responseMessage = await _httpClient.GetAsync($"Combat/{id}");
        var combat = await responseMessage.Content.ReadFromJsonAsync<CombatModel>();

        return Ok(combat);
    }
}
