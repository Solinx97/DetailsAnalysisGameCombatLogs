using CombatAnalysis.WebApp.Consts;
using CombatAnalysis.WebApp.Interfaces;
using CombatAnalysis.WebApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace CombatAnalysis.WebApp.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class CombatPlayerController : ControllerBase
{
    private readonly IHttpClientHelper _httpClient;

    public CombatPlayerController(IHttpClientHelper httpClient)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = Port.CombatParserApi;
    }

    [HttpGet("findByCombatId/{combatId:int:min(1)}")]
    public async Task<IActionResult> GetCombatPlayersByCombatId(int combatId)
    {
        var responseMessage = await _httpClient.GetAsync($"CombatPlayer/findByCombatId/{combatId}");
        var combatPlayers = await responseMessage.Content.ReadFromJsonAsync<IEnumerable<CombatPlayerModel>>();

        return Ok(combatPlayers);
    }

    [HttpGet("{id:int:min(1)}")]
    public async Task<IActionResult> GetById(int id)
    {
        var responseMessage = await _httpClient.GetAsync($"CombatPlayer/{id}");
        var combatPlayer = await responseMessage.Content.ReadFromJsonAsync<CombatPlayerModel>();

        return Ok(combatPlayer);
    }
}
