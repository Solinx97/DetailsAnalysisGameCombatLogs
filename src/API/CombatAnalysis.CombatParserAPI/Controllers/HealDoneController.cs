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
    private readonly IService<HealDoneDto, int> _service;
    private readonly IMapper _mapper;
    private readonly ILogger _logger;

    public HealDoneController(IService<HealDoneDto, int> service, IMapper mapper, ILogger logger)
    {
        _service = service;
        _mapper = mapper;
        _logger = logger;
    }

    [HttpGet("findByCombatPlayerId/{combatPlayerId:int:min(1)}")]
    public async Task<IActionResult> Find(int combatPlayerId)
    {
        var healDones = await _service.GetByParamAsync("CombatPlayerId", combatPlayerId);
        var map = _mapper.Map<IEnumerable<HealDoneModel>>(healDones);

        return Ok(map);
    }

    [HttpPost]
    public async Task<IActionResult> Create(HealDoneModel model)
    {
        try
        {
            var map = _mapper.Map<HealDoneDto>(model);
            var createdItem = await _service.CreateAsync(map);
            var resultMap = _mapper.Map<HealDoneModel>(createdItem);

            return Ok(resultMap);
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, ex.Message);

            return BadRequest();
        }
    }

    [HttpDelete]
    public async Task<IActionResult> Delete(HealDoneModel value)
    {
        try
        {
            var map = _mapper.Map<HealDoneDto>(value);
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
