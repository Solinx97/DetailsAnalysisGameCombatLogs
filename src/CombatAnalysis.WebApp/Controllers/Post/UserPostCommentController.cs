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
public class UserPostCommentController : ControllerBase
{
    private readonly IHttpClientHelper _httpClient;

    public UserPostCommentController(IHttpClientHelper httpClient)
    {
        _httpClient = httpClient;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var accessToken = HttpContext.Items[AuthenticationCookie.AccessToken.ToString()] as string;

        var responseMessage = await _httpClient.GetAsync("UserPostComment", accessToken, Port.CommunicationApi);
        if (responseMessage.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            return Unauthorized();
        }
        else if (responseMessage.IsSuccessStatusCode)
        {
            var postComment = await responseMessage.Content.ReadFromJsonAsync<UserPostCommentModel>();

            return Ok(postComment);
        }

        return BadRequest();
    }

    [HttpGet("{id:int:min(1)}")]
    public async Task<IActionResult> GetById(int id)
    {
        var accessToken = HttpContext.Items[AuthenticationCookie.AccessToken.ToString()] as string;

        var responseMessage = await _httpClient.GetAsync($"UserPostComment/{id}", accessToken, Port.CommunicationApi);
        if (responseMessage.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            return Unauthorized();
        }
        else if (responseMessage.IsSuccessStatusCode)
        {
            var postComment = await responseMessage.Content.ReadFromJsonAsync<UserPostCommentModel>();

            return Ok(postComment);
        }

        return BadRequest();
    }

    [HttpGet("searchByPostId/{id:int:min(1)}")]
    public async Task<IActionResult> SearchByPostId(int id)
    {
        var accessToken = HttpContext.Items[AuthenticationCookie.AccessToken.ToString()] as string;

        var responseMessage = await _httpClient.GetAsync($"UserPostComment/searchByPostId/{id}", accessToken, Port.CommunicationApi);
        if (responseMessage.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            return Unauthorized();
        }
        else if (responseMessage.IsSuccessStatusCode)
        {
            var postComments = await responseMessage.Content.ReadFromJsonAsync<IEnumerable<UserPostCommentModel>>();

            return Ok(postComments);
        }

        return BadRequest();
    }

    [HttpPut]
    public async Task<IActionResult> Update(UserPostCommentModel model)
    {
        var accessToken = HttpContext.Items[AuthenticationCookie.AccessToken.ToString()] as string;

        var responseMessage = await _httpClient.PutAsync("UserPostComment", JsonContent.Create(model), accessToken, Port.CommunicationApi);
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
    public async Task<IActionResult> Create(UserPostCommentModel model)
    {
        var accessToken = HttpContext.Items[AuthenticationCookie.AccessToken.ToString()] as string;

        var responseMessage = await _httpClient.PostAsync("UserPostComment", JsonContent.Create(model), accessToken, Port.CommunicationApi);
        if (responseMessage.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            return Unauthorized();
        }
        else if (responseMessage.IsSuccessStatusCode)
        {
            var postComment = await responseMessage.Content.ReadFromJsonAsync<UserPostCommentModel>();

            return Ok(postComment);
        }

        return BadRequest();
    }

    [HttpDelete("{id:int:min(1)}")]
    public async Task<IActionResult> Delete(int id)
    {
        var accessToken = HttpContext.Items[AuthenticationCookie.AccessToken.ToString()] as string;

        var responseMessage = await _httpClient.DeletAsync($"UserPostComment/{id}", accessToken, Port.CommunicationApi);
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
