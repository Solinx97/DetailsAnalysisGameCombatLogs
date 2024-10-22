﻿using AutoMapper;
using CombatAnalysis.CustomerBL.DTO;
using CombatAnalysis.CustomerBL.Interfaces;
using CombatAnalysis.UserApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CombatAnalysis.UserApi.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
[Authorize]
public class AccountController : ControllerBase
{
    private readonly IUserService<AppUserDto> _service;
    private readonly IMapper _mapper;
    private readonly ILogger<AccountController> _logger;

    public AccountController(IUserService<AppUserDto> service, IMapper mapper, ILogger<AccountController> logger)
    {
        _service = service;
        _mapper = mapper;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var results = await _service.GetAllAsync();

        return Ok(results);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(string id)
    {
        var result = await _service.GetByIdAsync(id);

        return Ok(result);
    }

    [AllowAnonymous]
    [HttpPost]
    public async Task<IActionResult> Create(AppUserModel model)
    {
        try
        {
            model.Id = Guid.NewGuid().ToString();

            var map = _mapper.Map<AppUserDto>(model);
            var result = await _service.CreateAsync(map);

            return Ok(result);
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, $"Create App User failed: ${ex.Message}", model);

            return BadRequest();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Create App User failed: ${ex.Message}", model);

            return BadRequest();
        }
    }

    [HttpPut]
    public async Task<IActionResult> Update(AppUserModel model)
    {
        try
        {
            var map = _mapper.Map<AppUserDto>(model);
            var result = await _service.UpdateAsync(map);

            return Ok(result);
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, $"Update App User failed: ${ex.Message}", model);

            return BadRequest();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Update App User failed: ${ex.Message}", model);

            return BadRequest();
        }
    }

    [HttpGet("find/{identityUserId}")]
    public async Task<IActionResult> Find(string identityUserId)
    {
        var result = await _service.GetAsync(identityUserId);

        return Ok(result);
    }

    [HttpGet("check/{username}")]
    [AllowAnonymous]
    public async Task<IActionResult> CheckByUsername(string username)
    {
        var usernameAlreadyUsed = await _service.CheckByUsernameAsync(username);

        return Ok(usernameAlreadyUsed);
    }
}
