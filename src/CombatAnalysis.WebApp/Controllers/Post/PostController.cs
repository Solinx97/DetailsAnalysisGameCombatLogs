using CombatAnalysis.WebApp.Consts;
using CombatAnalysis.WebApp.Extensions;
using CombatAnalysis.WebApp.Interfaces;
using CombatAnalysis.WebApp.Models.Post;
using Microsoft.AspNetCore.Mvc;

namespace CombatAnalysis.WebApp.Controllers.Post;

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
        if (!HttpContext.Request.Cookies.TryGetValue("refreshToken", out var refreshToken))
        {
            return Unauthorized();
        }

        var responseMessage = await _httpClient.GetAsync($"Post/{id}", refreshToken, Port.CommunicationApi);
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
        if (!HttpContext.Request.Cookies.TryGetValue("refreshToken", out var refreshToken))
        {
            return Unauthorized();
        }

        var responseMessage = await _httpClient.GetAsync($"Post/searchByOwnerId/{id}", refreshToken, Port.CommunicationApi);
        if (responseMessage.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            return Unauthorized();
        } else if (responseMessage.IsSuccessStatusCode)
        {
            var posts = await responseMessage.Content.ReadFromJsonAsync<IEnumerable<PostModel>>();

            return Ok(posts);
        }

        return BadRequest();
    }

    [HttpPut]
    public async Task<IActionResult> Update(PostModel model)
    {
        if (!HttpContext.Request.Cookies.TryGetValue("refreshToken", out var refreshToken))
        {
            return Unauthorized();
        }

        var responseMessage = await _httpClient.PutAsync("Post", JsonContent.Create(model), refreshToken, Port.CommunicationApi);
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
        if (!HttpContext.Request.Cookies.TryGetValue("refreshToken", out var refreshToken))
        {
            return Unauthorized();
        }

        var responseMessage = await _httpClient.PostAsync("Post", JsonContent.Create(model), refreshToken, Port.CommunicationApi);
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
        if (!HttpContext.Request.Cookies.TryGetValue("refreshToken", out var refreshToken))
        {
            return Unauthorized();
        }

        var responseMessage = await _httpClient.DeletAsync($"Post/{id}", refreshToken, Port.CommunicationApi);
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
