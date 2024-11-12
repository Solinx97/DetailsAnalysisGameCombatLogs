using CombatAnalysis.WebApp.Attributes;
using CombatAnalysis.WebApp.Consts;
using CombatAnalysis.WebApp.Enums;
using CombatAnalysis.WebApp.Extensions;
using CombatAnalysis.WebApp.Interfaces;
using CombatAnalysis.WebApp.Models.User;
using Microsoft.AspNetCore.Mvc;

namespace CombatAnalysis.WebApp.Controllers.User;

[RequireAccessToken]
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
        var accessToken = HttpContext.Items[AuthenticationCookie.AccessToken.ToString()] as string;

        var responseMessage = await _httpClient.GetAsync($"RequestToConnect/{id}", accessToken, Port.UserApi);
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
        var accessToken = HttpContext.Items[AuthenticationCookie.AccessToken.ToString()] as string;

        var responseMessage = await _httpClient.GetAsync($"RequestToConnect/searchByOwnerId/{id}", accessToken, Port.UserApi);
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
        var accessToken = HttpContext.Items[AuthenticationCookie.AccessToken.ToString()] as string;

        var responseMessage = await _httpClient.GetAsync($"RequestToConnect/searchByToUserId/{id}", accessToken, Port.UserApi);
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

    [HttpGet("isExist")]
    public async Task<IActionResult> IsExist(string initiatorId, string companionId)
    {
        var accessToken = HttpContext.Items[AuthenticationCookie.AccessToken.ToString()] as string;

        var responseMessage = await _httpClient.GetAsync("RequestToConnect", accessToken, Port.UserApi);
        if (responseMessage.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            return Unauthorized();
        }
        else if (!responseMessage.IsSuccessStatusCode)
        {
            return BadRequest();
        }

        var allRequestsToConnect = await responseMessage.Content.ReadFromJsonAsync<IEnumerable<RequestToConnectModel>>();
        var requestsToConnectToUser = allRequestsToConnect.Where(x => x.ToAppUserId == initiatorId && x.AppUserId == companionId).ToList();
        if (requestsToConnectToUser.Any())
        {
            return Ok(true);
        }

        var requestsToConnectToOwner = allRequestsToConnect.Where(x => x.ToAppUserId == companionId && x.AppUserId == initiatorId).ToList();
        if (requestsToConnectToOwner.Any())
        {
            return Ok(true);
        }

        return Ok(false);
    }

    [HttpPost]
    public async Task<IActionResult> Create(RequestToConnectModel model)
    {
        var accessToken = HttpContext.Items[AuthenticationCookie.AccessToken.ToString()] as string;

        var responseMessage = await _httpClient.PostAsync("RequestToConnect", JsonContent.Create(model), accessToken, Port.UserApi);
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
        var accessToken = HttpContext.Items[AuthenticationCookie.AccessToken.ToString()] as string;

        var responseMessage = await _httpClient.DeletAsync($"RequestToConnect/{id}", accessToken, Port.UserApi);
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
