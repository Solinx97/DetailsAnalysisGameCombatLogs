using CombatAnalysis.WebApp.Consts;
using CombatAnalysis.WebApp.Extensions;
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
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        if (!HttpContext.Request.Cookies.TryGetValue("refreshToken", out var refreshToken))
        {
            return Unauthorized();
        }

        var responseMessage = await _httpClient.GetAsync("Customer", refreshToken, Port.UserApi);
        var customers = await responseMessage.Content.ReadFromJsonAsync<IEnumerable<CustomerModel>>();

        return Ok(customers);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(string id)
    {
        if (!HttpContext.Request.Cookies.TryGetValue("refreshToken", out var refreshToken))
        {
            return Unauthorized();
        }

        var responseMessage = await _httpClient.GetAsync($"Customer/{id}", refreshToken, Port.UserApi);
        var customer = await responseMessage.Content.ReadFromJsonAsync<CustomerModel>();

        return Ok(customer);
    }

    [HttpGet("searchByUserId/{id}")]
    public async Task<IActionResult> SearchByUserId(string id)
    {
        if (!HttpContext.Request.Cookies.TryGetValue("refreshToken", out var refreshToken))
        {
            return Unauthorized();
        }

        var responseMessage = await _httpClient.GetAsync($"Customer/searchByUserId/{id}", refreshToken, Port.UserApi);
        var customers = await responseMessage.Content.ReadFromJsonAsync<IEnumerable<CustomerModel>>();
        var currentCustomer = customers.FirstOrDefault();

        return Ok(currentCustomer);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CustomerModel model)
    {
        if (!HttpContext.Request.Cookies.TryGetValue("refreshToken", out var refreshToken))
        {
            return Unauthorized();
        }

        var responseMessage = await _httpClient.PostAsync("Customer", refreshToken, JsonContent.Create(model), Port.UserApi);
        var customer = await responseMessage.Content.ReadFromJsonAsync<CustomerModel>();

        return Ok(customer);
    }

    [HttpPut]
    public async Task<IActionResult> Edit(CustomerModel model)
    {
        if (!HttpContext.Request.Cookies.TryGetValue("refreshToken", out var refreshToken))
        {
            return Unauthorized();
        }

        await _httpClient.PutAsync("Customer", refreshToken, JsonContent.Create(model), Port.UserApi);

        return Ok();
    }
}
