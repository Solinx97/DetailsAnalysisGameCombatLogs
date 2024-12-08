using AutoMapper;
using CombatAnalysis.BL.DTO;
using CombatAnalysis.BL.Interfaces;
using CombatAnalysis.CombatParserAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace CombatAnalysis.CombatParserAPI.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class DamageTakenController : ControllerBase
{
    private readonly IPlayerInfoCountService<DamageTakenDto> _service;
    private readonly IMapper _mapper;
    private readonly ILogger<CombatPlayerController> _logger;

    public DamageTakenController(IPlayerInfoCountService<DamageTakenDto> service, IMapper mapper, ILogger<CombatPlayerController> logger)
    {
        _service = service;
        _mapper = mapper;
        _logger = logger;
    }

    [HttpGet("getByCombatPlayerId")]
    public async Task<IActionResult> GetByCombatPlayerId(int combatPlayerId, int page, int pageSize)
    {
        var damageDones = await _service.GetByCombatPlayerIdAsync(combatPlayerId, page, pageSize);

        return Ok(damageDones);
    }

    [HttpGet("count/{combatPlayerId}")]
    public async Task<IActionResult> Count(int combatPlayerId)
    {
        var countByCombatPlayerId = await _service.CountByCombatPlayerIdAsync(combatPlayerId);

        return Ok(countByCombatPlayerId);
    }

    [HttpPost]
    public async Task<IActionResult> Create(DamageTakenModel model)
    {
        try
        {
            var map = _mapper.Map<DamageTakenDto>(model);
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
