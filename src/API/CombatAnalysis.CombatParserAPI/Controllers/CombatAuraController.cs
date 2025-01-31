using AutoMapper;
using CombatAnalysis.BL.DTO;
using CombatAnalysis.BL.Interfaces.General;
using CombatAnalysis.CombatParserAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace CombatAnalysis.CombatParserAPI.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class CombatAuraController : ControllerBase
{
    private readonly IQueryService<CombatAuraDto> _queryService;
    private readonly IMutationService<CombatAuraDto> _mutationService;
    private readonly IMapper _mapper;
    private readonly ILogger<CombatAuraController> _logger;

    public CombatAuraController(IQueryService<CombatAuraDto> queryService, IMutationService<CombatAuraDto> mutationService, 
        IMapper mapper, ILogger<CombatAuraController> logger)
    {
        _queryService = queryService;
        _mutationService = mutationService;
        _mapper = mapper;
        _logger = logger;
    }

    [HttpGet("getByCombatId/{combatId:int:min(1)}")]
    public async Task<IActionResult> GetByCombatId(int combatId)
    {
        try
        {
            var combatAuras = await _queryService.GetByParamAsync(nameof(CombatAuraModel.CombatId), combatId);

            return Ok(combatAuras);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);

            return BadRequest();
        }
    }

    [HttpGet("{id:int:min(1)}")]
    public async Task<IActionResult> GetById(int id)
    {
        try
        {
            var combatAura = await _queryService.GetByIdAsync(id);

            return Ok(combatAura);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);

            return BadRequest();
        }
    }

    [HttpPost]
    public async Task<IActionResult> Create(CombatAuraModel model)
    {
        try
        {
            var map = _mapper.Map<CombatAuraDto>(model);
            var createdItem = await _mutationService.CreateAsync(map);

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
            var item = await GetById(id);
            var map = _mapper.Map<CombatAuraDto>(item);

            var rowsAffected = await _mutationService.DeleteAsync(map);

            return Ok(rowsAffected);
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, ex.Message);

            return BadRequest();
        }
    }
}
