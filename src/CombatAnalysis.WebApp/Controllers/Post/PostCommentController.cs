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
public class PostCommentController : ControllerBase
{
    private readonly IHttpClientHelper _httpClient;

    public PostCommentController(IHttpClientHelper httpClient)
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

        var responseMessage = await _httpClient.GetAsync($"PostComment/{id}", accessToken, Port.CommunicationApi);
        if (responseMessage.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            return Unauthorized();
        }
        else if (responseMessage.IsSuccessStatusCode)
        {
            var postComment = await responseMessage.Content.ReadFromJsonAsync<PostCommentModel>();

            return Ok(postComment);
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

        var responseMessage = await _httpClient.GetAsync($"PostComment/searchByPostId/{id}", accessToken, Port.CommunicationApi);
        if (responseMessage.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            return Unauthorized();
        }
        else if (responseMessage.IsSuccessStatusCode)
        {
            var postComments = await responseMessage.Content.ReadFromJsonAsync<IEnumerable<PostCommentModel>>();

            return Ok(postComments);
        }

        return BadRequest();
    }

    [HttpPut]
    public async Task<IActionResult> Update(PostCommentModel model)
    {
        if (!Request.Cookies.TryGetValue(AuthenticationTokenType.AccessToken.ToString(), out var accessToken))
        {
            return Unauthorized();
        }

        var responseMessage = await _httpClient.PutAsync("PostComment", JsonContent.Create(model), accessToken, Port.CommunicationApi);
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
    public async Task<IActionResult> Create(PostCommentModel model)
    {
        if (!Request.Cookies.TryGetValue(AuthenticationTokenType.AccessToken.ToString(), out var accessToken))
        {
            return Unauthorized();
        }

        var responseMessage = await _httpClient.PostAsync("PostComment", JsonContent.Create(model), accessToken, Port.CommunicationApi);
        if (responseMessage.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            return Unauthorized();
        }
        else if (responseMessage.IsSuccessStatusCode)
        {
            var postComment = await responseMessage.Content.ReadFromJsonAsync<PostCommentModel>();

            return Ok(postComment);
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

        var responseMessage = await _httpClient.DeletAsync($"PostComment/{id}", accessToken, Port.CommunicationApi);
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
