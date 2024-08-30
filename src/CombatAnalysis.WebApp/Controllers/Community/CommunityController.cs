using CombatAnalysis.WebApp.Attributes;
using CombatAnalysis.WebApp.Consts;
using CombatAnalysis.WebApp.Enums;
using CombatAnalysis.WebApp.Extensions;
using CombatAnalysis.WebApp.Interfaces;
using CombatAnalysis.WebApp.Models.Community;
using Microsoft.AspNetCore.Mvc;

namespace CombatAnalysis.WebApp.Controllers.Community;

[RequireAccessToken]
[Route("api/v1/[controller]")]
[ApiController]
public class CommunityController : ControllerBase
{
    private readonly IHttpClientHelper _httpClient;

    public CommunityController(IHttpClientHelper httpClient)
    {
        _httpClient = httpClient;
    }

    [HttpPost]
    public async Task<IActionResult> Create(CommunityModel newCommunity)
    {
        if (!Request.Cookies.TryGetValue(AuthenticationCookie.AccessToken.ToString(), out var accessToken))
        {
            return Unauthorized();
        }

        var responseMessage = await _httpClient.PostAsync("Community", JsonContent.Create(newCommunity), accessToken, Port.CommunicationApi);
        if (responseMessage.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            return Unauthorized();
        }
        else if (responseMessage.IsSuccessStatusCode)
        {
            var community = await responseMessage.Content.ReadFromJsonAsync<CommunityModel>();

            return Ok(community);
        }

        return BadRequest();
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var responseMessage = await _httpClient.GetAsync("Community", Port.CommunicationApi);
        if (responseMessage.IsSuccessStatusCode)
        {
            var communities = await responseMessage.Content.ReadFromJsonAsync<IEnumerable<CommunityModel>>();

            return Ok(communities);
        }

        return BadRequest();
    }

    [HttpGet("{id:int:min(1)}")]
    public async Task<IActionResult> GetById(int id)
    {
        var responseMessage = await _httpClient.GetAsync($"Community/{id}", Port.CommunicationApi);
        if (responseMessage.IsSuccessStatusCode)
        {
            var community = await responseMessage.Content.ReadFromJsonAsync<CommunityModel>();

            return Ok(community);
        }

        return BadRequest();
    }

    [HttpPut]
    public async Task<IActionResult> Update(CommunityModel chat)
    {
        var responseMessage = await _httpClient.PutAsync("Community", JsonContent.Create(chat), Port.CommunicationApi);
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
        var responseMessage = await _httpClient.DeletAsync($"Community/{id}", Port.CommunicationApi);
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

    [HttpGet("getWithPagination")]
    public async Task<IActionResult> GetWithPaginationAsync(int pageSize)
    {
        var responseMessage = await _httpClient.GetAsync($"Community/getWithPagination?&pageSize={pageSize}", Port.CommunicationApi);
        if (responseMessage.IsSuccessStatusCode)
        {
            var communities = await responseMessage.Content.ReadFromJsonAsync<IEnumerable<CommunityModel>>();

            return Ok(communities);
        }

        return BadRequest();
    }

    [HttpGet("getMoreWithPagination")]
    public async Task<IActionResult> GetMoreWithPaginationAsync(int offset, int pageSize)
    {
        var responseMessage = await _httpClient.GetAsync($"Community/getMoreWithPagination?offset={offset}&pageSize={pageSize}", Port.CommunicationApi);
        if (responseMessage.IsSuccessStatusCode)
        {
            var communities = await responseMessage.Content.ReadFromJsonAsync<IEnumerable<CommunityModel>>();

            return Ok(communities);
        }

        return BadRequest();
    }

    [HttpGet("count")]
    public async Task<IActionResult> Count()
    {
        var responseMessage = await _httpClient.GetAsync("Community/count", Port.CommunicationApi);
        if (responseMessage.IsSuccessStatusCode)
        {
            var count = await responseMessage.Content.ReadFromJsonAsync<int>();

            return Ok(count);
        }

        return BadRequest();
    }
}
