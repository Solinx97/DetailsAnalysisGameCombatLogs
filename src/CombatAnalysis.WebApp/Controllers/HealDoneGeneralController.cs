using CombatAnalysis.WebApp.Consts;
using CombatAnalysis.WebApp.Interfaces;
using CombatAnalysis.WebApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace CombatAnalysis.WebApp.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class HealDoneGeneralController : ControllerBase
{
    private readonly IHttpClientHelper _httpClient;

    public HealDoneGeneralController(IHttpClientHelper httpClient)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = Port.CombatParserApi;
    }

    [HttpGet("findByCombatPlayerId/{combatPlayerId:int:min(1)}")]
    public async Task<IActionResult> GetByCombatPlayerId(int combatPlayerId)
    {
        var responseMessage = await _httpClient.GetAsync($"HealDoneGeneral/findByCombatPlayerId/{combatPlayerId}");
        var healDoneGenerals = await responseMessage.Content.ReadFromJsonAsync<IEnumerable<HealDoneGeneralModel>>();

        return Ok(healDoneGenerals);
    }
}
