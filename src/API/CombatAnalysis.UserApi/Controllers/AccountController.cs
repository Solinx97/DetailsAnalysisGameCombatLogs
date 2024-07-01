using AutoMapper;
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
    private readonly ILogger _logger;

    public AccountController(IUserService<AppUserDto> service, IMapper mapper, ILogger logger)
    {
        _service = service;
        _mapper = mapper;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var users = await _service.GetAllAsync();
            if (users == null)
            {
                return BadRequest();
            }

            return Ok(users);
        }
        catch (Exception)
        {
            return BadRequest();
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(string id)
    {
        try
        {
            var user = await _service.GetByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }
        catch (Exception)
        {
            return BadRequest();
        }
    }

    [HttpPut]
    public async Task<IActionResult> Edit(AppUserModel user)
    {
        var map = _mapper.Map<AppUserDto>(user);
        var updatedUser = await _service.UpdateAsync(map);
        if (updatedUser == 0)
        {
            return BadRequest();
        }

        var login = new LoginModel { Email = user.Email, Password = user.Password };
        return Ok();
    }

    [HttpGet("find/{identityUserId}")]
    public async Task<IActionResult> Find(string identityUserId)
    {
        try
        {
            var user = await _service.GetAsync(identityUserId);
            return Ok(user);
        }
        catch (Exception)
        {
            return BadRequest();
        }
    }
}
