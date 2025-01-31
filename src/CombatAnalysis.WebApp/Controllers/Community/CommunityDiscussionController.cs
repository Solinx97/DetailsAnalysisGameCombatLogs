using CombatAnalysis.WebApp.Attributes;
using CombatAnalysis.WebApp.Consts;
using CombatAnalysis.WebApp.Interfaces;
using CombatAnalysis.WebApp.Models.Community;
using Microsoft.AspNetCore.Mvc;

namespace CombatAnalysis.WebApp.Controllers.Community;

[ServiceFilter(typeof(RequireAccessTokenAttribute))]
[Route("api/v1/[controller]")]
[ApiController]
public class CommunityDiscussionController : ControllerBase
{
    private readonly IHttpClientHelper _httpClient;

    public CommunityDiscussionController(IHttpClientHelper httpClient)
    {
        _httpClient = httpClient;
        _httpClient.APIUrl = Cluster.Communication;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var responseMessage = await _httpClient.GetAsync("CommunityDiscussion");
        if (responseMessage.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            return Unauthorized();
        }
        else if (responseMessage.IsSuccessStatusCode)
        {
            var communities = await responseMessage.Content.ReadFromJsonAsync<IEnumerable<CommunityDiscussionModel>>();

            return Ok(communities);
        }

        return BadRequest();
    }

    [HttpGet("{id:int:min(1)}")]
    public async Task<IActionResult> GetById(int id)
    {
        var responseMessage = await _httpClient.GetAsync($"CommunityDiscussion/{id}");
        if (responseMessage.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            return Unauthorized();
        }
        else if (responseMessage.IsSuccessStatusCode)
        {
            var community = await responseMessage.Content.ReadFromJsonAsync<CommunityDiscussionModel>();

            return Ok(community);
        }

        return BadRequest();
    }

    [HttpGet("findByCommunityId/{id:int:min(1)}")]
    public async Task<IActionResult> FindByCommunityId(int id)
    {
        var responseMessage = await _httpClient.GetAsync($"CommunityDiscussion/findByCommunityId/{id}");
        if (responseMessage.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            return Unauthorized();
        }
        else if (responseMessage.IsSuccessStatusCode)
        {
            var communityDiscussions = await responseMessage.Content.ReadFromJsonAsync<IEnumerable<CommunityDiscussionModel>>();

            return Ok(communityDiscussions);
        }

        return BadRequest();
    }

    [HttpPost]
    public async Task<IActionResult> Create(CommunityDiscussionModel newCommunity)
    {
        var responseMessage = await _httpClient.PostAsync("CommunityDiscussion", JsonContent.Create(newCommunity));
        if (responseMessage.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            return Unauthorized();
        }
        else if (responseMessage.IsSuccessStatusCode)
        {
            var community = await responseMessage.Content.ReadFromJsonAsync<CommunityDiscussionModel>();

            return Ok(community);
        }

        return BadRequest();
    }

    [HttpPut]
    public async Task<IActionResult> Update(CommunityDiscussionModel chat)
    {
        var responseMessage = await _httpClient.PutAsync("CommunityDiscussion", JsonContent.Create(chat));
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
        var responseMessage = await _httpClient.DeletAsync($"CommunityDiscussion/{id}");
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
