using CombatAnalysis.WebApp.Consts;
using CombatAnalysis.WebApp.Extensions;
using CombatAnalysis.WebApp.Interfaces;
using CombatAnalysis.WebApp.Models.User;
using Microsoft.AspNetCore.Mvc;

namespace CombatAnalysis.WebApp.Controllers.User;

[Route("api/v1/[controller]")]
[ApiController]
public class FriendController : ControllerBase
{
    private readonly IHttpClientHelper _httpClient;

    public FriendController(IHttpClientHelper httpClient)
    {
        _httpClient = httpClient;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(string id)
    {
        if (!HttpContext.Request.Cookies.TryGetValue("refreshToken", out var refreshToken))
        {
            return Unauthorized();
        }

        var responseMessage = await _httpClient.GetAsync($"Friend/{id}", refreshToken, Port.UserApi);
        if (responseMessage.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            return Unauthorized();
        }
        else if (responseMessage.IsSuccessStatusCode)
        {
            var friend = await responseMessage.Content.ReadFromJsonAsync<FriendModel>();

            return Ok(friend);
        }

        return BadRequest();
    }

    [HttpGet("searchByForWhomId/{id}")]
    public async Task<IActionResult> SearchByForWhomId(string id)
    {
        if (!HttpContext.Request.Cookies.TryGetValue("refreshToken", out var refreshToken))
        {
            return Unauthorized();
        }

        var responseMessage = await _httpClient.GetAsync($"Friend/searchByForWhomId/{id}", refreshToken, Port.UserApi);
        if (responseMessage.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            return Unauthorized();
        }
        else if (responseMessage.IsSuccessStatusCode)
        {
            var friends = await responseMessage.Content.ReadFromJsonAsync<IEnumerable<FriendModel>>();

            return Ok(friends);
        }

        return BadRequest();
    }

    [HttpGet("searchByUserId/{id}")]
    public async Task<IActionResult> SearchByUserId(string id)
    {
        if (!HttpContext.Request.Cookies.TryGetValue("refreshToken", out var refreshToken))
        {
            return Unauthorized();
        }

        var responseMessage = await _httpClient.GetAsync($"Friend/searchByUserId/{id}", refreshToken, Port.UserApi);
        if (responseMessage.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            return Unauthorized();
        }
        else if (responseMessage.IsSuccessStatusCode)
        {
            var friends = await responseMessage.Content.ReadFromJsonAsync<IEnumerable<FriendModel>>();

            return Ok(friends);
        }

        return BadRequest();
    }

    [HttpPost]
    public async Task<IActionResult> Create(FriendModel model)
    {
        if (!HttpContext.Request.Cookies.TryGetValue("refreshToken", out var refreshToken))
        {
            return Unauthorized();
        }

        var responseMessage = await _httpClient.PostAsync("Friend", JsonContent.Create(model), refreshToken, Port.UserApi);
        if (responseMessage.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            return Unauthorized();
        }
        else if (responseMessage.IsSuccessStatusCode)
        {
            var friend = await responseMessage.Content.ReadFromJsonAsync<FriendModel>();

            return Ok(friend);
        }

        return BadRequest();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        if (!HttpContext.Request.Cookies.TryGetValue("refreshToken", out var refreshToken))
        {
            return Unauthorized();
        }

        var responseMessage = await _httpClient.DeletAsync($"Friend/{id}", refreshToken, Port.UserApi);
        if (responseMessage.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            return Unauthorized();
        }
        else if (responseMessage.IsSuccessStatusCode)
        {
            return Ok();
        }

        return BadRequest();
    }
}
