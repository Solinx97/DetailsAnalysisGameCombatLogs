using CombatAnalysis.WebApp.Consts;
using CombatAnalysis.WebApp.Interfaces;
using CombatAnalysis.WebApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace CombatAnalysis.WebApp.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class DamageTakenController : ControllerBase
{
    private readonly IHttpClientHelper _httpClient;

    public DamageTakenController(IHttpClientHelper httpClient)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = Port.CombatParserApi;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var responseMessage = await _httpClient.GetAsync($"DamageTaken/FindByCombatPlayerId/{id}");
        var damageTakens = await responseMessage.Content.ReadFromJsonAsync<IEnumerable<DamageTakenModel>>();

        return Ok(damageTakens);
    }
}
