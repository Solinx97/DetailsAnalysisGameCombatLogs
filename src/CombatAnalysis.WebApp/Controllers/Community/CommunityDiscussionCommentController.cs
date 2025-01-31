using CombatAnalysis.WebApp.Attributes;
using CombatAnalysis.WebApp.Consts;
using CombatAnalysis.WebApp.Interfaces;
using CombatAnalysis.WebApp.Models.Community;
using Microsoft.AspNetCore.Mvc;

namespace CombatAnalysis.WebApp.Controllers.Community;

[ServiceFilter(typeof(RequireAccessTokenAttribute))]
[Route("api/v1/[controller]")]
[ApiController]
public class CommunityDiscussionCommentController : ControllerBase
{
    private readonly IHttpClientHelper _httpClient;

    public CommunityDiscussionCommentController(IHttpClientHelper httpClient)
    {
        _httpClient = httpClient;
        _httpClient.APIUrl = Cluster.Communication;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var responseMessage = await _httpClient.GetAsync("CommunityDiscussionComment");
        if (responseMessage.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            return Unauthorized();
        }
        else if (responseMessage.IsSuccessStatusCode)
        {
            var communities = await responseMessage.Content.ReadFromJsonAsync<IEnumerable<CommunityDiscussionCommentModel>>();

            return Ok(communities);
        }

        return BadRequest();
    }

    [HttpGet("{id:int:min(1)}")]
    public async Task<IActionResult> GetById(int id)
    {
        var responseMessage = await _httpClient.GetAsync($"CommunityDiscussionComment/{id}");
        if (responseMessage.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            return Unauthorized();
        }
        else if (responseMessage.IsSuccessStatusCode)
        {
            var community = await responseMessage.Content.ReadFromJsonAsync<CommunityDiscussionCommentModel>();

            return Ok(community);
        }

        return BadRequest();
    }

    [HttpGet("findByDiscussionId/{id:int:min(1)}")]
    public async Task<IActionResult> FindByDiscussionId(int id)
    {
        var responseMessage = await _httpClient.GetAsync($"CommunityDiscussionComment/findByDiscussionId/{id}");
        if (responseMessage.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            return Unauthorized();
        }
        else if (responseMessage.IsSuccessStatusCode)
        {
            var communityDiscussionComments = await responseMessage.Content.ReadFromJsonAsync<IEnumerable<CommunityDiscussionCommentModel>>();

            return Ok(communityDiscussionComments);
        }

        return BadRequest();
    }

    [HttpPost]
    public async Task<IActionResult> Create(CommunityDiscussionCommentModel newCommunity)
    {
        var responseMessage = await _httpClient.PostAsync("CommunityDiscussionComment", JsonContent.Create(newCommunity));
        if (responseMessage.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            return Unauthorized();
        }
        else if (responseMessage.IsSuccessStatusCode)
        {
            var community = await responseMessage.Content.ReadFromJsonAsync<CommunityDiscussionCommentModel>();

            return Ok(community);
        }

        return BadRequest();
    }

    [HttpPut]
    public async Task<IActionResult> Update(CommunityDiscussionCommentModel chat)
    {
        var responseMessage = await _httpClient.PutAsync("CommunityDiscussionComment", JsonContent.Create(chat));
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
        var responseMessage = await _httpClient.DeletAsync($"CommunityDiscussionComment/{id}");
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
