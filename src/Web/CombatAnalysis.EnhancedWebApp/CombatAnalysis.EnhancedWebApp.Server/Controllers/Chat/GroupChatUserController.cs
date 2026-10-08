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
public class GroupChatUserController : ControllerBase
{
    private readonly IHttpClientHelper _httpClient;
    private readonly ILogger<GroupChatUserController> _logger;

    public GroupChatUserController(IOptions<Cluster> cluster, IHttpClientHelper httpClient, ILogger<GroupChatUserController> logger)
    {
        _httpClient = httpClient;
        _logger = logger;

        _httpClient.APIUrl = cluster.Value.Chat;
    }

    [HttpGet("{id:minlength(8)}")]
    public async Task<IActionResult> GetById(string id)
    {
        try
        {
            var responseMessage = await _httpClient.GetAsync($"GroupChatUser/{id}");
            responseMessage.EnsureSuccessStatusCode();

            var groupChatUser = await responseMessage.Content.ReadFromJsonAsync<GroupChatUserModel>();

            return Ok(groupChatUser);

        }
        catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.Unauthorized)
        {
            _logger.LogError(ex, "Get group chat user {Id} failed. User should be authorize to get chat user.", id);

            return Unauthorized();
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Get group chat user {Id} failed. Something wrong during getting group chat user.", id);

            return StatusCode((int)(ex.StatusCode ?? HttpStatusCode.InternalServerError), ex.Message);
        }
    }

    [HttpGet("findByAppUserId")]
    public async Task<IActionResult> FindByAppUserId(int chatId, string appUserId)
    {
        try
        {
            var responseMessage = await _httpClient.GetAsync($"GroupChatUser/findByAppUserId?chatId={chatId}&appUserId={appUserId}");
            responseMessage.EnsureSuccessStatusCode();

            var groupChatUser = await responseMessage.Content.ReadFromJsonAsync<GroupChatUserModel>();

            return Ok(groupChatUser);
        }
        catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.Unauthorized)
        {
            _logger.LogError(ex, "Find chat user by app user {AppUserId} for chat {ChatId} failed. User should be authorize to find chat user by app user.", appUserId, chatId);
            
            return Unauthorized();
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Get group chat user by app user {Id} for chat {ChatId} failed. Something wrong during getting group chat user by app user.", appUserId, chatId);

            return StatusCode((int)(ex.StatusCode ?? HttpStatusCode.InternalServerError), ex.Message);
        }
    }

    [HttpGet("findAll/{chatId:int:min(1)}")]
    public async Task<IActionResult> FindAll(int chatId)
    {
        try
        {
            var responseMessage = await _httpClient.GetAsync($"GroupChatUser/findAll/{chatId}");
            responseMessage.EnsureSuccessStatusCode();

            var groupChatUsers = await responseMessage.Content.ReadFromJsonAsync<IEnumerable<GroupChatUserModel>>();

            return Ok(groupChatUsers);
        }
        catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.Unauthorized)
        {
            _logger.LogError(ex, "Find all users in chat {ChatId} failed. User should be authorize to find all users in chat.", chatId);
            
            return Unauthorized();
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Find all users in chat {ChatId} failed. Something wrong during getting all group chat users in chat.", chatId);

            return StatusCode((int)(ex.StatusCode ?? HttpStatusCode.InternalServerError), ex.Message);
        }
    }


    [HttpGet("findAllByAppUserId/{appUserId:minlength(8)}")]
    public async Task<IActionResult> FindByUserId(string appUserId)
    {
        try
        {
            var responseMessage = await _httpClient.GetAsync($"GroupChatUser/findAllByAppUserId/{appUserId}");
            responseMessage.EnsureSuccessStatusCode();

            var groupChatUsers = await responseMessage.Content.ReadFromJsonAsync<IEnumerable<GroupChatUserModel>>();

            return Ok(groupChatUsers);
        }
        catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.Unauthorized)
        {
            _logger.LogError(ex, "Find group chat users by app user {AppUserId} failed. User should be authorize to find chat users by app user", appUserId);
            
            return Unauthorized();
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Find group chat users by app user {AppUserId} failed. Something wrong during getting group chat users by app user.", appUserId);
            
            return StatusCode((int)(ex.StatusCode ?? HttpStatusCode.InternalServerError), ex.Message);
        }
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] GroupChatUserModel user)
    {
        try
        {
            var responseMessage = await _httpClient.PostAsync("GroupChatUser", JsonContent.Create(user));
            responseMessage.EnsureSuccessStatusCode();

            var groupChatUser = await responseMessage.Content.ReadFromJsonAsync<GroupChatUserModel>();

            return Ok(groupChatUser);
        }
        catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.Unauthorized)
        {
            _logger.LogError(ex, "Create chat user failed. User should be authorize to create chat user.");
            
            return Unauthorized();
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Create chat user failed. Something wrong during creating group chat user.");
            
            return StatusCode((int)(ex.StatusCode ?? HttpStatusCode.InternalServerError), ex.Message);
        }
    }

    [HttpDelete("{id:minlength(8)}")]
    public async Task<IActionResult> Delete(string id)
    {
        try
        {
            var responseMessage = await _httpClient.DeleteAsync($"GroupChatUser/{id}");
            responseMessage.EnsureSuccessStatusCode();

            return NoContent();
        }
        catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.Unauthorized)
        {
            _logger.LogError(ex, "Delete group chat user {Id} failed. User should be authorize to delete chat user", id);

            return Unauthorized();
        }
        catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
        {
            _logger.LogError(ex, "Delete group chat user {Id} failed. Group chat user not found.", id);

            return NotFound();
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Delete group chat user {Id} failed. Something wrong during deleting group chat user.", id);

            return StatusCode((int)(ex.StatusCode ?? HttpStatusCode.InternalServerError), ex.Message);
        }
    }
}
