using CombatAnalysis.WebApp.Consts;
using CombatAnalysis.WebApp.Extensions;
using CombatAnalysis.WebApp.Interfaces;
using CombatAnalysis.WebApp.Models.Post;
using Microsoft.AspNetCore.Mvc;

namespace CombatAnalysis.WebApp.Controllers.Post;

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
        var responseMessage = await _httpClient.GetAsync($"PostComment/{id}", Port.ChatApi);
        var postComment = await responseMessage.Content.ReadFromJsonAsync<PostCommentModel>();

        return Ok(postComment);
    }

    [HttpGet("searchByPostId/{id:int:min(1)}")]
    public async Task<IActionResult> SearchByPostId(int id)
    {
        var responseMessage = await _httpClient.GetAsync($"PostComment/searchByPostId/{id}", Port.ChatApi);
        var postComments = await responseMessage.Content.ReadFromJsonAsync<IEnumerable<PostCommentModel>>();

        return Ok(postComments);
    }

    [HttpPut]
    public async Task<IActionResult> Update(PostCommentModel model)
    {
        await _httpClient.PutAsync("PostComment", JsonContent.Create(model), Port.ChatApi);

        return Ok();
    }

    [HttpPost]
    public async Task<IActionResult> Create(PostCommentModel model)
    {
        var responseMessage = await _httpClient.PostAsync("PostComment", JsonContent.Create(model), Port.ChatApi);
        var postComment = await responseMessage.Content.ReadFromJsonAsync<PostCommentModel>();

        return Ok(postComment);
    }

    [HttpDelete("{id:int:min(1)}")]
    public async Task<IActionResult> Delete(int id)
    {
        var responseMessage = await _httpClient.DeletAsync($"PostComment/{id}", Port.ChatApi);
        if (responseMessage.StatusCode == System.Net.HttpStatusCode.OK)
        {
            return Ok();
        }

        return BadRequest();
    }
}
