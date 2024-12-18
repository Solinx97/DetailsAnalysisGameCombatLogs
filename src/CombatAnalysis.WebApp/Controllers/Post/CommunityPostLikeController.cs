using CombatAnalysis.WebApp.Attributes;
using CombatAnalysis.WebApp.Consts;
using CombatAnalysis.WebApp.Interfaces;
using CombatAnalysis.WebApp.Models.Post;
using Microsoft.AspNetCore.Mvc;

namespace CombatAnalysis.WebApp.Controllers.Post;

[ServiceFilter(typeof(RequireAccessTokenAttribute))]
[Route("api/v1/[controller]")]
[ApiController]
public class CommunityPostLikeController : ControllerBase
{
    private readonly IHttpClientHelper _httpClient;

    public CommunityPostLikeController(IHttpClientHelper httpClient)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = Port.CommunicationApi;
    }

    [HttpGet("{id:int:min(1)}")]
    public async Task<IActionResult> GetById(int id)
    {
        var responseMessage = await _httpClient.GetAsync($"CommunityPostLike/{id}");
        if (responseMessage.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            return Unauthorized();
        }
        else if (responseMessage.IsSuccessStatusCode)
        {
            var postLike = await responseMessage.Content.ReadFromJsonAsync<CommunityPostLikeModel>();

            return Ok(postLike);
        }

        return BadRequest();
    }

    [HttpGet("searchByPostId/{id:int:min(1)}")]
    public async Task<IActionResult> SearchByPostId(int id)
    {
        var responseMessage = await _httpClient.GetAsync($"CommunityPostLike/searchByPostId/{id}");
        if (responseMessage.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            return Unauthorized();
        }
        else if (responseMessage.IsSuccessStatusCode)
        {
            var postLikes = await responseMessage.Content.ReadFromJsonAsync<IEnumerable<CommunityPostLikeModel>>();

            return Ok(postLikes);
        }

        return BadRequest();
    }

    [HttpPost]
    public async Task<IActionResult> Create(CommunityPostLikeModel model)
    {
        var responseMessage = await _httpClient.PostAsync("CommunityPostLike", JsonContent.Create(model));
        if (responseMessage.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            return Unauthorized();
        }
        else if (responseMessage.IsSuccessStatusCode)
        {
            var postLike = await responseMessage.Content.ReadFromJsonAsync<CommunityPostLikeModel>();

            return Ok(postLike);
        }

        return BadRequest();
    }

    [HttpPut]
    public async Task<IActionResult> Update(CommunityPostLikeModel model)
    {
        var responseMessage = await _httpClient.PutAsync("CommunityPostLike", JsonContent.Create(model));
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
        var responseMessage = await _httpClient.DeletAsync($"CommunityPostLike/{id}");
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
