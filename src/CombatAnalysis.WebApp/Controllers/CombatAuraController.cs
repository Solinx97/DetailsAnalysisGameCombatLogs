using CombatAnalysis.WebApp.Consts;
using CombatAnalysis.WebApp.Interfaces;
using CombatAnalysis.WebApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace CombatAnalysis.WebApp.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class CombatAuraController : ControllerBase
{
    private readonly IHttpClientHelper _httpClient;

    public CombatAuraController(IHttpClientHelper httpClient)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = Port.CombatParserApi;
    }

    [HttpGet("findByCombatId/{combatId:int:min(1)}")]
    public async Task<IActionResult> Find(int combatId)
    {
        var response = await _httpClient.GetAsync($"CombatAura/findByCombatId/{combatId}");
        var combatAuras = await response.Content.ReadFromJsonAsync<IEnumerable<CombatAuraModel>>();

        return Ok(combatAuras);
    }
}