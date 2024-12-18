using CombatAnalysis.WebApp.Attributes;
using CombatAnalysis.WebApp.Consts;
using CombatAnalysis.WebApp.Interfaces;
using CombatAnalysis.WebApp.Models.Post;
using Microsoft.AspNetCore.Mvc;

namespace CombatAnalysis.WebApp.Controllers.Post;

[ServiceFilter(typeof(RequireAccessTokenAttribute))]
[Route("api/v1/[controller]")]
[ApiController]
public class UserPostDislikeController : ControllerBase
{
    private readonly IHttpClientHelper _httpClient;

    public UserPostDislikeController(IHttpClientHelper httpClient)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = Port.CommunicationApi;
    }

    [HttpGet("{id:int:min(1)}")]
    public async Task<IActionResult> GetById(int id)
    {
        var responseMessage = await _httpClient.GetAsync($"UserPostDislike/{id}");
        if (responseMessage.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            return Unauthorized();
        }
        else if (responseMessage.IsSuccessStatusCode)
        {
            var postDislike = await responseMessage.Content.ReadFromJsonAsync<UserPostDislikeModel>();

            return Ok(postDislike);
        }

        return BadRequest();
    }

    [HttpGet("searchByPostId/{id:int:min(1)}")]
    public async Task<IActionResult> SearchByPostId(int id)
    {
        var responseMessage = await _httpClient.GetAsync($"UserPostDislike/searchByPostId/{id}");
        if (responseMessage.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            return Unauthorized();
        }
        else if (responseMessage.IsSuccessStatusCode)
        {
            var postDislikes = await responseMessage.Content.ReadFromJsonAsync<IEnumerable<UserPostDislikeModel>>();

            return Ok(postDislikes);
        }

        return BadRequest();
    }

    [HttpPost]
    public async Task<IActionResult> Create(UserPostLikeModel model)
    {
        var responseMessage = await _httpClient.PostAsync("UserPostDislike", JsonContent.Create(model));
        if (responseMessage.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            return Unauthorized();
        }
        else if (responseMessage.IsSuccessStatusCode)
        {
            var postDislike = await responseMessage.Content.ReadFromJsonAsync<UserPostDislikeModel>();

            return Ok(postDislike);
        }

        return BadRequest();
    }

    [HttpPut]
    public async Task<IActionResult> Update(UserPostLikeModel model)
    {
        var responseMessage = await _httpClient.PutAsync("UserPostDislike", JsonContent.Create(model));
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
        var responseMessage = await _httpClient.DeletAsync($"UserPostDislike/{id}");
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
