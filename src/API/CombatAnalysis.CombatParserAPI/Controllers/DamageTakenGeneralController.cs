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
    private readonly IService<DamageTakenGeneralDto, int> _service;
    private readonly IMapper _mapper;
    private readonly ILogger _logger;

    public DamageTakenGeneralController(IService<DamageTakenGeneralDto, int> service, IMapper mapper, ILogger logger)
    {
        _service = service;
        _mapper = mapper;
        _logger = logger;
    }

    [HttpGet("findByCombatPlayerId/{combatPlayerId:int:min(1)}")]
    public async Task<IActionResult> Find(int combatPlayerId)
    {
        var damageTakenGenerals = await _service.GetByParamAsync("CombatPlayerId", combatPlayerId);

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

    [HttpDelete]
    public async Task<IActionResult> Delete(DamageTakenGeneralModel model)
    {
        try
        {
            var map = _mapper.Map<DamageTakenGeneralDto>(model);
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
