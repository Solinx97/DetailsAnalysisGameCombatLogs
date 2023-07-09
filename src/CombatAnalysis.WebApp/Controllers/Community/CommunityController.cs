using CombatAnalysis.WebApp.Consts;
using CombatAnalysis.WebApp.Interfaces;
using CombatAnalysis.WebApp.Models.Community;
using Microsoft.AspNetCore.Mvc;

namespace CombatAnalysis.WebApp.Controllers.Community;

[Route("api/v1/[controller]")]
[ApiController]
public class CommunityController : ControllerBase
{
    private readonly IHttpClientHelper _httpClient;

    public CommunityController(IHttpClientHelper httpClient)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = Port.ChatApi;
    }

    [HttpPost]
    public async Task<IActionResult> Create(CommunityModel newCommunity)
    {
        _httpClient.BaseAddress = Port.ChatApi;

        var responseMessage = await _httpClient.PostAsync("Community", JsonContent.Create(newCommunity));
        if (responseMessage.StatusCode == System.Net.HttpStatusCode.OK)
        {
            var community = await responseMessage.Content.ReadFromJsonAsync<CommunityModel>();
            return Ok(community);
        }

        return BadRequest();
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        _httpClient.BaseAddress = Port.ChatApi;

        var responseMessage = await _httpClient.GetAsync("Community");
        if (responseMessage.StatusCode == System.Net.HttpStatusCode.OK)
        {
            var communities = await responseMessage.Content.ReadFromJsonAsync<IEnumerable<CommunityModel>>();

            return Ok(communities);
        }


        return BadRequest();
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        _httpClient.BaseAddress = Port.ChatApi;

        var responseMessage = await _httpClient.GetAsync($"Community/{id}");
        if (responseMessage.StatusCode == System.Net.HttpStatusCode.OK)
        {
            var community = await responseMessage.Content.ReadFromJsonAsync<CommunityModel>();

            return Ok(community);
        }


        return BadRequest();
    }

    [HttpPut]
    public async Task<IActionResult> Update(CommunityModel chat)
    {
        _httpClient.BaseAddress = Port.ChatApi;

        var responseMessage = await _httpClient.PutAsync("Community", JsonContent.Create(chat));
        if (responseMessage.StatusCode == System.Net.HttpStatusCode.OK)
        {
            return Ok();
        }

        return BadRequest();
    }
}
