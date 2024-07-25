using AutoMapper;
using CombatAnalysis.BL.DTO;
using CombatAnalysis.BL.Interfaces;
using CombatAnalysis.CombatParserAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace CombatAnalysis.CombatParserAPI.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class HealDoneController : ControllerBase
{
    private readonly IPlayerInfoService<HealDoneDto, int> _service;
    private readonly IMapper _mapper;
    private readonly ILogger<HealDoneController> _logger;

    public HealDoneController(IPlayerInfoService<HealDoneDto, int> service, IMapper mapper, ILogger<HealDoneController> logger)
    {
        _service = service;
        _mapper = mapper;
        _logger = logger;
    }

    [HttpGet("findByCombatPlayerId/{combatPlayerId:int:min(1)}")]
    public async Task<IActionResult> Find(int combatPlayerId)
    {
        var healDones = await _service.GetByCombatPlayerIdAsync(combatPlayerId);

        return Ok(healDones);
    }

    [HttpPost]
    public async Task<IActionResult> Create(HealDoneModel model)
    {
        try
        {
            var map = _mapper.Map<HealDoneDto>(model);
            var createdItem = await _service.CreateAsync(map);

            return Ok(createdItem);
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, ex.Message);

            return BadRequest();
        }
    }

    [HttpDelete("{id:int:min(1)}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var deletedId = await _service.DeleteAsync(id);

            return Ok(deletedId);
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, ex.Message);

            return BadRequest();
        }
    }
}
