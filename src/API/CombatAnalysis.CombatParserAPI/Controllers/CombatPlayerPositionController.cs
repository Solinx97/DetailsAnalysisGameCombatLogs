using AutoMapper;
using CombatAnalysis.BL.DTO;
using CombatAnalysis.BL.Interfaces.General;
using CombatAnalysis.CombatParserAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace CombatAnalysis.CombatParserAPI.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class CombatPlayerPositionController : ControllerBase
{
    private readonly IQueryService<CombatPlayerPositionDto> _queryCombatPlayerPosition;
    private readonly IMutationService<CombatPlayerPositionDto> _mutationCombatPlayerService;
    private readonly IMapper _mapper;
    private readonly ILogger<CombatPlayerPositionController> _logger;

    public CombatPlayerPositionController(IQueryService<CombatPlayerPositionDto> queryCombatPlayerPosition, IMutationService<CombatPlayerPositionDto> mutationCombatPlayerService,
        IMapper mapper, ILogger<CombatPlayerPositionController> logger)
    {
        _queryCombatPlayerPosition = queryCombatPlayerPosition;
        _mutationCombatPlayerService = mutationCombatPlayerService;
        _mapper = mapper;
        _logger = logger;
    }

    [HttpGet("getByCombatId/{combatId:int:min(1)}")]
    public async Task<IActionResult> GetByCombatId(int combatId)
    {
        try
        {
            var combatPlayerPositions = await _queryCombatPlayerPosition.GetByParamAsync(nameof(CombatPlayerPositionModel.CombatId), combatId);

            return Ok(combatPlayerPositions);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error get combat player positions by combat id: {Message}", ex.Message);

            return BadRequest();
        }
    }

    [HttpGet("{id:int:min(1)}")]
    public async Task<IActionResult> GetById(int id)
    {
        try
        {
            var combatPlayerPosition = await _queryCombatPlayerPosition.GetByIdAsync(id);

            return Ok(combatPlayerPosition);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error get combat player positions by id: {Message}", ex.Message);

            return BadRequest();
        }
    }

    [HttpPost]
    public async Task<IActionResult> Create(CombatPlayerPositionModel model)
    {
        try
        {
            var map = _mapper.Map<CombatPlayerPositionDto>(model);
            var createdItem = await _mutationCombatPlayerService.CreateAsync(map);

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
            var map = _mapper.Map<CombatPlayerPositionDto>(item);

            var rowsAffected = await _mutationCombatPlayerService.DeleteAsync(map);

            return Ok(rowsAffected);
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, ex.Message);

            return BadRequest();
        }
    }
}
