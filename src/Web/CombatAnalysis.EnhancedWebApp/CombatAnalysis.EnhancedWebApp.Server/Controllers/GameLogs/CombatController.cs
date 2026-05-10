using CombatAnalysis.EnhancedWebApp.Server.Consts;
using CombatAnalysis.EnhancedWebApp.Server.Interfaces;
using CombatAnalysis.EnhancedWebApp.Server.Models.GameLogs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace CombatAnalysis.EnhancedWebApp.Server.Controllers.GameLogs;

[Route("api/v1/[controller]")]
[ApiController]
public class CombatController : ControllerBase
{
    private readonly IHttpClientHelper _httpClient;

    public CombatController(IOptions<Cluster> cluster, IHttpClientHelper httpClient)
    {
        _httpClient = httpClient;
        _httpClient.APIUrl = cluster.Value.CombatParser;
    }

    [HttpGet("getByCombatLogId/{combatLogId:int:min(1)}")]
    public async Task<IActionResult> GetByCombatLogId(int combatLogId)
    {
        var responseMessage = await _httpClient.GetAsync($"Combat/getByCombatLogId/{combatLogId}");
        var combats = await responseMessage.Content.ReadFromJsonAsync<IEnumerable<CombatModel>>();

        return Ok(combats);
    }

    [HttpGet("{id:int:min(1)}")]
    public async Task<IActionResult> GetById(int id)
    {
        var responseMessage = await _httpClient.GetAsync($"Combat/{id}");
        var combat = await responseMessage.Content.ReadFromJsonAsync<CombatModel>();

        return Ok(combat);
    }
}
