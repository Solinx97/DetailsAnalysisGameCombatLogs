using CombatAnalysis.WebApp.Attributes;
using CombatAnalysis.WebApp.Consts;
using CombatAnalysis.WebApp.Interfaces;
using CombatAnalysis.WebApp.Models.Post;
using Microsoft.AspNetCore.Mvc;

namespace CombatAnalysis.WebApp.Controllers.Post;

[ServiceFilter(typeof(RequireAccessTokenAttribute))]
[Route("api/v1/[controller]")]
[ApiController]
public class UserPostLikeController : ControllerBase
{
    private readonly IHttpClientHelper _httpClient;

    public UserPostLikeController(IHttpClientHelper httpClient)
    {
        _httpClient = httpClient;
        _httpClient.APIUrl = Cluster.Communication;
    }

    [HttpGet("{id:int:min(1)}")]
    public async Task<IActionResult> GetById(int id)
    {
        var responseMessage = await _httpClient.GetAsync($"UserPostLike/{id}");
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
        var responseMessage = await _httpClient.GetAsync($"UserPostLike/searchByPostId/{id}");
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

    [HttpPost]
    public async Task<IActionResult> Create(UserPostLikeModel model)
    {
        var responseMessage = await _httpClient.PostAsync("UserPostLike", JsonContent.Create(model));
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

    [HttpPut]
    public async Task<IActionResult> Update(UserPostLikeModel model)
    {
        var responseMessage = await _httpClient.PutAsync("UserPostLike", JsonContent.Create(model));
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
        var responseMessage = await _httpClient.DeletAsync($"UserPostLike/{id}");
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
