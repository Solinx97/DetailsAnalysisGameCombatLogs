using CombatAnalysis.WebApp.Consts;
using CombatAnalysis.WebApp.Interfaces;
using CombatAnalysis.WebApp.Models.User;
using Microsoft.AspNetCore.Mvc;

namespace CombatAnalysis.WebApp.Controllers.User;

[Route("api/v1/[controller]")]
[ApiController]
public class PostCommentController : ControllerBase
{
    private readonly IHttpClientHelper _httpClient;

    public PostCommentController(IHttpClientHelper httpClient)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = Port.UserApi;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var responseMessage = await _httpClient.GetAsync($"PostComment/{id}");
        var postComment = await responseMessage.Content.ReadFromJsonAsync<PostCommentModel>();

        return Ok(postComment);
    }

    [HttpGet("searchByPostId/{id}")]
    public async Task<IActionResult> SearchByPostId(int id)
    {
        var responseMessage = await _httpClient.GetAsync($"PostComment/searchByPostId/{id}");
        var postComments = await responseMessage.Content.ReadFromJsonAsync<IEnumerable<PostCommentModel>>();

        return Ok(postComments);
    }

    [HttpPut]
    public async Task<IActionResult> Update(PostCommentModel model)
    {
        await _httpClient.PutAsync("PostComment", JsonContent.Create(model));

        return Ok();
    }

    [HttpPost]
    public async Task<IActionResult> Create(PostCommentModel model)
    {
        var responseMessage = await _httpClient.PostAsync("PostComment", JsonContent.Create(model));
        var postComment = await responseMessage.Content.ReadFromJsonAsync<PostCommentModel>();

        return Ok(postComment);
    }
}
