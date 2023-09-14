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
        if (responseMessage.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            return Unauthorized();
        }
        else if (responseMessage.IsSuccessStatusCode)
        {
            var customers = await responseMessage.Content.ReadFromJsonAsync<IEnumerable<CustomerModel>>();

            return Ok(customers);
        }

        return BadRequest();
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(string id)
    {
        if (!HttpContext.Request.Cookies.TryGetValue("refreshToken", out var refreshToken))
        {
            return Unauthorized();
        }

        var responseMessage = await _httpClient.GetAsync($"Customer/{id}", refreshToken, Port.UserApi);
        if (responseMessage.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            return Unauthorized();
        }
        else if (responseMessage.IsSuccessStatusCode)
        {
            if (responseMessage.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var customer = await responseMessage.Content.ReadFromJsonAsync<CustomerModel>();

                return Ok(customer);
            }

            return NotFound();
        }

        return BadRequest();
    }

    [HttpGet("searchByUserId/{id}")]
    public async Task<IActionResult> SearchByUserId(string id)
    {
        if (!HttpContext.Request.Cookies.TryGetValue("refreshToken", out var refreshToken))
        {
            return Unauthorized();
        }

        var responseMessage = await _httpClient.GetAsync($"Customer/searchByUserId/{id}", refreshToken, Port.UserApi);
        if (responseMessage.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            return Unauthorized();
        }
        else if (responseMessage.IsSuccessStatusCode)
        {
            var customers = await responseMessage.Content.ReadFromJsonAsync<IEnumerable<CustomerModel>>();
            var currentCustomer = customers.FirstOrDefault();

            return Ok(currentCustomer);
        }

        return BadRequest();
    }

    [HttpPost]
    public async Task<IActionResult> Create(CustomerModel model)
    {
        if (!HttpContext.Request.Cookies.TryGetValue("refreshToken", out var refreshToken))
        {
            return Unauthorized();
        }

        var responseMessage = await _httpClient.PostAsync("Customer", JsonContent.Create(model), refreshToken, Port.UserApi);
        if (responseMessage.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            return Unauthorized();
        }
        else if (responseMessage.IsSuccessStatusCode)
        {
            var customer = await responseMessage.Content.ReadFromJsonAsync<CustomerModel>();

            return Ok(customer);
        }

        return BadRequest();
    }

    [HttpPut]
    public async Task<IActionResult> Edit(CustomerModel model)
    {
        if (!HttpContext.Request.Cookies.TryGetValue("refreshToken", out var refreshToken))
        {
            return Unauthorized();
        }

        var responseMessage = await _httpClient.PutAsync("Customer", JsonContent.Create(model), refreshToken, Port.UserApi);
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

    [HttpGet("checkIfCustomerExist/{username}")]
    public async Task<IActionResult> CheckIfCustomerExist(string username)
    {
        var responseMessage = await _httpClient.GetAsync($"Customer/checkIfCustomerExist/{username}", Port.UserApi);
        if (responseMessage.IsSuccessStatusCode)
        {
            var customerIsExist = await responseMessage.Content.ReadFromJsonAsync<bool>();

            return Ok(customerIsExist);
        }

        return BadRequest();
    }
}
