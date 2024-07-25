using CombatAnalysis.WebApp.Consts;
using CombatAnalysis.WebApp.Interfaces;
using CombatAnalysis.WebApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace CombatAnalysis.WebApp.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class PlayerDeathController : ControllerBase
{
    private readonly IHttpClientHelper _httpClient;

    public PlayerDeathController(IHttpClientHelper httpClient)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = Port.CombatParserApi;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var responseMessage = await _httpClient.GetAsync($"PlayerDeath/FindByCombatPlayerId/{id}");
        var damageDones = await responseMessage.Content.ReadFromJsonAsync<IEnumerable<PlayerDeathModel>>();

        return Ok(damageDones);
    }
}
