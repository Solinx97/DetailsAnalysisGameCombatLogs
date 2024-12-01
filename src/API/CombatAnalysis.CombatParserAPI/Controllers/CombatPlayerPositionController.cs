using AutoMapper;
using CombatAnalysis.BL.DTO;
using CombatAnalysis.BL.Interfaces;
using CombatAnalysis.CombatParserAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace CombatAnalysis.CombatParserAPI.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class CombatPlayerPositionController : ControllerBase
{
    private readonly IService<CombatPlayerPositionDto, int> _service;
    private readonly IMapper _mapper;
    private readonly ILogger<CombatPlayerController> _logger;

    public CombatPlayerPositionController(IService<CombatPlayerPositionDto, int> service, IMapper mapper, ILogger<CombatPlayerController> logger)
    {
        _service = service;
        _mapper = mapper;
        _logger = logger;
    }

    [HttpGet("findByCombatId/{combatId:int:min(1)}")]
    public async Task<IActionResult> Find(int combatId)
    {
        var combatPlayerPositions = await _service.GetByParamAsync(nameof(CombatPlayerPositionModel.CombatId), combatId);

        return Ok(combatPlayerPositions);
    }

    [HttpGet("{id:int:min(1)}")]
    public async Task<IActionResult> GetById(int id)
    {
        var combatPlayerPosition = await _service.GetByIdAsync(id);

        return Ok(combatPlayerPosition);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CombatPlayerPositionModel model)
    {
        try
        {
            var map = _mapper.Map<CombatPlayerPositionDto>(model);
            var createdItem = await _service.CreateAsync(map);

            return Ok(createdItem);
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, ex.Message);

            return BadRequest();
        }
        catch (Exception ex)
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
