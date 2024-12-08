using AutoMapper;
using CombatAnalysis.BL.DTO;
using CombatAnalysis.BL.Interfaces;
using CombatAnalysis.CombatParserAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace CombatAnalysis.CombatParserAPI.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class CombatAuraController : ControllerBase
{
    private readonly IService<CombatAuraDto> _service;
    private readonly IMapper _mapper;
    private readonly ILogger<CombatAuraController> _logger;

    public CombatAuraController(IService<CombatAuraDto> service, IMapper mapper, ILogger<CombatAuraController> logger)
    {
        _service = service;
        _mapper = mapper;
        _logger = logger;
    }

    [HttpGet("findByCombatId/{combatId:int:min(1)}")]
    public async Task<IActionResult> Find(int combatId)
    {
        var combatAuras = await _service.GetByParamAsync(nameof(CombatAuraModel.CombatId), combatId);

        return Ok(combatAuras);
    }

    [HttpGet("{id:int:min(1)}")]
    public async Task<IActionResult> GetById(int id)
    {
        var combatAura = await _service.GetByIdAsync(id);

        return Ok(combatAura);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CombatAuraModel model)
    {
        try
        {
            var map = _mapper.Map<CombatAuraDto>(model);
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
