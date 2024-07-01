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
public class PostController : ControllerBase
{
    private readonly IHttpClientHelper _httpClient;

    public PostController(IHttpClientHelper httpClient)
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

        var responseMessage = await _httpClient.GetAsync($"Post/{id}", accessToken, Port.CommunicationApi);
        if (responseMessage.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            return Unauthorized();
        }
        else if (responseMessage.IsSuccessStatusCode)
        {
            var post = await responseMessage.Content.ReadFromJsonAsync<PostModel>();

            return Ok(post);
        }

        return BadRequest();
    }

    [HttpGet("searchByOwnerId/{id}")]
    public async Task<IActionResult> SearchByOwnerId(string id)
    {
        if (!Request.Cookies.TryGetValue(AuthenticationTokenType.AccessToken.ToString(), out var accessToken))
        {
            return Unauthorized();
        }

        var responseMessage = await _httpClient.GetAsync($"Post/searchByOwnerId/{id}", accessToken, Port.CommunicationApi);
        if (responseMessage.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            return Unauthorized();
        } 
        else if (responseMessage.IsSuccessStatusCode)
        {
            var posts = await responseMessage.Content.ReadFromJsonAsync<IEnumerable<PostModel>>();

            return Ok(posts);
        }

        return BadRequest();
    }

    [HttpPut]
    public async Task<IActionResult> Update(PostModel model)
    {
        if (!Request.Cookies.TryGetValue(AuthenticationTokenType.AccessToken.ToString(), out var accessToken))
        {
            return Unauthorized();
        }

        var responseMessage = await _httpClient.PutAsync("Post", JsonContent.Create(model), accessToken, Port.CommunicationApi);
        if (responseMessage.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            return Unauthorized();
        } else if (responseMessage.IsSuccessStatusCode)
        {
            return Ok();
        }

        return BadRequest();
    }

    [HttpPost]
    public async Task<IActionResult> Create(PostModel model)
    {
        if (!Request.Cookies.TryGetValue(AuthenticationTokenType.AccessToken.ToString(), out var accessToken))
        {
            return Unauthorized();
        }

        var responseMessage = await _httpClient.PostAsync("Post", JsonContent.Create(model), accessToken, Port.CommunicationApi);
        if (responseMessage.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            return Unauthorized();
        } else if (responseMessage.IsSuccessStatusCode)
        {
            var post = await responseMessage.Content.ReadFromJsonAsync<PostModel>();

            return Ok(post);
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

        var responseMessage = await _httpClient.DeletAsync($"Post/{id}", accessToken, Port.CommunicationApi);
        if (responseMessage.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            return Unauthorized();
        } else if (responseMessage.IsSuccessStatusCode)
        {
            return Ok();
        }

        return BadRequest();
    }
}
