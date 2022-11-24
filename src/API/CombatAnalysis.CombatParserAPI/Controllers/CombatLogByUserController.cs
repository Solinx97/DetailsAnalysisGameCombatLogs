using AutoMapper;
using CombatAnalysis.BL.DTO;
using CombatAnalysis.BL.Interfaces;
using CombatAnalysis.CombatParserAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace CombatAnalysis.CombatParserAPI.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class CombatLogByUserController : ControllerBase
{
    private readonly IService<CombatLogByUserDto, int> _service;
    private readonly IMapper _mapper;
    private readonly ILogger _logger;

    public CombatLogByUserController(IService<CombatLogByUserDto, int> service, IMapper mapper, ILogger logger)
    {
        _service = service;
        _mapper = mapper;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var combatLogsByUser = await _service.GetAllAsync();
        var map = _mapper.Map<IEnumerable<CombatLogByUserModel>>(combatLogsByUser);

        return Ok(map);
    }

    [HttpGet("{id:int:min(1)}")]
    public async Task<IActionResult> GetById(int id)
    {
        var combatLogByUser = await _service.GetByIdAsync(id);
        var map = _mapper.Map<CombatLogByUserModel>(combatLogByUser);

        return Ok(map);
    }

    [HttpGet("byUserId/{userId}")]
    public async Task<IActionResult> GetByUserId(string userId)
    {
        var combatLogsByUser = new List<CombatLogByUserModel>();
        var allCombatLogsByUser = await _service.GetAllAsync();
        var mapAllCombatLogsByUser = _mapper.Map<IEnumerable<CombatLogByUserModel>>(allCombatLogsByUser);
        foreach (var item in mapAllCombatLogsByUser)
        {
            if (item.UserId == userId)
            {
                combatLogsByUser.Add(item);
            }
        }

        return Ok(combatLogsByUser);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CombatLogByUserModel model)
    {
        try
        {
            var map = _mapper.Map<CombatLogByUserDto>(model);
            var createdItem = await _service.CreateAsync(map);
            var resultMap = _mapper.Map<CombatLogByUserModel>(createdItem);

            return Ok(resultMap);
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, ex.Message);

            return BadRequest();
        }
    }

    [HttpPut]
    public async Task<IActionResult> Update(CombatLogByUserModel value)
    {
        try
        {
            var map = _mapper.Map<CombatLogByUserDto>(value);
            var rowsAffected = await _service.UpdateAsync(map);

            return Ok(rowsAffected);
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, ex.Message);

            return BadRequest();
        }
    }

    [HttpDelete]
    public async Task<IActionResult> Delete(CombatLogByUserModel value)
    {
        try
        {
            var map = _mapper.Map<CombatLogByUserDto>(value);
            var deletedId = await _service.DeleteAsync(map);

            return Ok(deletedId);
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, ex.Message);

            return BadRequest();
        }
    }
}
