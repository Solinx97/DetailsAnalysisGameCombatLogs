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
public class PostDislikeController : ControllerBase
{
    private readonly IHttpClientHelper _httpClient;

    public PostDislikeController(IHttpClientHelper httpClient)
    {
        _httpClient = httpClient;
    }

    [HttpGet("{id:int:min(1)}")]
    public async Task<IActionResult> GetById(int id)
    {
        if (!Request.Cookies.TryGetValue(AuthenticationTokenType.AccessToken.ToString(), out var accessToken))
        {
            return Unauthorized();
        }

        var responseMessage = await _httpClient.GetAsync($"PostDislike/{id}", accessToken, Port.CommunicationApi);
        if (responseMessage.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            return Unauthorized();
        }
        else if (responseMessage.IsSuccessStatusCode)
        {
            var postDislike = await responseMessage.Content.ReadFromJsonAsync<PostDislikeModel>();

            return Ok(postDislike);
        }

        return BadRequest();
    }

    [HttpGet("searchByPostId/{id:int:min(1)}")]
    public async Task<IActionResult> SearchByPostId(int id)
    {
        if (!Request.Cookies.TryGetValue(AuthenticationTokenType.AccessToken.ToString(), out var accessToken))
        {
            return Unauthorized();
        }

        var responseMessage = await _httpClient.GetAsync($"PostDislike/searchByPostId/{id}", accessToken, Port.CommunicationApi);
        if (responseMessage.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            return Unauthorized();
        }
        else if (responseMessage.IsSuccessStatusCode)
        {
            var postDislikes = await responseMessage.Content.ReadFromJsonAsync<IEnumerable<PostDislikeModel>>();

            return Ok(postDislikes);
        }

        return BadRequest();
    }

    [HttpPut]
    public async Task<IActionResult> Update(PostLikeModel model)
    {
        if (!Request.Cookies.TryGetValue(AuthenticationTokenType.AccessToken.ToString(), out var accessToken))
        {
            return Unauthorized();
        }

        var responseMessage = await _httpClient.PutAsync("PostDislike", JsonContent.Create(model), accessToken, Port.CommunicationApi);
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
    public async Task<IActionResult> Create(PostLikeModel model)
    {
        if (!Request.Cookies.TryGetValue(AuthenticationTokenType.AccessToken.ToString(), out var accessToken))
        {
            return Unauthorized();
        }

        var responseMessage = await _httpClient.PostAsync("PostDislike", JsonContent.Create(model), accessToken, Port.CommunicationApi);
        if (responseMessage.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            return Unauthorized();
        }
        else if (responseMessage.IsSuccessStatusCode)
        {
            var postDislike = await responseMessage.Content.ReadFromJsonAsync<PostDislikeModel>();

            return Ok(postDislike);
        }

        return BadRequest();
    }

    [HttpDelete("{id:int:min(1)}")]
    public async Task<IActionResult> Delete(int id)
    {
        if (!Request.Cookies.TryGetValue(AuthenticationTokenType.AccessToken.ToString(), out var accessToken))
        {
            return Unauthorized();
        }

        var responseMessage = await _httpClient.DeletAsync($"PostDislike/{id}", accessToken, Port.CommunicationApi);
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
