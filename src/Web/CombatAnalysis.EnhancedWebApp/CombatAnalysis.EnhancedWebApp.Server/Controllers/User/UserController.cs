using CombatAnalysis.EnhancedWebApp.Server.Attributes;
using CombatAnalysis.EnhancedWebApp.Server.Consts;
using CombatAnalysis.EnhancedWebApp.Server.Interfaces;
using CombatAnalysis.EnhancedWebApp.Server.Models.User;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Net;

namespace CombatAnalysis.EnhancedWebApp.Server.Controllers.User;

[Route("api/v1/[controller]")]
[ApiController]
[ServiceFilter(typeof(RequireAccessTokenAttribute))]
public class UserController : ControllerBase
{
    private readonly IHttpClientHelper _httpClient;
    private readonly ILogger<UserController> _logger;

    public UserController(IOptions<Cluster> cluster, IHttpClientHelper httpClient, ILogger<UserController> logger)
    {
        _httpClient = httpClient;
        _httpClient.APIUrl = cluster.Value.User;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var responseMessage = await _httpClient.GetAsync("User");
            responseMessage.EnsureSuccessStatusCode();

            var users = await responseMessage.Content.ReadFromJsonAsync<IEnumerable<AppUserModel>>();

            return Ok(users);
        }
        catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.Unauthorized)
        {
            _logger.LogError(ex, "Get users failed. User should be authorize to see other users.");

            return Unauthorized();
        }
        catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
        {
            _logger.LogError(ex, "Get users failed. Users not found.");

            return NotFound();
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Get users failed. Something wrong during getting users.");

            return StatusCode((int)(ex.StatusCode ?? HttpStatusCode.InternalServerError), ex.Message);
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(string id)
    {
        try
        {
            var responseMessage = await _httpClient.GetAsync($"User/{id}");
            responseMessage.EnsureSuccessStatusCode();

            var user = await responseMessage.Content.ReadFromJsonAsync<AppUserModel>();

            return Ok(user);
        }
        catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.Unauthorized)
        {
            _logger.LogError(ex, "Get user {Id} failed. User should be authorize to see this user.", id);

            return Unauthorized();
        }
        catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
        {
            _logger.LogError(ex, "Get user {Id} failed. User not found.", id);

            return NotFound();
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Get user {Id} failed. Something wrong during getting user.", id);

            return StatusCode((int)(ex.StatusCode ?? HttpStatusCode.InternalServerError), ex.Message);
        }
    }

    [HttpGet("getByIdentityUserId/{identityUserId}")]
    public async Task<IActionResult> GetByIdentityUserId(string identityUserId)
    {
        try
        {
            var responseMessage = await _httpClient.GetAsync($"User/getByIdentityUserId/{identityUserId}");
            responseMessage.EnsureSuccessStatusCode();

            var user = await responseMessage.Content.ReadFromJsonAsync<AppUserModel>();

            return Ok(user);
        }
        catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.Unauthorized)
        {
            _logger.LogError(ex, "Get user {Id} failed. User should be authorize to see this user.", identityUserId);

            return Unauthorized();
        }
        catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
        {
            _logger.LogError(ex, "Get user {Id} failed. User not found.", identityUserId);

            return NotFound();
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Get user {Id} failed. Something wrong during getting user.", identityUserId);

            return StatusCode((int)(ex.StatusCode ?? HttpStatusCode.InternalServerError), ex.Message);
        }
    }

    [HttpGet("find")]
    public async Task<IActionResult> FindByUsernameStartAt(string startAt)
    {
        try
        {
            var responseMessage = await _httpClient.GetAsync($"User/findByUsername?startAt={startAt}");
            responseMessage.EnsureSuccessStatusCode();

            var users = await responseMessage.Content.ReadFromJsonAsync<IEnumerable<AppUserModel>>();

            return Ok(users);
        }
        catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.Unauthorized)
        {
            _logger.LogError(ex, "Find user by username start at failed. User should be authorize to do searching.");

            return Unauthorized();
        }
        catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
        {
            _logger.LogError(ex, "Find user by username start at failed. User not found.");

            return NotFound();
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Find user by username start at failed. Something wrong during searching user.");

            return StatusCode((int)(ex.StatusCode ?? HttpStatusCode.InternalServerError), ex.Message);
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(string id, [FromBody] AppUserModel user)
    {
        try
        {
            var responseMessage = await _httpClient.PutAsync($"User/{id}", JsonContent.Create(user));
            responseMessage.EnsureSuccessStatusCode();

            return NoContent();
        }
        catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.Unauthorized)
        {
            _logger.LogError(ex, "Update account {Id} failed. User should be authorize to update account.", id);

            return Unauthorized();
        }
        catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
        {
            _logger.LogError(ex, "Update account {Id} failed. Account not found.", id);

            return NotFound();
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Update account {Id} failed. Something wrong during updating account.", id);

            return StatusCode((int)(ex.StatusCode ?? HttpStatusCode.InternalServerError), ex.Message);
        }
    }
}
