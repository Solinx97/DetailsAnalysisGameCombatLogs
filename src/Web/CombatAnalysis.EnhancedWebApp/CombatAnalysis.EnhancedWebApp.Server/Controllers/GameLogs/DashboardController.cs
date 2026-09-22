using CombatAnalysis.EnhancedWebApp.Server.Consts;
using CombatAnalysis.EnhancedWebApp.Server.Interfaces;
using CombatAnalysis.EnhancedWebApp.Server.Models.GameLogs.Dashboard;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace CombatAnalysis.EnhancedWebApp.Server.Controllers.GameLogs;

[Route("api/v1/[controller]")]
[ApiController]
public class DashboardController : ControllerBase
{
    private readonly IHttpClientHelper _httpClient;

    public DashboardController(IOptions<Cluster> cluster, IHttpClientHelper httpClient)
    {
        _httpClient = httpClient;
        _httpClient.APIUrl = cluster.Value.CombatParser;
    }

    [HttpGet("getDamage/{combatLogId:int:min(1)}")]
    public async Task<IActionResult> GetDamage(int combatLogId, string bossName, int combatId, string creatorName, string targetName, int valueType, CancellationToken cancellationToken)
    {
        var responseMessage = await _httpClient.GetAsync($"Dashboard/getDamage/{combatLogId}?bossName={bossName}&combatId={combatId}&creatorName={creatorName}&targetName={targetName}&valueType={valueType}", cancellationToken);
        var dashboard = await responseMessage.Content.ReadFromJsonAsync<DashboardModel>(cancellationToken);

        return Ok(dashboard);
    }

    [HttpGet("getHeal/{combatLogId:int:min(1)}")]
    public async Task<IActionResult> GetHeal(int combatLogId, string bossName, int combatId, string creatorName, string targetName, int valueType, CancellationToken cancellationToken)
    {
        var responseMessage = await _httpClient.GetAsync($"Dashboard/getHeal/{combatLogId}?bossName={bossName}&combatId={combatId}&creatorName={creatorName}&targetName={targetName}&valueType={valueType}", cancellationToken);
        var dashboard = await responseMessage.Content.ReadFromJsonAsync<DashboardModel>(cancellationToken);

        return Ok(dashboard);
    }

    [HttpGet("getDamageSpells/{combatLogId:int:min(1)}")]
    public async Task<IActionResult> GetDamageSpells(int combatLogId, string bossName, int combatId, CancellationToken cancellationToken)
    {
        var responseMessage = await _httpClient.GetAsync($"Dashboard/getDamageSpells/{combatLogId}?bossName={bossName}&combatId={combatId}", cancellationToken);
        var dashboard = await responseMessage.Content.ReadFromJsonAsync<DashboardModel>(cancellationToken);

        return Ok(dashboard);
    }

    [HttpGet("getHealSpells/{combatLogId:int:min(1)}")]
    public async Task<IActionResult> GetHealSpells(int combatLogId, string bossName, int combatId, CancellationToken cancellationToken)
    {
        var responseMessage = await _httpClient.GetAsync($"Dashboard/getHealSpells/{combatLogId}?bossName={bossName}&combatId={combatId}", cancellationToken);
        var spells = await responseMessage.Content.ReadFromJsonAsync<DashboardModel>(cancellationToken);

        return Ok(spells);
    }

    [HttpGet("getPotions/{combatLogId:int:min(1)}")]
    public async Task<IActionResult> GetPotions(int combatLogId, CancellationToken cancellationToken)
    {
        var responseMessage = await _httpClient.GetAsync($"Dashboard/getPotions/{combatLogId}", cancellationToken);
        var potions = await responseMessage.Content.ReadFromJsonAsync<Dictionary<string, int>>(cancellationToken);

        return Ok(potions);
    }
}
