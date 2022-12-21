using AutoMapper;
using CombatAnalysis.BL.DTO;
using CombatAnalysis.BL.Interfaces;
using CombatAnalysis.CombatParserAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace CombatAnalysis.CombatParserAPI.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class DamageDoneController : ControllerBase
{
    private readonly IService<DamageDoneDto, int> _service;
    private readonly IMapper _mapper;
    private readonly ILogger _logger;

    public DamageDoneController(IService<DamageDoneDto, int> service, IMapper mapper, ILogger logger)
    {
        _service = service;
        _mapper = mapper;
        _logger = logger;
    }

    [HttpGet("findByCombatPlayerId/{combatPlayerId:int:min(1)}")]
    public async Task<IActionResult> Find(int combatPlayerId)
    {
        var damageDones = await _service.GetByParamAsync("CombatPlayerId", combatPlayerId);
        var map = _mapper.Map<IEnumerable<DamageDoneModel>>(damageDones);

        return Ok(map);
    }

    [HttpPost]
    public async Task<IActionResult> Create(DamageDoneModel model)
    {
        try
        {
            var map = _mapper.Map<DamageDoneDto>(model);
            var createdItem = await _service.CreateAsync(map);
            var resultMap = _mapper.Map<DamageDoneModel>(createdItem);

            return Ok(resultMap);
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, ex.Message);

            return BadRequest();
        }
    }

    [HttpDelete]
    public async Task<IActionResult> Delete(DamageDoneModel model)
    {
        try
        {
            var map = _mapper.Map<DamageDoneDto>(model);
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
