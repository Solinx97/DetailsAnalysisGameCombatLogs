using CombatAnalysis.WebApp.Attributes;
using CombatAnalysis.WebApp.Consts;
using CombatAnalysis.WebApp.Interfaces;
using CombatAnalysis.WebApp.Models.Post;
using Microsoft.AspNetCore.Mvc;

namespace CombatAnalysis.WebApp.Controllers.Post;

[ServiceFilter(typeof(RequireAccessTokenAttribute))]
[Route("api/v1/[controller]")]
[ApiController]
public class CommunityPostDislikeController : ControllerBase
{
    private readonly IHttpClientHelper _httpClient;

    public CommunityPostDislikeController(IHttpClientHelper httpClient)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = Port.CommunicationApi;
    }

    [HttpGet("{id:int:min(1)}")]
    public async Task<IActionResult> GetById(int id)
    {
        var responseMessage = await _httpClient.GetAsync($"CommunityPostDislike/{id}");
        if (responseMessage.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            return Unauthorized();
        }
        else if (responseMessage.IsSuccessStatusCode)
        {
            var postDislike = await responseMessage.Content.ReadFromJsonAsync<CommunityPostDislikeModel>();

            return Ok(postDislike);
        }

        return BadRequest();
    }

    [HttpGet("searchByPostId/{id:int:min(1)}")]
    public async Task<IActionResult> SearchByPostId(int id)
    {
        var responseMessage = await _httpClient.GetAsync($"CommunityPostDislike/searchByPostId/{id}");
        if (responseMessage.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            return Unauthorized();
        }
        else if (responseMessage.IsSuccessStatusCode)
        {
            var postDislikes = await responseMessage.Content.ReadFromJsonAsync<IEnumerable<CommunityPostDislikeModel>>();

            return Ok(postDislikes);
        }

        return BadRequest();
    }

    [HttpPost]
    public async Task<IActionResult> Create(CommunityPostDislikeModel model)
    {
        var responseMessage = await _httpClient.PostAsync("CommunityPostDislike", JsonContent.Create(model));
        if (responseMessage.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            return Unauthorized();
        }
        else if (responseMessage.IsSuccessStatusCode)
        {
            var postDislike = await responseMessage.Content.ReadFromJsonAsync<CommunityPostDislikeModel>();

            return Ok(postDislike);
        }

        return BadRequest();
    }

    [HttpPut]
    public async Task<IActionResult> Update(CommunityPostDislikeModel model)
    {
        var responseMessage = await _httpClient.PutAsync("CommunityPostDislike", JsonContent.Create(model));
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
        var responseMessage = await _httpClient.DeletAsync($"CommunityPostDislike/{id}");
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
