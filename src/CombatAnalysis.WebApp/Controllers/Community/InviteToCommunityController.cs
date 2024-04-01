using CombatAnalysis.WebApp.Consts;
using CombatAnalysis.WebApp.Extensions;
using CombatAnalysis.WebApp.Interfaces;
using CombatAnalysis.WebApp.Models.Community;
using Microsoft.AspNetCore.Mvc;

namespace CombatAnalysis.WebApp.Controllers.Community;

[Route("api/v1/[controller]")]
[ApiController]
public class InviteToCommunityController : ControllerBase
{
    private readonly IHttpClientHelper _httpClient;

    public InviteToCommunityController(IHttpClientHelper httpClient)
    {
        _httpClient = httpClient;
    }

    [HttpPost]
    public async Task<IActionResult> Create(InviteToCommunityModel chat)
    {
        if (!HttpContext.Request.Cookies.TryGetValue("refreshToken", out var refreshToken))
        {
            return Unauthorized();
        }

        var responseMessage = await _httpClient.PostAsync("InviteToCommunity", JsonContent.Create(chat), refreshToken, Port.CommunicationApi);
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
        if (!HttpContext.Request.Cookies.TryGetValue("refreshToken", out var refreshToken))
        {
            return Unauthorized();
        }

        var responseMessage = await _httpClient.GetAsync("InviteToCommunity", refreshToken, Port.CommunicationApi);
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
        if (!HttpContext.Request.Cookies.TryGetValue("refreshToken", out var refreshToken))
        {
            return Unauthorized();
        }

        var responseMessage = await _httpClient.GetAsync($"InviteToCommunity/{id}", refreshToken, Port.CommunicationApi);
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
        if (!HttpContext.Request.Cookies.TryGetValue("refreshToken", out var refreshToken))
        {
            return Unauthorized();
        }

        var responseMessage = await _httpClient.GetAsync($"InviteToCommunity/searchByUserId/{id}", refreshToken, Port.CommunicationApi);
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
        if (!HttpContext.Request.Cookies.TryGetValue("refreshToken", out var refreshToken))
        {
            return Unauthorized();
        }

        var responseMessage = await _httpClient.GetAsync("InviteToCommunity", refreshToken, Port.CommunicationApi);
        if (responseMessage.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            return Unauthorized();
        }
        else if (!responseMessage.IsSuccessStatusCode)
        {
            return BadRequest();
        }

        var allInvites = await responseMessage.Content.ReadFromJsonAsync<IEnumerable<InviteToCommunityModel>>();
        var existedInvites = allInvites.Where(x => x.ToCustomerId == peopleId && x.CommunityId == communityId).ToList();
        if (existedInvites.Any())
        {
            return Ok(true);
        }

        return Ok(false);
    }

    [HttpPut]
    public async Task<IActionResult> Update(InviteToCommunityModel chat)
    {
        if (!HttpContext.Request.Cookies.TryGetValue("refreshToken", out var refreshToken))
        {
            return Unauthorized();
        }

        var responseMessage = await _httpClient.PutAsync("InviteToCommunity", JsonContent.Create(chat), refreshToken, Port.CommunicationApi);
        if (responseMessage.StatusCode == System.Net.HttpStatusCode.OK)
        {
            return Ok();
        }

        return BadRequest();
    }

    [HttpDelete("{id:int:min(1)}")]
    public async Task<IActionResult> Delete(int id)
    {
        if (!HttpContext.Request.Cookies.TryGetValue("refreshToken", out var refreshToken))
        {
            return Unauthorized();
        }

        var responseMessage = await _httpClient.DeletAsync($"InviteToCommunity/{id}", refreshToken, Port.CommunicationApi);
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
