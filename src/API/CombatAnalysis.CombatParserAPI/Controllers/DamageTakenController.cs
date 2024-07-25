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
    private readonly IPlayerInfoService<DamageTakenDto, int> _service;
    private readonly IMapper _mapper;
    private readonly ILogger<CombatPlayerController> _logger;

    public DamageTakenController(IPlayerInfoService<DamageTakenDto, int> service, IMapper mapper, ILogger<CombatPlayerController> logger)
    {
        _service = service;
        _mapper = mapper;
        _logger = logger;
    }

    [HttpGet("findByCombatPlayerId/{combatPlayerId:int:min(1)}")]
    public async Task<IActionResult> Find(int combatPlayerId)
    {
        var damageTakens = await _service.GetByCombatPlayerIdAsync(combatPlayerId);

        return Ok(damageTakens);
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
