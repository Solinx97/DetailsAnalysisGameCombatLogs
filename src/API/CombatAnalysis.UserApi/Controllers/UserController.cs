using AutoMapper;
using CombatAnalysis.UserAPI.Models;
using CombatAnalysis.UserBL.DTO;
using CombatAnalysis.UserBL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CombatAnalysis.UserAPI.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
[Authorize]
public class UserController(IUserService service, IMapper mapper, ILogger<UserController> logger) : ControllerBase
{
    private readonly IUserService _service = service;
    private readonly IMapper _mapper = mapper;
    private readonly ILogger<UserController> _logger = logger;

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _service.GetAllAsync();

        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(string id)
    {
        var result = await _service.GetByIdAsync(id);
        if (result == null)
        {
            return NotFound();
        }

        return Ok(result);
    }

    [HttpGet("find/{identityUserId}")]
    public async Task<IActionResult> FindByIdentityUserId(string identityUserId)
    {
        var result = await _service.FindByIdentityUserIdAsync(identityUserId);
        if (result == null)
        {
            return NotFound();
        }

        return Ok(result);
    }

    [HttpGet("findByUsername")]
    public async Task<IActionResult> FindByUsernameStartAt(string startAt)
    {
        var result = await _service.FindByUsernameStartAtAsync(startAt);

        return Ok(result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(string id, [FromBody] AppUserModel user)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid AppUser update request received: {@AppUser}", user);

                return ValidationProblem(ModelState);
            }

            if (id != user.Id)
            {
                return BadRequest("Route ID and body ID do not match.");
            }

            var map = _mapper.Map<AppUserDto>(user);
            await _service.UpdateAsync(id, map);

            return NoContent();
        }
        catch (DbUpdateConcurrencyException ex)
        {
            _logger.LogWarning(ex, "The resource was modified by another user. Please refresh and try again.");

            return Conflict(new { message = "The resource was modified by another user. Please refresh and try again." });
        }
    }
}
