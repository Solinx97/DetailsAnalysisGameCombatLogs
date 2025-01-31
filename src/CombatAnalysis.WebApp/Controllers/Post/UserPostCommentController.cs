using CombatAnalysis.WebApp.Attributes;
using CombatAnalysis.WebApp.Consts;
using CombatAnalysis.WebApp.Interfaces;
using CombatAnalysis.WebApp.Models.Post;
using Microsoft.AspNetCore.Mvc;

namespace CombatAnalysis.WebApp.Controllers.Post;

[ServiceFilter(typeof(RequireAccessTokenAttribute))]
[Route("api/v1/[controller]")]
[ApiController]
public class UserPostCommentController : ControllerBase
{
    private readonly IHttpClientHelper _httpClient;

    public UserPostCommentController(IHttpClientHelper httpClient)
    {
        _httpClient = httpClient;
        _httpClient.APIUrl = Cluster.Communication;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var responseMessage = await _httpClient.GetAsync("UserPostComment");
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
        var responseMessage = await _httpClient.GetAsync($"UserPostComment/{id}");
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
        var responseMessage = await _httpClient.GetAsync($"UserPostComment/searchByPostId/{id}");
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

    [HttpPost]
    public async Task<IActionResult> Create(UserPostCommentModel model)
    {
        var responseMessage = await _httpClient.PostAsync("UserPostComment", JsonContent.Create(model));
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

    [HttpPut]
    public async Task<IActionResult> Update(UserPostCommentModel model)
    {
        var responseMessage = await _httpClient.PutAsync("UserPostComment", JsonContent.Create(model));
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

    [HttpDelete("{id:int:min(1)}")]
    public async Task<IActionResult> Delete(int id)
    {
        var responseMessage = await _httpClient.DeletAsync($"UserPostComment/{id}");
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
