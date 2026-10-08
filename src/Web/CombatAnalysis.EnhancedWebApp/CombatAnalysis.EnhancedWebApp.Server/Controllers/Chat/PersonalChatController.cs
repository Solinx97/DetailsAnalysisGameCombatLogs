using CombatAnalysis.EnhancedWebApp.Server.Attributes;
using CombatAnalysis.EnhancedWebApp.Server.Consts;
using CombatAnalysis.EnhancedWebApp.Server.Interfaces;
using CombatAnalysis.EnhancedWebApp.Server.Models.Chat;
using CombatAnalysis.EnhancedWebApp.Server.Patches;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Net;

namespace CombatAnalysis.EnhancedWebApp.Server.Controllers.Chat;

[ServiceFilter(typeof(RequireAccessTokenAttribute))]
[Route("api/v1/[controller]")]
[ApiController]
public class PersonalChatController : ControllerBase
{
    private readonly IHttpClientHelper _httpClient;
    private readonly ILogger<PersonalChatController> _logger;

    public PersonalChatController(IOptions<Cluster> cluster, IHttpClientHelper httpClient, ILogger<PersonalChatController> logger)
    {
        _httpClient = httpClient;
        _logger = logger;

        _httpClient.APIUrl = cluster.Value.Chat;
    }

    [HttpGet("{id:int:min(1)}")]
    public async Task<IActionResult> GetById(int id)
    {
        try
        {
            var responseMessage = await _httpClient.GetAsync($"PersonalChat/{id}");
            responseMessage.EnsureSuccessStatusCode();

            var personalChat = await responseMessage.Content.ReadFromJsonAsync<PersonalChatModel>();

            return Ok(personalChat);
        }
        catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.Unauthorized)
        {
            _logger.LogError(ex, "Get personal chat {Id} failed. User should be authorize to get personal chat", id);

            return Unauthorized();
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Get personal chat {Id} failed: received unsuccessful request", id);

            return StatusCode((int)(ex.StatusCode ?? HttpStatusCode.InternalServerError), ex.Message);
        }
    }

    [HttpGet("getByUserId/{userId:minlength(8)}")]
    public async Task<IActionResult> GetByUserId(string userId)
    {
        try
        {
            var responseMessage = await _httpClient.GetAsync($"PersonalChat/getByUserId/{userId}");
            responseMessage.EnsureSuccessStatusCode();

            var personalChats = await responseMessage.Content.ReadFromJsonAsync<IEnumerable<PersonalChatModel>>();

            return Ok(personalChats);
        }
        catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.Unauthorized)
        {
            _logger.LogError(ex, "Get personal chat by user {UserId} failed. User should be authorize to get personal chat by user", userId);

            return Unauthorized();
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Get personal chat by user {UserId} failed: received unsuccessful request", userId);

            return StatusCode((int)(ex.StatusCode ?? HttpStatusCode.InternalServerError), ex.Message);
        }
    }

    [HttpGet("isExist")]
    public async Task<IActionResult> IsExist(string initiatorId, string companionId)
    {
        try
        {
            var responseMessage = await _httpClient.GetAsync("PersonalChat");
            responseMessage.EnsureSuccessStatusCode();

            var personalChats = await responseMessage.Content.ReadFromJsonAsync<IEnumerable<PersonalChatModel>>();
            var chats = personalChats?.Where(x => x.InitiatorId == initiatorId && x.CompanionId == companionId).ToList();
            if (chats != null && chats.Count == 0)
            {
                chats = personalChats?.Where(x => x.CompanionId == companionId && x.InitiatorId == initiatorId).ToList();
                if (chats != null && chats.Count == 0)
                {
                    return Ok(false);
                }
            }

            return Ok(true);
        }
        catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.Unauthorized)
        {
            _logger.LogError(ex, "Check if personal chat already exist failed. User should be authorize to check if personal chat already exist");

            return Unauthorized();
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Check if personal chat already exist failed: received unsuccessful request");

            return StatusCode((int)(ex.StatusCode ?? HttpStatusCode.InternalServerError), ex.Message);
        }
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] PersonalChatModel chat)
    {
        try
        {
            var responseMessage = await _httpClient.PostAsync("PersonalChat", JsonContent.Create(chat));
            responseMessage.EnsureSuccessStatusCode();

            var personalChat = await responseMessage.Content.ReadFromJsonAsync<PersonalChatModel>();
            return Ok(personalChat);
        }
        catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.Unauthorized)
        {
            _logger.LogError(ex, "Create personal chat failed. User should be authorize to create personal chat");

            return Unauthorized();
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Create personal chat failed. Something wrong during creating personal chat.");

            return StatusCode((int)(ex.StatusCode ?? HttpStatusCode.InternalServerError), ex.Message);
        }
    }

    [HttpPatch("{id:int:min(1)}")]
    public async Task<IActionResult> PartialUpdate(int id, [FromBody] PersonalChatPatch chat)
    {
        try
        {
            var responseMessage = await _httpClient.PatchAsync($"PersonalChat/{id}", JsonContent.Create(chat));
            responseMessage.EnsureSuccessStatusCode();

            return NoContent();
        }
        catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.Unauthorized)
        {
            _logger.LogError(ex, "Update personal chat {Id} failed. User should be authorize to update personal chat", id);

            return Unauthorized();
        }
        catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
        {
            _logger.LogError(ex, "Update personal chat {Id} failed. Personal chat not found.", id);

            return NotFound();
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Update personal chat {Id} failed. Something wrong during updating personal chat.", id);

            return StatusCode((int)(ex.StatusCode ?? HttpStatusCode.InternalServerError), ex.Message);
        }
    }

    [HttpDelete("{id:int:min(1)}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var responseMessage = await _httpClient.DeleteAsync($"PersonalChat/{id}");
            responseMessage.EnsureSuccessStatusCode();

            return NoContent();
        }
        catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.Unauthorized)
        {
            _logger.LogError(ex, "Delete personal chat {Id} failed. User should be authorize to delete personal chat", id);

            return Unauthorized();
        }
        catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
        {
            _logger.LogError(ex, "Delete personal chat {Id} failed. Personal chat not found.", id);

            return NotFound();
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Delete personal chat {Id} failed. Something wrong during deleting personal chat.", id);

            return StatusCode((int)(ex.StatusCode ?? HttpStatusCode.InternalServerError), ex.Message);
        }
    }
}
