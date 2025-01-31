using CombatAnalysis.WebApp.Attributes;
using CombatAnalysis.WebApp.Consts;
using CombatAnalysis.WebApp.Interfaces;
using CombatAnalysis.WebApp.Models.Post;
using Microsoft.AspNetCore.Mvc;

namespace CombatAnalysis.WebApp.Controllers.Post;

[ServiceFilter(typeof(RequireAccessTokenAttribute))]
[Route("api/v1/[controller]")]
[ApiController]
public class UserPostController : ControllerBase
{
    private readonly IHttpClientHelper _httpClient;

    public UserPostController(IHttpClientHelper httpClient)
    {
        _httpClient = httpClient;
        _httpClient.APIUrl = Cluster.Communication;
    }

    [HttpGet("count/{appUserId}")]
    public async Task<IActionResult> Count(string appUserId)
    {
        var responseMessage = await _httpClient.GetAsync($"UserPost/count/{appUserId}");
        var count = await responseMessage.Content.ReadFromJsonAsync<int>();

        return Ok(count);
    }

    [HttpGet("countByListOfAppUsers/{appUserIds}")]
    public async Task<IActionResult> CountByListOfAppUsers(string appUserIds)
    {
        var responseMessage = await _httpClient.GetAsync($"UserPost/countByListOfAppUsers/{appUserIds}");
        var count = await responseMessage.Content.ReadFromJsonAsync<int>();

        return Ok(count);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var responseMessage = await _httpClient.GetAsync("UserPost");
        if (responseMessage.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            return Unauthorized();
        }
        else if (responseMessage.IsSuccessStatusCode)
        {
            var post = await responseMessage.Content.ReadFromJsonAsync<UserPostModel>();

            return Ok(post);
        }

        return BadRequest();
    }

    [HttpGet("{id:int:min(1)}")]
    public async Task<IActionResult> GetById(int id)
    {
        var responseMessage = await _httpClient.GetAsync($"UserPost/{id}");
        if (responseMessage.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            return Unauthorized();
        }
        else if (responseMessage.IsSuccessStatusCode)
        {
            var post = await responseMessage.Content.ReadFromJsonAsync<UserPostModel>();

            return Ok(post);
        }

        return BadRequest();
    }

    [HttpGet("getByUserId")]
    public async Task<IActionResult> GetByUserId(string appUserId, int pageSize)
    {
        var responseMessage = await _httpClient.GetAsync($"UserPost/getByUserId?appUserId={appUserId}&pageSize={pageSize}");
        if (responseMessage.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            return Unauthorized();
        }
        else if (responseMessage.IsSuccessStatusCode)
        {
            var posts = await responseMessage.Content.ReadFromJsonAsync<IEnumerable<UserPostModel>>();

            return Ok(posts);
        }

        return BadRequest();
    }

    [HttpGet("getMoreByUserId")]
    public async Task<IActionResult> GetMoreByUserId(string appUserId, int offset, int pageSize)
    {
        var responseMessage = await _httpClient.GetAsync($"UserPost/getMoreByUserId?appUserId={appUserId}&offset={offset}&pageSize={pageSize}");
        if (responseMessage.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            return Unauthorized();
        }
        else if (responseMessage.IsSuccessStatusCode)
        {
            var posts = await responseMessage.Content.ReadFromJsonAsync<IEnumerable<UserPostModel>>();

            return Ok(posts);
        }

        return BadRequest();
    }

    [HttpGet("getNewPosts")]
    public async Task<IActionResult> GetNewPosts(string appUserId, string checkFrom)
    {
        var responseMessage = await _httpClient.GetAsync($"UserPost/getNewPosts?appUserId={appUserId}&checkFrom={checkFrom}");
        if (responseMessage.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            return Unauthorized();
        }
        else if (responseMessage.IsSuccessStatusCode)
        {
            var posts = await responseMessage.Content.ReadFromJsonAsync<IEnumerable<UserPostModel>>();

            return Ok(posts);
        }

        return BadRequest();
    }

    [HttpGet("getByListOfUserIds")]
    public async Task<IActionResult> GetByListOfUserIds(string appUserIds, int pageSize)
    {
        var responseMessage = await _httpClient.GetAsync($"UserPost/getByListOfUserIds?appUserIds={appUserIds}&pageSize={pageSize}");
        if (responseMessage.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            return Unauthorized();
        }
        else if (responseMessage.IsSuccessStatusCode)
        {
            var posts = await responseMessage.Content.ReadFromJsonAsync<IEnumerable<UserPostModel>>();

            return Ok(posts);
        }

        return BadRequest();
    }

    [HttpGet("getMoreByListOfUserIds")]
    public async Task<IActionResult> GetMoreByListOfUserIds(string appUserIds, int offset, int pageSize)
    {
        var responseMessage = await _httpClient.GetAsync($"UserPost/getMoreByListOfUserIds?appUserIds={appUserIds}&offset={offset}&pageSize={pageSize}");
        if (responseMessage.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            return Unauthorized();
        }
        else if (responseMessage.IsSuccessStatusCode)
        {
            var posts = await responseMessage.Content.ReadFromJsonAsync<IEnumerable<UserPostModel>>();

            return Ok(posts);
        }

        return BadRequest();
    }

    [HttpGet("getNewByListOfUserIds")]
    public async Task<IActionResult> GetNewByListOfUserIds(string appUserIds, string checkFrom)
    {
        var responseMessage = await _httpClient.GetAsync($"UserPost/getNewByListOfUserIds?appUserIds={appUserIds}&checkFrom={checkFrom}");
        if (responseMessage.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            return Unauthorized();
        }
        else if (responseMessage.IsSuccessStatusCode)
        {
            var posts = await responseMessage.Content.ReadFromJsonAsync<IEnumerable<UserPostModel>>();

            return Ok(posts);
        }

        return BadRequest();
    }

    [HttpPost]
    public async Task<IActionResult> Create(UserPostModel model)
    {
        var responseMessage = await _httpClient.PostAsync("UserPost", JsonContent.Create(model));
        if (responseMessage.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            return Unauthorized();
        }
        else if (responseMessage.IsSuccessStatusCode)
        {
            var post = await responseMessage.Content.ReadFromJsonAsync<UserPostModel>();

            return Ok(post);
        }

        return BadRequest();
    }

    [HttpPut]
    public async Task<IActionResult> Update(UserPostModel model)
    {
        var responseMessage = await _httpClient.PutAsync("UserPost", JsonContent.Create(model));
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
        var responseMessage = await _httpClient.DeletAsync($"UserPost/{id}");
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
