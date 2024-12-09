using CombatAnalysis.WebApp.Consts;
using CombatAnalysis.WebApp.Interfaces;
using CombatAnalysis.WebApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace CombatAnalysis.WebApp.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class DamageTakenGeneralController : ControllerBase
{
    private readonly IHttpClientHelper _httpClient;

    public DamageTakenGeneralController(IHttpClientHelper httpClient)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = Port.CombatParserApi;
    }

    [HttpGet("findByCombatPlayerId/{combatPlayerId:int:min(1)}")]
    public async Task<IActionResult> GetByCombatPlayerId(int combatPlayerId)
    {
        var responseMessage = await _httpClient.GetAsync($"DamageTakenGeneral/FindByCombatPlayerId/{combatPlayerId}");
        var damageTakenGenerals = await responseMessage.Content.ReadFromJsonAsync<IEnumerable<DamageTakenGeneralModel>>();

        return Ok(damageTakenGenerals);
    }
}
