using CombatAnalysis.WebApp.Attributes;
using CombatAnalysis.WebApp.Consts;
using CombatAnalysis.WebApp.Enums;
using CombatAnalysis.WebApp.Extensions;
using CombatAnalysis.WebApp.Interfaces;
using CombatAnalysis.WebApp.Models.Post;
using Microsoft.AspNetCore.Mvc;

namespace CombatAnalysis.WebApp.Controllers.Post;

[RequireAccessToken]
[Route("api/v1/[controller]")]
[ApiController]
public class UserPostLikeController : ControllerBase
{
    private readonly IHttpClientHelper _httpClient;

    public UserPostLikeController(IHttpClientHelper httpClient)
    {
        _httpClient = httpClient;
    }

    [HttpGet("{id:int:min(1)}")]
    public async Task<IActionResult> GetById(int id)
    {
        var accessToken = HttpContext.Items[AuthenticationCookie.AccessToken.ToString()] as string;

        var responseMessage = await _httpClient.GetAsync($"UserPostLike/{id}", accessToken, Port.CommunicationApi);
        if (responseMessage.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            return Unauthorized();
        }
        else if (responseMessage.IsSuccessStatusCode)
        {
            var postLike = await responseMessage.Content.ReadFromJsonAsync<UserPostLikeModel>();

            return Ok(postLike);
        }

        return BadRequest();
    }

    [HttpGet("searchByPostId/{id:int:min(1)}")]
    public async Task<IActionResult> SearchByPostId(int id)
    {
        var accessToken = HttpContext.Items[AuthenticationCookie.AccessToken.ToString()] as string;

        var responseMessage = await _httpClient.GetAsync($"UserPostLike/searchByPostId/{id}", accessToken, Port.CommunicationApi);
        if (responseMessage.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            return Unauthorized();
        }
        else if (responseMessage.IsSuccessStatusCode)
        {
            var postLikes = await responseMessage.Content.ReadFromJsonAsync<IEnumerable<UserPostLikeModel>>();

            return Ok(postLikes);
        }

        return BadRequest();
    }

    [HttpPut]
    public async Task<IActionResult> Update(UserPostLikeModel model)
    {
        var accessToken = HttpContext.Items[AuthenticationCookie.AccessToken.ToString()] as string;

        var responseMessage = await _httpClient.PutAsync("UserPostLike", JsonContent.Create(model), accessToken, Port.CommunicationApi);
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

    [HttpPost]
    public async Task<IActionResult> Create(UserPostLikeModel model)
    {
        var accessToken = HttpContext.Items[AuthenticationCookie.AccessToken.ToString()] as string;

        var responseMessage = await _httpClient.PostAsync("UserPostLike", JsonContent.Create(model), accessToken, Port.CommunicationApi);
        if (responseMessage.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            return Unauthorized();
        }
        else if (responseMessage.IsSuccessStatusCode)
        {
            var postLike = await responseMessage.Content.ReadFromJsonAsync<UserPostLikeModel>();

            return Ok(postLike);
        }

        return BadRequest();
    }

    [HttpDelete("{id:int:min(1)}")]
    public async Task<IActionResult> Delete(int id)
    {
        var accessToken = HttpContext.Items[AuthenticationCookie.AccessToken.ToString()] as string;

        var responseMessage = await _httpClient.DeletAsync($"UserPostLike/{id}", accessToken, Port.CommunicationApi);
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
