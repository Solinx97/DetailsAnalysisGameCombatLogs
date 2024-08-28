using CombatAnalysis.WebApp.Attributes;
using CombatAnalysis.WebApp.Consts;
using CombatAnalysis.WebApp.Enums;
using CombatAnalysis.WebApp.Extensions;
using CombatAnalysis.WebApp.Interfaces;
using CombatAnalysis.WebApp.Models.Post;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Globalization;

namespace CombatAnalysis.WebApp.Controllers.Post;

[RequireAccessToken]
[Route("api/v1/[controller]")]
[ApiController]
public class CommunityPostController : ControllerBase
{
    private readonly IHttpClientHelper _httpClient;

    public CommunityPostController(IHttpClientHelper httpClient)
    {
        _httpClient = httpClient;
    }

    [HttpGet("count/{communityId}")]
    public async Task<IActionResult> Count(int communityId)
    {
        if (!Request.Cookies.TryGetValue(AuthenticationCookie.AccessToken.ToString(), out var accessToken))
        {
            return Unauthorized();
        }

        var responseMessage = await _httpClient.GetAsync($"CommunityPost/count/{communityId}", accessToken, Port.CommunicationApi);
        var count = await responseMessage.Content.ReadFromJsonAsync<int>();

        return Ok(count);
    }

    [HttpGet("{id:int:min(1)}")]
    public async Task<IActionResult> GetById(int id)
    {
        if (!Request.Cookies.TryGetValue(AuthenticationCookie.AccessToken.ToString(), out var accessToken))
        {
            return Unauthorized();
        }

        var responseMessage = await _httpClient.GetAsync($"CommunityPost/{id}", accessToken, Port.CommunicationApi);
        if (responseMessage.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            return Unauthorized();
        }
        else if (responseMessage.IsSuccessStatusCode)
        {
            var communityPost = await responseMessage.Content.ReadFromJsonAsync<CommunityPostModel>();

            return Ok(communityPost);
        }

        return BadRequest();
    }

    [HttpGet("getByCommunityId")]
    public async Task<IActionResult> GetByCommunityId(int communityId, int pageSize)
    {
        if (!Request.Cookies.TryGetValue(AuthenticationCookie.AccessToken.ToString(), out var accessToken))
        {
            return Unauthorized();
        }

        var responseMessage = await _httpClient.GetAsync($"CommunityPost/getByCommunityId?communityId={communityId}&pageSize={pageSize}", accessToken, Port.CommunicationApi);
        if (responseMessage.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            return Unauthorized();
        }
        else if (responseMessage.IsSuccessStatusCode)
        {
            var posts = await responseMessage.Content.ReadFromJsonAsync<IEnumerable<CommunityPostModel>>();

            return Ok(posts);
        }

        return BadRequest();
    }

    [HttpGet("getMoreByCommunityId")]
    public async Task<IActionResult> GetMoreByCommunityId(int communityId, int offset, int pageSize)
    {
        if (!Request.Cookies.TryGetValue(AuthenticationCookie.AccessToken.ToString(), out var accessToken))
        {
            return Unauthorized();
        }

        var responseMessage = await _httpClient.GetAsync($"CommunityPost/getMoreByCommunityId?communityId={communityId}&offset={offset}&pageSize={pageSize}", accessToken, Port.CommunicationApi);
        if (responseMessage.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            return Unauthorized();
        }
        else if (responseMessage.IsSuccessStatusCode)
        {
            var posts = await responseMessage.Content.ReadFromJsonAsync<IEnumerable<CommunityPostModel>>();

            return Ok(posts);
        }

        return BadRequest();
    }

    [HttpGet("getNewPosts")]
    public async Task<IActionResult> GetNewPosts(int communityId, string checkFrom)
    {
        if (!Request.Cookies.TryGetValue(AuthenticationCookie.AccessToken.ToString(), out var accessToken))
        {
            return Unauthorized();
        }

        var responseMessage = await _httpClient.GetAsync($"CommunityPost/getNewPosts?communityId={communityId}&checkFrom={checkFrom}", accessToken, Port.CommunicationApi);
        if (responseMessage.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            return Unauthorized();
        }
        else if (responseMessage.IsSuccessStatusCode)
        {
            var posts = await responseMessage.Content.ReadFromJsonAsync<IEnumerable<CommunityPostModel>>();

            return Ok(posts);
        }

        return BadRequest();
    }

    [HttpPut]
    public async Task<IActionResult> Update(CommunityPostModel model)
    {
        if (!Request.Cookies.TryGetValue(AuthenticationCookie.AccessToken.ToString(), out var accessToken))
        {
            return Unauthorized();
        }

        var responseMessage = await _httpClient.PutAsync("CommunityPost", JsonContent.Create(model), accessToken, Port.CommunicationApi);
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

    [HttpPost]
    public async Task<IActionResult> Create(CommunityPostModel model)
    {
        if (!Request.Cookies.TryGetValue(AuthenticationCookie.AccessToken.ToString(), out var accessToken))
        {
            return Unauthorized();
        }

        var responseMessage = await _httpClient.PostAsync("CommunityPost", JsonContent.Create(model), accessToken, Port.CommunicationApi);
        if (responseMessage.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            return Unauthorized();
        }
        else if (responseMessage.IsSuccessStatusCode)
        {
            var communityPost = await responseMessage.Content.ReadFromJsonAsync<CommunityPostModel>();

            return Ok(communityPost);
        }

        return BadRequest();
    }

    [HttpDelete("{id:int:min(1)}")]
    public async Task<IActionResult> Delete(int id)
    {
        if (!Request.Cookies.TryGetValue(AuthenticationCookie.AccessToken.ToString(), out var accessToken))
        {
            return Unauthorized();
        }

        var responseMessage = await _httpClient.DeletAsync($"CommunityPost/{id}", accessToken, Port.CommunicationApi);
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
