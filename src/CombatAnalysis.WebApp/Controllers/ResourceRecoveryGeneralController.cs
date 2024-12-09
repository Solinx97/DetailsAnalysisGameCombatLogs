using CombatAnalysis.WebApp.Consts;
using CombatAnalysis.WebApp.Interfaces;
using CombatAnalysis.WebApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace CombatAnalysis.WebApp.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class ResourceRecoveryGeneralController : ControllerBase
{
    private readonly IHttpClientHelper _httpClient;

    public ResourceRecoveryGeneralController(IHttpClientHelper httpClient)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = Port.CombatParserApi;
    }

    [HttpGet("findByCombatPlayerId/{combatPlayerId:int:min(1)}")]
    public async Task<IActionResult> GetByCombatPlayerId(int combatPlayerId)
    {
        var responseMessage = await _httpClient.GetAsync($"ResourceRecoveryGeneral/FindByCombatPlayerId/{combatPlayerId}");
        var resourceRecoveryGenerals = await responseMessage.Content.ReadFromJsonAsync<IEnumerable<ResourceRecoveryGeneralModel>>();

        return Ok(resourceRecoveryGenerals);
    }
}
