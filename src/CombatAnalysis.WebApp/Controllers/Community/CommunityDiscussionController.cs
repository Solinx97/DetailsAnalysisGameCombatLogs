﻿using CombatAnalysis.WebApp.Attributes;
using CombatAnalysis.WebApp.Consts;
using CombatAnalysis.WebApp.Enums;
using CombatAnalysis.WebApp.Extensions;
using CombatAnalysis.WebApp.Interfaces;
using CombatAnalysis.WebApp.Models.Community;
using Microsoft.AspNetCore.Mvc;

namespace CombatAnalysis.WebApp.Controllers.Community;

[Route("api/v1/[controller]")]
[ApiController]
public class CommunityDiscussionController : ControllerBase
{
    private readonly IHttpClientHelper _httpClient;

    public CommunityDiscussionController(IHttpClientHelper httpClient)
    {
        _httpClient = httpClient;
    }

    [RequireAccessToken]
    [HttpPost]
    public async Task<IActionResult> Create(CommunityDiscussionModel newCommunity)
    {
        if (!Request.Cookies.TryGetValue(AuthenticationCookie.AccessToken.ToString(), out var accessToken))
        {
            return Unauthorized();
        }

        var responseMessage = await _httpClient.PostAsync("CommunityDiscussion", JsonContent.Create(newCommunity), accessToken, Port.CommunicationApi);
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

    [RequireAccessToken]
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        if (!Request.Cookies.TryGetValue(AuthenticationCookie.AccessToken.ToString(), out var accessToken))
        {
            return Unauthorized();
        }

        var responseMessage = await _httpClient.GetAsync("CommunityDiscussion", accessToken, Port.CommunicationApi);
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

    [RequireAccessToken]
    [HttpGet("{id:int:min(1)}")]
    public async Task<IActionResult> GetById(int id)
    {
        if (!Request.Cookies.TryGetValue(AuthenticationCookie.AccessToken.ToString(), out var accessToken))
        {
            return Unauthorized();
        }

        var responseMessage = await _httpClient.GetAsync($"CommunityDiscussion/{id}", accessToken, Port.CommunicationApi);
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

    [RequireAccessToken]
    [HttpGet("findByCommunityId/{id:int:min(1)}")]
    public async Task<IActionResult> FindByCommunityId(int id)
    {
        if (!Request.Cookies.TryGetValue(AuthenticationCookie.AccessToken.ToString(), out var accessToken))
        {
            return Unauthorized();
        }

        var responseMessage = await _httpClient.GetAsync($"CommunityDiscussion/findByCommunityId/{id}", accessToken, Port.CommunicationApi);
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

    [HttpPut]
    public async Task<IActionResult> Update(CommunityDiscussionModel chat)
    {
        var responseMessage = await _httpClient.PutAsync("CommunityDiscussion", JsonContent.Create(chat), Port.CommunicationApi);
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
        var responseMessage = await _httpClient.DeletAsync($"CommunityDiscussion/{id}", Port.CommunicationApi);
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
