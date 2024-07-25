using AutoMapper;
using CombatAnalysis.BL.DTO;
using CombatAnalysis.BL.Interfaces;
using CombatAnalysis.CombatParserAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace CombatAnalysis.CombatParserAPI.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class DamageTakenGeneralController : ControllerBase
{
    private readonly IPlayerInfoService<DamageTakenGeneralDto, int> _service;
    private readonly IMapper _mapper;
    private readonly ILogger<DamageTakenGeneralController> _logger;

    public DamageTakenGeneralController(IPlayerInfoService<DamageTakenGeneralDto, int> service, IMapper mapper, ILogger<DamageTakenGeneralController> logger)
    {
        _service = service;
        _mapper = mapper;
        _logger = logger;
    }

    [HttpGet("findByCombatPlayerId/{combatPlayerId:int:min(1)}")]
    public async Task<IActionResult> Find(int combatPlayerId)
    {
        var damageTakenGenerals = await _service.GetByCombatPlayerIdAsync(combatPlayerId);

        return Ok(damageTakenGenerals);
    }

    [HttpPost]
    public async Task<IActionResult> Create(DamageTakenGeneralModel model)
    {
        try
        {
            var map = _mapper.Map<DamageTakenGeneralDto>(model);
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
