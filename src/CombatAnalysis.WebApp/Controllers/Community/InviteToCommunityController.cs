using CombatAnalysis.WebApp.Attributes;
using CombatAnalysis.WebApp.Consts;
using CombatAnalysis.WebApp.Interfaces;
using CombatAnalysis.WebApp.Models.Community;
using Microsoft.AspNetCore.Mvc;

namespace CombatAnalysis.WebApp.Controllers.Community;

[ServiceFilter(typeof(RequireAccessTokenAttribute))]
[Route("api/v1/[controller]")]
[ApiController]
public class InviteToCommunityController : ControllerBase
{
    private readonly IHttpClientHelper _httpClient;

    public InviteToCommunityController(IHttpClientHelper httpClient)
    {
        _httpClient = httpClient;
        _httpClient.APIUrl = Cluster.Communication;
    }

    [HttpPost]
    public async Task<IActionResult> Create(InviteToCommunityModel invite)
    {
        var responseMessage = await _httpClient.PostAsync("InviteToCommunity", JsonContent.Create(invite));
        if (responseMessage.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            return Unauthorized();
        }
        else if (responseMessage.IsSuccessStatusCode)
        {
            var inviteToCommunity = await responseMessage.Content.ReadFromJsonAsync<InviteToCommunityModel>();

            return Ok(inviteToCommunity);
        }

        return BadRequest();
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var responseMessage = await _httpClient.GetAsync("InviteToCommunity");
        if (responseMessage.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            return Unauthorized();
        }
        else if (responseMessage.IsSuccessStatusCode)
        {
            var invitesToCommunity = await responseMessage.Content.ReadFromJsonAsync<IEnumerable<InviteToCommunityModel>>();

            return Ok(invitesToCommunity);
        }

        return BadRequest();
    }

    [HttpGet("{id:int:min(1)}")]
    public async Task<IActionResult> GetById(int id)
    {
        var responseMessage = await _httpClient.GetAsync($"InviteToCommunity/{id}");
        if (responseMessage.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            return Unauthorized();
        }
        else if (responseMessage.IsSuccessStatusCode)
        {
            var inviteToCommunity = await responseMessage.Content.ReadFromJsonAsync<InviteToCommunityModel>();

            return Ok(inviteToCommunity);
        }

        return BadRequest();
    }

    [HttpGet("searchByUserId/{id}")]
    public async Task<IActionResult> SearchByUserId(string id)
    {
        var responseMessage = await _httpClient.GetAsync($"InviteToCommunity/searchByUserId/{id}");
        if (responseMessage.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            return Unauthorized();
        }
        else if (responseMessage.IsSuccessStatusCode)
        {
            var invitesToCommunity = await responseMessage.Content.ReadFromJsonAsync<IEnumerable<InviteToCommunityModel>>();

            return Ok(invitesToCommunity);
        }

        return BadRequest();
    }

    [HttpGet("isExist")]
    public async Task<IActionResult> IsExist(string peopleId, int communityId)
    {
        var responseMessage = await _httpClient.GetAsync("InviteToCommunity");
        if (responseMessage.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            return Unauthorized();
        }
        else if (!responseMessage.IsSuccessStatusCode)
        {
            return BadRequest();
        }

        var allInvites = await responseMessage.Content.ReadFromJsonAsync<IEnumerable<InviteToCommunityModel>>();
        var existedInvites = allInvites.Where(x => x.ToAppUserId == peopleId && x.CommunityId == communityId).ToList();
        if (existedInvites.Any())
        {
            return Ok(true);
        }

        return Ok(false);
    }

    [HttpPut]
    public async Task<IActionResult> Update(InviteToCommunityModel invite)
    {
        var responseMessage = await _httpClient.PutAsync("InviteToCommunity", JsonContent.Create(invite));
        if (responseMessage.StatusCode == System.Net.HttpStatusCode.OK)
        {
            return Ok();
        }

        return BadRequest();
    }

    [HttpDelete("{id:int:min(1)}")]
    public async Task<IActionResult> Delete(int id)
    {
        var responseMessage = await _httpClient.DeletAsync($"InviteToCommunity/{id}");
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
