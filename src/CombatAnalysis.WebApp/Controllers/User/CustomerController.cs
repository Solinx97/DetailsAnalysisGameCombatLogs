using CombatAnalysis.WebApp.Consts;
using CombatAnalysis.WebApp.Interfaces;
using CombatAnalysis.WebApp.Models.User;
using Microsoft.AspNetCore.Mvc;

namespace CombatAnalysis.WebApp.Controllers.User;

[Route("api/v1/[controller]")]
[ApiController]
public class CustomerController : ControllerBase
{
    private readonly IHttpClientHelper _httpClient;

    public CustomerController(IHttpClientHelper httpClient)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = Port.UserApi;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(string id)
    {
        var responseMessage = await _httpClient.GetAsync($"Customer/{id}");
        var customer = await responseMessage.Content.ReadFromJsonAsync<CustomerModel>();

        return Ok(customer);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CustomerModel model)
    {
        var responseMessage = await _httpClient.PostAsync("Customer", JsonContent.Create(model));
        var customer = await responseMessage.Content.ReadFromJsonAsync<CustomerModel>();

        return Ok(customer);
    }
}
