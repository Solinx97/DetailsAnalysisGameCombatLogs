using CombatAnalysis.WebApp.Consts;
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
        _httpClient.BaseAddress = Port.ChatApi;
    }

    [HttpPost]
    public async Task<IActionResult> Create(InviteToCommunityModel chat)
    {
        var responseMessage = await _httpClient.PostAsync("InviteToCommunity", JsonContent.Create(chat));
        if (responseMessage.StatusCode == System.Net.HttpStatusCode.OK)
        {
            var groupChat = await responseMessage.Content.ReadFromJsonAsync<InviteToCommunityModel>();
            return Ok(groupChat);
        }

        return BadRequest();
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var responseMessage = await _httpClient.GetAsync("InviteToCommunity");
        if (responseMessage.StatusCode == System.Net.HttpStatusCode.OK)
        {
            var groupChats = await responseMessage.Content.ReadFromJsonAsync<IEnumerable<InviteToCommunityModel>>();

            return Ok(groupChats);
        }


        return BadRequest();
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var responseMessage = await _httpClient.GetAsync($"InviteToCommunity/{id}");
        if (responseMessage.StatusCode == System.Net.HttpStatusCode.OK)
        {
            var groupChat = await responseMessage.Content.ReadFromJsonAsync<InviteToCommunityModel>();

            return Ok(groupChat);
        }


        return BadRequest();
    }

    [HttpPut]
    public async Task<IActionResult> Update(InviteToCommunityModel chat)
    {
        var responseMessage = await _httpClient.PutAsync("InviteToCommunity", JsonContent.Create(chat));
        if (responseMessage.StatusCode == System.Net.HttpStatusCode.OK)
        {
            return Ok();
        }

        return BadRequest();
    }
}
