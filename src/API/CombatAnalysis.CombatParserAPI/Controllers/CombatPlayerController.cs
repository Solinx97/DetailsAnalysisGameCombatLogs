using AutoMapper;
using CombatAnalysis.BL.DTO;
using CombatAnalysis.BL.Interfaces;
using CombatAnalysis.CombatParserAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace CombatAnalysis.CombatParserAPI.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class CombatPlayerController : ControllerBase
{
    private readonly IService<CombatPlayerDto> _service;
    private readonly IMapper _mapper;
    private readonly ILogger<CombatPlayerController> _logger;

    public CombatPlayerController(IService<CombatPlayerDto> service, IMapper mapper, ILogger<CombatPlayerController> logger)
    {
        _service = service;
        _mapper = mapper;
        _logger = logger;
    }

    [HttpGet("findByCombatId/{combatId:int:min(1)}")]
    public async Task<IActionResult> Find(int combatId)
    {
        var players = await _service.GetByParamAsync(nameof(CombatPlayerModel.CombatId), combatId);

        return Ok(players);
    }

    [HttpGet("{id:int:min(1)}")]
    public async Task<IActionResult> GetById(int id)
    {
        var combatLog = await _service.GetByIdAsync(id);

        return Ok(combatLog);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CombatPlayerModel model)
    {
        try
        {
            var map = _mapper.Map<CombatPlayerDto>(model);
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
