using AutoMapper;
using CombatAnalysis.UserBL.DTO;
using CombatAnalysis.UserBL.Interfaces;
using CombatAnalysis.UserApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CombatAnalysis.UserApi.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
[Authorize]
public class BannedUserController : ControllerBase
{
    private readonly IService<BannedUserDto, int> _service;
    private readonly IMapper _mapper;
    private readonly ILogger<BannedUserController> _logger;

    public BannedUserController(IService<BannedUserDto, int> service, IMapper mapper, ILogger<BannedUserController> logger)
    {
        _service = service;
        _mapper = mapper;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _service.GetAllAsync();

        return Ok(result);
    }

    [HttpGet("{id:int:min(1)}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _service.GetByIdAsync(id);

        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create(BannedUserModel model)
    {
        try
        {
            var map = _mapper.Map<BannedUserDto>(model);
            var result = await _service.CreateAsync(map);

            return Ok(result);
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, $"Create Banned User failed: ${ex.Message}", model);

            return BadRequest();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Create Banned User failed: ${ex.Message}", model);

            return BadRequest();
        }
    }

    [HttpPut]
    public async Task<IActionResult> Update(BannedUserModel model)
    {
        try
        {
            var map = _mapper.Map<BannedUserDto>(model);
            var result = await _service.UpdateAsync(map);

            return Ok(result);
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, $"Update Banned User failed: ${ex.Message}", model);

            return BadRequest();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Update Banned User failed: ${ex.Message}", model);

            return BadRequest();
        }
    }

    [HttpDelete("{id:int:min(1)}")]
    public async Task<IActionResult> Delete(int id)
    {
        var rowsAffected = await _service.DeleteAsync(id);

        return Ok(rowsAffected);
    }
}
