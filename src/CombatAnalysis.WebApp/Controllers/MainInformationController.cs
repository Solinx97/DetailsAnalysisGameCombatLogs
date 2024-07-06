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
    private readonly ILogger<MainInformationController> _logger;

    public MainInformationController(IHttpClientHelper httpClient, ILogger<MainInformationController> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
        _httpClient.BaseAddress = Port.CombatParserApi;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var content = string.Empty;
        try
        {
            var responseMessage = await _httpClient.GetAsync("CombatLog");
            if (responseMessage.StatusCode == System.Net.HttpStatusCode.NoContent)
            {
                return NoContent();
            }
            if (responseMessage.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                return BadRequest();
            }

            content = await responseMessage.Content.ReadAsStringAsync();
            var combatLogs = await responseMessage.Content.ReadFromJsonAsync<IEnumerable<CombatLogModel>>();

            return Ok(combatLogs);
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, $"Error get all Combats Logs. Content: {content}, Error: {ex.Message}");

            return BadRequest(ex.Message);
        }
    }
}
