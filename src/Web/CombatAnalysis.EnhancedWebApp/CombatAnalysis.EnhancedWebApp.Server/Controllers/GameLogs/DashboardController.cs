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

    [HttpGet("getDPS/{combatLogId:int:min(1)}")]
    public async Task<IActionResult> GetDPS(int combatLogId, int combatId, string unitName)
    {
        var responseMessag = await _httpClient.GetAsync($"Dashboard/getDPS/{combatLogId}?combatId={combatId}&unitName={unitName}");
        var dashboard = await responseMessag.Content.ReadFromJsonAsync<DashboardModel>();

        return Ok(dashboard);
    }

    [HttpGet("getHPS/{combatLogId:int:min(1)}")]
    public async Task<IActionResult> GetHPS(int combatLogId, int combatId, string unitName)
    {
        var responseMessag = await _httpClient.GetAsync($"Dashboard/getHPS/{combatLogId}?combatId={combatId}&unitName={unitName}");
        var dashboard = await responseMessag.Content.ReadFromJsonAsync<DashboardModel>();

        return Ok(dashboard);
    }

    [HttpGet("getDamageSpells/{combatLogId:int:min(1)}")]
    public async Task<IActionResult> GetDamageSpells(int combatLogId, int combatId, string unitName)
    {
        var responseMessage = await _httpClient.GetAsync($"Dashboard/getDamageSpells/{combatLogId}?combatId={combatId}&unitName={unitName}");
        var dashboard = await responseMessage.Content.ReadFromJsonAsync<DashboardModel>();

        return Ok(dashboard);
    }

    [HttpGet("getHealSpells/{combatLogId:int:min(1)}")]
    public async Task<IActionResult> GetHealSpells(int combatLogId, int combatId, string unitName)
    {
        var responseMessage = await _httpClient.GetAsync($"Dashboard/getHealSpells/{combatLogId}?combatId={combatId}&unitName={unitName}");
        var spells = await responseMessage.Content.ReadFromJsonAsync<DashboardModel>();

        return Ok(spells);
    }

    [HttpGet("getPotions/{combatLogId:int:min(1)}")]
    public async Task<IActionResult> GetPotions(int combatLogId)
    {
        var responseMessage = await _httpClient.GetAsync($"Dashboard/getPotions/{combatLogId}");
        var potions = await responseMessage.Content.ReadFromJsonAsync<Dictionary<string, int>>();

        return Ok(potions);
    }
}
