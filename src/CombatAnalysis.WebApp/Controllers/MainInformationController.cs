using CombatAnalysis.WebApp.Consts;
using CombatAnalysis.WebApp.Interfaces;
using CombatAnalysis.WebApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace CombatAnalysis.WebApp.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class MainInformationController : ControllerBase
{
    private readonly IHttpClientHelper _httpClient;

    public MainInformationController(IHttpClientHelper httpClient)
    {
        _httpClient = httpClient;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        _httpClient.BaseAddress = Port.CombatParserApi;

        var responseMessage = await _httpClient.GetAsync("CombatLog");
        var combatLogs = await responseMessage.Content.ReadFromJsonAsync<IEnumerable<CombatLogModel>>();

        return Ok(combatLogs);
    }
}
