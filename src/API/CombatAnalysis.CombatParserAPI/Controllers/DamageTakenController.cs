using AutoMapper;
using CombatAnalysis.BL.DTO;
using CombatAnalysis.BL.Interfaces;
using CombatAnalysis.BL.Interfaces.Filters;
using CombatAnalysis.CombatParserAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace CombatAnalysis.CombatParserAPI.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class DamageTakenController : ControllerBase
{
    private readonly IPlayerInfoCountService<DamageTakenDto> _service;
    private readonly IGeneralFilterService<DamageTakenDto> _filterService;
    private readonly IMapper _mapper;
    private readonly ILogger<CombatPlayerController> _logger;

    public DamageTakenController(IPlayerInfoCountService<DamageTakenDto> service, IGeneralFilterService<DamageTakenDto> filterService, IMapper mapper, ILogger<CombatPlayerController> logger)
    {
        _service = service;
        _filterService = filterService;
        _mapper = mapper;
        _logger = logger;
    }

    [HttpGet("getByCombatPlayerId")]
    public async Task<IActionResult> GetByCombatPlayerId(int combatPlayerId, int page, int pageSize)
    {
        try
        {
            var damageTakens = await _service.GetByCombatPlayerIdAsync(combatPlayerId, page, pageSize);

            return Ok(damageTakens);

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error get damage taken by combat player id: {Message}", ex.Message);

            return BadRequest();
        }
    }

    [HttpGet("getUniqueTargets/{combatPlayerId}")]
    public async Task<IActionResult> GetUniqueTargets(int combatPlayerId)
    {
        try
        {
            var uniqueTargets = await _filterService.GetTargetNamesByCombatPlayerIdAsync(combatPlayerId);

            return Ok(uniqueTargets);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error get unique damage taken targets: {Message}", ex.Message);

            return BadRequest();
        }
    }

    [HttpGet("count/{combatPlayerId}")]
    public async Task<IActionResult> Count(int combatPlayerId)
    {
        try
        {
            var count = await _service.CountByCombatPlayerIdAsync(combatPlayerId);

            return Ok(count);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error get damage taken count by target: {Message}", ex.Message);

            return BadRequest();
        }
    }

    [HttpGet("getByTarget")]
    public async Task<IActionResult> GetByTarget(int combatPlayerId, string target, int page, int pageSize)
    {
        try
        {
            var damageTakens = await _filterService.GetTargetsByCombatPlayerIdAsync(combatPlayerId, target, page, pageSize);

            return Ok(damageTakens);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error find damage taken by target: {Message}", ex.Message);

            return BadRequest();
        }
    }

    [HttpGet("countByTarget")]
    public async Task<IActionResult> CountByTarget(int combatPlayerId, string target)
    {
        try
        {
            var count = await _filterService.CountTargetsByCombatPlayerIdAsync(combatPlayerId, target);

            return Ok(count);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error get damage taken count by target: {Message}", ex.Message);

            return BadRequest();
        }
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
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);

            return BadRequest();
        }
    }
}
