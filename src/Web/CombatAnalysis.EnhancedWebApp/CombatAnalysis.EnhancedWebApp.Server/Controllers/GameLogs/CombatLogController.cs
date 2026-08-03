using CombatAnalysis.EnhancedWebApp.Server.Attributes;
using CombatAnalysis.EnhancedWebApp.Server.Consts;
using CombatAnalysis.EnhancedWebApp.Server.Interfaces;
using CombatAnalysis.EnhancedWebApp.Server.Models.GameLogs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Net;

namespace CombatAnalysis.EnhancedWebApp.Server.Controllers.GameLogs;

[Route("api/v1/[controller]")]
[ApiController]
public class CombatLogController : ControllerBase
{
    private readonly IHttpClientHelper _httpClient;
    private readonly ILogger<CombatLogController> _logger;

    public CombatLogController(IOptions<Cluster> cluster, IHttpClientHelper httpClient, ILogger<CombatLogController> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
        _httpClient.APIUrl = cluster.Value.CombatParser;
    }

    [HttpGet("getByLogType")]
    public async Task<IActionResult> GetByLogType(int logType, string? appUserId)
    {
        var content = string.Empty;
        try
        {
            var responseMessage = await _httpClient.GetAsync($"CombatLog/getByLogType?logType={logType}&appUserId={appUserId}");
            if (responseMessage.StatusCode == HttpStatusCode.NoContent)
            {
                return NoContent();
            }
            if (responseMessage.StatusCode == HttpStatusCode.BadRequest)
            {
                return BadRequest();
            }

            content = await responseMessage.Content.ReadAsStringAsync();
            var combatLogs = await responseMessage.Content.ReadFromJsonAsync<IEnumerable<CombatLogModel>>();

            return Ok(combatLogs);
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, $"Error get Combats Logs. Content: {content}, Error: {ex.Message}");

            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("{id:int:min(1)}")]
    [ServiceFilter(typeof(RequireAccessTokenAttribute))]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var responseMessage = await _httpClient.DeletAsync($"CombatLog/{id}");
            responseMessage.EnsureSuccessStatusCode();

            return NoContent();
        }
        catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.Unauthorized)
        {
            _logger.LogError(ex, "Delete combat log {Id} failed. User should be authorize to delete combat log", id);

            return Unauthorized();
        }
        catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
        {
            _logger.LogError(ex, "Delete combat log {Id} failed. Combat log not found.", id);

            return NotFound();
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Delete combat log {Id} failed. Something wrong during deleting combat log.", id);

            return StatusCode((int)(ex.StatusCode ?? HttpStatusCode.InternalServerError), ex.Message);
        }
    }
}
