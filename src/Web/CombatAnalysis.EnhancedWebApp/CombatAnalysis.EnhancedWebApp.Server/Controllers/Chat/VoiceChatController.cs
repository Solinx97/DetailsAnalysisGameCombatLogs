using CombatAnalysis.EnhancedWebApp.Server.Attributes;
using CombatAnalysis.EnhancedWebApp.Server.Consts;
using CombatAnalysis.EnhancedWebApp.Server.Interfaces;
using CombatAnalysis.EnhancedWebApp.Server.Models.Chat;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Net;

namespace CombatAnalysis.EnhancedWebApp.Server.Controllers.Chat;

[ServiceFilter(typeof(RequireAccessTokenAttribute))]
[Route("api/v1/[controller]")]
[ApiController]
public class VoiceChatController : ControllerBase
{
    private readonly IHttpClientHelper _httpClient;
    private readonly ILogger<VoiceChatController> _logger;

    public VoiceChatController(IOptions<Cluster> cluster, IHttpClientHelper httpClient, ILogger<VoiceChatController> logger)
    {
        _httpClient = httpClient;
        _logger = logger;

        _httpClient.APIUrl = cluster.Value.Chat;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var responseMessage = await _httpClient.GetAsync("VoiceChat");
            if (responseMessage.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                return Unauthorized();
            }
            else if (responseMessage.IsSuccessStatusCode)
            {
                var groupChats = await responseMessage.Content.ReadFromJsonAsync<IEnumerable<VoiceChatModel>>();

                return Ok(groupChats);
            }

            return BadRequest();
        }
        catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.Unauthorized)
        {
            _logger.LogError(ex, "Get all voice chats failed. User should be authorize to get all voice chats.");

            return Unauthorized();
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Get all voice chats failed. Something wrong during getting all voice chats.");

            return StatusCode((int)(ex.StatusCode ?? HttpStatusCode.InternalServerError), ex.Message);
        }
    }

    [HttpGet("{id:minlength(8)}")]
    public async Task<IActionResult> GetById(string id)
    {
        try
        {
            var responseMessage = await _httpClient.GetAsync($"VoiceChat/{id}");
            responseMessage.EnsureSuccessStatusCode();

            var voiceChat = await responseMessage.Content.ReadFromJsonAsync<VoiceChatModel>();

            return Ok(voiceChat);
        }
        catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.Unauthorized)
        {
            _logger.LogError(ex, "Get voice chat {Id} failed. User should be authorize to get voice chat.", id);

            return Unauthorized();
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Get voice chat {Id} failed. Something wrong during getting voice chat.", id);

            return StatusCode((int)(ex.StatusCode ?? HttpStatusCode.InternalServerError), ex.Message);
        }
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] VoiceChatModel chat)
    {
        try
        {
            var responseMessage = await _httpClient.PostAsync("VoiceChat", JsonContent.Create(chat));
            responseMessage.EnsureSuccessStatusCode();

            var vocieChat = await responseMessage.Content.ReadFromJsonAsync<VoiceChatModel>();

            return Ok(vocieChat);
        }
        catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.Unauthorized)
        {
            _logger.LogError(ex, "Create voice chat failed. User should be authorize to create voice chat.");

            return Unauthorized();
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Create voice chat failed. Something wrong during creating voice chat.");

            return StatusCode((int)(ex.StatusCode ?? HttpStatusCode.InternalServerError), ex.Message);
        }
    }

    [HttpPut("{id:minlength(8)}")]
    public async Task<IActionResult> Update(string id, VoiceChatModel chat)
    {
        try
        {
            var responseMessage = await _httpClient.PutAsync($"VoiceChat/{id}", JsonContent.Create(chat));
            responseMessage.EnsureSuccessStatusCode();

            return NoContent();
        }
        catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.Unauthorized)
        {
            _logger.LogError(ex, "Update voice chat {Id} failed. User should be authorize to voice chat.", id);

            return Unauthorized();
        }
        catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
        {
            _logger.LogError(ex, "Update voice chat {Id} failed. Voice chat not found.", id);

            return NotFound();
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Update voice chat {Id} failed. Something wrong during updating voice chat.", id);

            return StatusCode((int)(ex.StatusCode ?? HttpStatusCode.InternalServerError), ex.Message);
        }
    }

    [HttpDelete("{id:minlength(8)}")]
    public async Task<IActionResult> Delete(string id)
    {
        try
        {
            var responseMessage = await _httpClient.DeleteAsync($"VoiceChat/{id}");
            responseMessage.EnsureSuccessStatusCode();

            return NoContent();
        }
        catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.Unauthorized)
        {
            _logger.LogError(ex, "Delete voice chat {Id} failed. User should be authorize to delete voice chat.", id);

            return Unauthorized();
        }
        catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
        {
            _logger.LogError(ex, "Delete voice chat {Id} failed. Voice chat not found.", id);

            return NotFound();
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Delete voice chat {Id} failed. Something wrong during deleting voice chat.", id);

            return StatusCode((int)(ex.StatusCode ?? HttpStatusCode.InternalServerError), ex.Message);
        }
    }
}
