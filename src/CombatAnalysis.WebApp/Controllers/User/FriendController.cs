using CombatAnalysis.WebApp.Attributes;
using CombatAnalysis.WebApp.Consts;
using CombatAnalysis.WebApp.Enums;
using CombatAnalysis.WebApp.Extensions;
using CombatAnalysis.WebApp.Interfaces;
using CombatAnalysis.WebApp.Models.User;
using Microsoft.AspNetCore.Mvc;

namespace CombatAnalysis.WebApp.Controllers.User;

[RequireAccessToken]
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
        if (!Request.Cookies.TryGetValue(AuthenticationCookie.AccessToken.ToString(), out var accessToken))
        {
            return Unauthorized();
        }

        var responseMessage = await _httpClient.GetAsync($"Friend/{id}", accessToken, Port.UserApi);
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

    [HttpGet("searchMyFriends/{id}")]
    public async Task<IActionResult> SearchMyFriends(string id)
    {
        if (!Request.Cookies.TryGetValue(AuthenticationCookie.AccessToken.ToString(), out var accessToken))
        {
            return Unauthorized();
        }

        var responseMessage = await _httpClient.GetAsync($"Friend/searchByUserId/{id}", accessToken, Port.UserApi);
        if (responseMessage.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            return Unauthorized();
        }
        else if (responseMessage.IsSuccessStatusCode)
        {
            var friendsCurrentUser = await responseMessage.Content.ReadFromJsonAsync<IEnumerable<FriendModel>>();

            return Ok(friendsCurrentUser);
        }

        return BadRequest();
    }

    [HttpPost]
    public async Task<IActionResult> Create(FriendModel model)
    {
        if (!Request.Cookies.TryGetValue(AuthenticationCookie.AccessToken.ToString(), out var accessToken))
        {
            return Unauthorized();
        }

        var responseMessage = await _httpClient.PostAsync("Friend", JsonContent.Create(model), accessToken, Port.UserApi);
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
        if (!Request.Cookies.TryGetValue(AuthenticationCookie.AccessToken.ToString(), out var accessToken))
        {
            return Unauthorized();
        }

        var responseMessage = await _httpClient.DeletAsync($"Friend/{id}", accessToken, Port.UserApi);
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
