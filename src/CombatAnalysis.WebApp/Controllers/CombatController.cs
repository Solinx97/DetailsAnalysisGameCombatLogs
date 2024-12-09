using CombatAnalysis.WebApp.Consts;
using CombatAnalysis.WebApp.Interfaces;
using CombatAnalysis.WebApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace CombatAnalysis.WebApp.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class CombatController : ControllerBase
{
    private readonly IHttpClientHelper _httpClient;

    public CombatController(IHttpClientHelper httpClient)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = Port.CombatParserApi;
    }

    [HttpGet("findByCombatLogId/{combatLogId:int:min(1)}")]
    public async Task<IActionResult> GetCombatsByCombatLogId(int combatLogId)
    {
        var responseMessage = await _httpClient.GetAsync($"Combat/findByCombatLogId/{combatLogId}");
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
