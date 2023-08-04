using CombatAnalysis.WebApp.Consts;
using CombatAnalysis.WebApp.Extensions;
using CombatAnalysis.WebApp.Interfaces;
using CombatAnalysis.WebApp.Models.User;
using Microsoft.AspNetCore.Mvc;

namespace CombatAnalysis.WebApp.Controllers.User;

[Route("api/v1/[controller]")]
[ApiController]
public class RequestToConnectController : ControllerBase
{
    private readonly IHttpClientHelper _httpClient;

    public RequestToConnectController(IHttpClientHelper httpClient)
    {
        _httpClient = httpClient;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        if (!HttpContext.Request.Cookies.TryGetValue("refreshToken", out var refreshToken))
        {
            return Unauthorized();
        }

        var responseMessage = await _httpClient.GetAsync($"RequestToConnect/{id}", refreshToken, Port.UserApi);
        if (responseMessage.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            return Unauthorized();
        }
        else if (responseMessage.IsSuccessStatusCode)
        {
            var requestToConnect = await responseMessage.Content.ReadFromJsonAsync<RequestToConnectModel>();

            return Ok(requestToConnect);
        }

        return BadRequest();
    }

    [HttpGet("searchByOwnerId/{id}")]
    public async Task<IActionResult> SearchByOwnerId(string id)
    {
        if (!HttpContext.Request.Cookies.TryGetValue("refreshToken", out var refreshToken))
        {
            return Unauthorized();
        }

        var responseMessage = await _httpClient.GetAsync($"RequestToConnect/searchByOwnerId/{id}", refreshToken, Port.UserApi);
        if (responseMessage.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            return Unauthorized();
        }
        else if (responseMessage.IsSuccessStatusCode)
        {
            var requestsToConnect = await responseMessage.Content.ReadFromJsonAsync<IEnumerable<RequestToConnectModel>>();

            return Ok(requestsToConnect);
        }

        return BadRequest();
    }

    [HttpGet("searchByToUserId/{id}")]
    public async Task<IActionResult> SearchByToUserId(string id)
    {
        if (!HttpContext.Request.Cookies.TryGetValue("refreshToken", out var refreshToken))
        {
            return Unauthorized();
        }

        var responseMessage = await _httpClient.GetAsync($"RequestToConnect/searchByToUserId/{id}", refreshToken, Port.UserApi);
        if (responseMessage.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            return Unauthorized();
        }
        else if (responseMessage.IsSuccessStatusCode)
        {
            var requestsToConnect = await responseMessage.Content.ReadFromJsonAsync<IEnumerable<RequestToConnectModel>>();

            return Ok(requestsToConnect);
        }

        return BadRequest();
    }

    [HttpPost]
    public async Task<IActionResult> Create(RequestToConnectModel model)
    {
        if (!HttpContext.Request.Cookies.TryGetValue("refreshToken", out var refreshToken))
        {
            return Unauthorized();
        }

        var responseMessage = await _httpClient.PostAsync("RequestToConnect", JsonContent.Create(model), refreshToken, Port.UserApi);
        if (responseMessage.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            return Unauthorized();
        }
        else if (responseMessage.IsSuccessStatusCode)
        {
            var requestToConnect = await responseMessage.Content.ReadFromJsonAsync<RequestToConnectModel>();

            return Ok(requestToConnect);
        }

        return BadRequest();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        if (!HttpContext.Request.Cookies.TryGetValue("refreshToken", out var refreshToken))
        {
            return Unauthorized();
        }

        var responseMessage = await _httpClient.DeletAsync($"RequestToConnect/{id}", refreshToken, Port.UserApi);
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
