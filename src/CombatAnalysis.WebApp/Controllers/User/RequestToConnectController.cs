using CombatAnalysis.WebApp.Consts;
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
        _httpClient.BaseAddress = Port.UserApi;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var responseMessage = await _httpClient.GetAsync($"RequestToConnect/{id}");
        var requestToConnect = await responseMessage.Content.ReadFromJsonAsync<RequestToConnectModel>();

        return Ok(requestToConnect);
    }

    [HttpPost]
    public async Task<IActionResult> Create(RequestToConnectModel model)
    {
        var responseMessage = await _httpClient.PostAsync("RequestToConnect", JsonContent.Create(model));
        var requestToConnect = await responseMessage.Content.ReadFromJsonAsync<RequestToConnectModel>();

        return Ok(requestToConnect);
    }
}
