using CombatAnalysis.WebApp.Attributes;
using CombatAnalysis.WebApp.Consts;
using CombatAnalysis.WebApp.Interfaces;
using CombatAnalysis.WebApp.Models.Community;
using Microsoft.AspNetCore.Mvc;

namespace CombatAnalysis.WebApp.Controllers.Community;

[ServiceFilter(typeof(RequireAccessTokenAttribute))]
[Route("api/v1/[controller]")]
[ApiController]
public class CommunityUserController : ControllerBase
{
    private readonly IHttpClientHelper _httpClient;

    public CommunityUserController(IHttpClientHelper httpClient)
    {
        _httpClient = httpClient;
        _httpClient.APIUrl = API.Communication;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(string id)
    {
        var responseMessage = await _httpClient.GetAsync($"CommunityUser/{id}");
        if (responseMessage.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            return Unauthorized();
        }
        else if (responseMessage.IsSuccessStatusCode)
        {
            var communityUser = await responseMessage.Content.ReadFromJsonAsync<CommunityUserModel>();

            return Ok(communityUser);
        }

        return BadRequest();
    }

    [HttpGet("searchByCommunityId/{communityId:int:min(1)}")]
    public async Task<IActionResult> SearchByCommunityId(int communityId)
    {
        var responseMessage = await _httpClient.GetAsync($"CommunityUser/searchByCommunityId/{communityId}");
        if (responseMessage.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            return Unauthorized();
        }
        else if (responseMessage.IsSuccessStatusCode)
        {
            var communityUsers = await responseMessage.Content.ReadFromJsonAsync<IEnumerable<CommunityUserModel>>();

            return Ok(communityUsers);
        }

        return BadRequest();
    }

    [HttpGet("searchByUserId/{userId}")]
    public async Task<IActionResult> SearchByUserId(string userId)
    {
        var responseMessage = await _httpClient.GetAsync($"CommunityUser/searchByUserId/{userId}");
        if (responseMessage.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            return Unauthorized();
        }
        else if (responseMessage.IsSuccessStatusCode)
        {
            var communityUsers = await responseMessage.Content.ReadFromJsonAsync<IEnumerable<CommunityUserModel>>();

            return Ok(communityUsers);
        }

        return BadRequest();
    }

    [HttpPost]
    public async Task<IActionResult> Create(CommunityUserModel model)
    {
        var responseMessage = await _httpClient.PostAsync("CommunityUser", JsonContent.Create(model));
        if (responseMessage.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            return Unauthorized();
        }
        else if (responseMessage.IsSuccessStatusCode)
        {
            var post = await responseMessage.Content.ReadFromJsonAsync<CommunityUserModel>();

            return Ok(post);
        }

        return BadRequest();
    }

    [HttpPut]
    public async Task<IActionResult> Update(CommunityUserModel model)
    {
        var responseMessage = await _httpClient.PutAsync("CommunityUser", JsonContent.Create(model));
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

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        var responseMessage = await _httpClient.DeletAsync($"CommunityUser/{id}");
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
