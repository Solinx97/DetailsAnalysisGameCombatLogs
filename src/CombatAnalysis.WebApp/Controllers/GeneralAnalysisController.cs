using CombatAnalysis.WebApp.Consts;
using CombatAnalysis.WebApp.Interfaces;
using CombatAnalysis.WebApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace CombatAnalysis.WebApp.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class GeneralAnalysisController : ControllerBase
{
    private readonly IHttpClientHelper _httpClient;

    public GeneralAnalysisController(IHttpClientHelper httpClient)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = Port.CombatParserApi;
    }

    [HttpGet("findByCombatLogId/{combatLogId}")]
    public async Task<IActionResult> GetByCombatLogIdId(int combatLogId)
    {
        var responseMessage = await _httpClient.GetAsync($"Combat/findByCombatLogId/{combatLogId}");
        var combats = await responseMessage.Content.ReadFromJsonAsync<IEnumerable<CombatModel>>();

        return Ok(combats);
    }
}
