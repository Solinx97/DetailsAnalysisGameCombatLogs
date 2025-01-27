using CombatAnalysis.WebApp.Attributes;
using CombatAnalysis.WebApp.Consts;
using CombatAnalysis.WebApp.Interfaces;
using CombatAnalysis.WebApp.Models.Post;
using Microsoft.AspNetCore.Mvc;

namespace CombatAnalysis.WebApp.Controllers.Post;

[ServiceFilter(typeof(RequireAccessTokenAttribute))]
[Route("api/v1/[controller]")]
[ApiController]
public class CommunityPostController : ControllerBase
{
    private readonly IHttpClientHelper _httpClient;

    public CommunityPostController(IHttpClientHelper httpClient)
    {
        _httpClient = httpClient;
        _httpClient.APIUrl = API.Communication;
    }

    [HttpGet("count/{communityId}")]
    public async Task<IActionResult> Count(int communityId)
    {
        var responseMessage = await _httpClient.GetAsync($"CommunityPost/count/{communityId}");
        if (responseMessage.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            return Unauthorized();
        }
        else if (responseMessage.IsSuccessStatusCode)
        {
            var count = await responseMessage.Content.ReadFromJsonAsync<int>();

            return Ok(count);
        }

        return BadRequest();
    }

    [HttpGet("countByListOfCommunities/{communityIds}")]
    public async Task<IActionResult> CountByListOfAppUsers(string communityIds)
    {
        var responseMessage = await _httpClient.GetAsync($"CommunityPost/countByListOfCommunities/{communityIds}");
        if (responseMessage.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            return Unauthorized();
        }
        else if (responseMessage.IsSuccessStatusCode)
        {
            var count = await responseMessage.Content.ReadFromJsonAsync<int>();

            return Ok(count);
        }

        return BadRequest();
    }

    [HttpGet("{id:int:min(1)}")]
    public async Task<IActionResult> GetById(int id)
    {
        var responseMessage = await _httpClient.GetAsync($"CommunityPost/{id}");
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
        var responseMessage = await _httpClient.GetAsync($"CommunityPost/getByCommunityId?communityId={communityId}&pageSize={pageSize}");
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
        var responseMessage = await _httpClient.GetAsync($"CommunityPost/getMoreByCommunityId?communityId={communityId}&offset={offset}&pageSize={pageSize}");
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
        var responseMessage = await _httpClient.GetAsync($"CommunityPost/getNewPosts?communityId={communityId}&checkFrom={checkFrom}");
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

    [HttpGet("getByListOfCommunityIds")]
    public async Task<IActionResult> GetByListOfCommunityIds(string communityIds, int pageSize)
    {
        var responseMessage = await _httpClient.GetAsync($"CommunityPost/getByListOfCommunityIds?communityIds={communityIds}&pageSize={pageSize}");
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

    [HttpGet("getMoreByListOfCommunityIds")]
    public async Task<IActionResult> GetMoreByListOfCommunityIds(string communityIds, int offset, int pageSize)
    {
        var responseMessage = await _httpClient.GetAsync($"CommunityPost/getMoreByListOfCommunityIds?communityIds={communityIds}&offset={offset}&pageSize={pageSize}");
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

    [HttpGet("getNewByListOfCommunityIds")]
    public async Task<IActionResult> GetNewByListOfCommunityIds(string communityIds, string checkFrom)
    {
        var responseMessage = await _httpClient.GetAsync($"CommunityPost/getNewByListOfCommunityIds?communityIds={communityIds}&checkFrom={checkFrom}");
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

    [HttpPost]
    public async Task<IActionResult> Create(CommunityPostModel model)
    {
        var responseMessage = await _httpClient.PostAsync("CommunityPost", JsonContent.Create(model));
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

    [HttpPut]
    public async Task<IActionResult> Update(CommunityPostModel model)
    {
        var responseMessage = await _httpClient.PutAsync("CommunityPost", JsonContent.Create(model));
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
        var responseMessage = await _httpClient.DeletAsync($"CommunityPost/{id}");
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
