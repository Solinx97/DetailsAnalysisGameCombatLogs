﻿using CombatAnalysis.WebApp.Consts;
using CombatAnalysis.WebApp.Interfaces;
using CombatAnalysis.WebApp.Models.User;
using Microsoft.AspNetCore.Mvc;

namespace CombatAnalysis.WebApp.Controllers.User;

[Route("api/v1/[controller]")]
[ApiController]
public class FriendController : ControllerBase
{
    private readonly IHttpClientHelper _httpClient;

    public FriendController(IHttpClientHelper httpClient)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = Port.UserApi;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(string id)
    {
        var responseMessage = await _httpClient.GetAsync($"Friend/{id}");
        var damageDones = await responseMessage.Content.ReadFromJsonAsync<FriendModel>();

        return Ok(damageDones);
    }

    [HttpPost]
    public async Task<IActionResult> Create(FriendModel model)
    {
        var responseMessage = await _httpClient.PostAsync("Friend", JsonContent.Create(model));
        var customer = await responseMessage.Content.ReadFromJsonAsync<FriendModel>();

        return Ok(customer);
    }
}