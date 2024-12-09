using CombatAnalysis.WebApp.Consts;
using CombatAnalysis.WebApp.Interfaces;
using CombatAnalysis.WebApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace CombatAnalysis.WebApp.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class DamageDoneGeneralController : ControllerBase
{
    private readonly IHttpClientHelper _httpClient;

    public DamageDoneGeneralController(IHttpClientHelper httpClient)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = Port.CombatParserApi;
    }

    [HttpGet("findByCombatPlayerId/{combatPlayerId:int:min(1)}")]
    public async Task<IEnumerable<DamageDoneGeneralModel>> GetByCombatPlayerId(int combatPlayerId)
    {
        var responseMessage = await _httpClient.GetAsync($"DamageDoneGeneral/findByCombatPlayerId/{combatPlayerId}");
        var damageDoneGenerals = await responseMessage.Content.ReadFromJsonAsync<IEnumerable<DamageDoneGeneralModel>>();

        return damageDoneGenerals;
    }
}
