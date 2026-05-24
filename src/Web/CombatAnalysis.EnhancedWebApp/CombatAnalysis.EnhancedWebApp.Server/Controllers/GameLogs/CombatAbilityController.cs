using CombatAnalysis.EnhancedWebApp.Server.Consts;
using CombatAnalysis.EnhancedWebApp.Server.Interfaces;
using CombatAnalysis.EnhancedWebApp.Server.Models.GameLogs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace CombatAnalysis.EnhancedWebApp.Server.Controllers.GameLogs;

[Route("api/v1/[controller]")]
[ApiController]
public class CombatAbilityController : ControllerBase
{
    private readonly IHttpClientHelper _httpClient;

    public CombatAbilityController(IOptions<Cluster> cluster, IHttpClientHelper httpClient)
    {
        _httpClient = httpClient;
        _httpClient.APIUrl = cluster.Value.CombatParser;
    }

    [HttpGet]
    public async Task<IActionResult> GetByAbilityType(int combatPlayerId, int abilityType)
    {
        var responseMessage = await _httpClient.GetAsync($"CombatAbility?combatPlayerId={combatPlayerId}&abilityType={abilityType}");
        var abilitties = await responseMessage.Content.ReadFromJsonAsync<IEnumerable<CombatAbilityModel>>();

        return Ok(abilitties);
    }
}
