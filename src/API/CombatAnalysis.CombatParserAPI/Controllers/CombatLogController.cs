using AutoMapper;
using CombatAnalysis.BL.DTO;
using CombatAnalysis.BL.Interfaces.General;
using CombatAnalysis.CombatParserAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace CombatAnalysis.CombatParserAPI.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class CombatLogController : ControllerBase
{
    private readonly IQueryService<CombatLogDto> _queryCombatLogService;
    private readonly IMutationService<CombatLogDto> _mutationCombatLogService;
    private readonly IMapper _mapper;
    private readonly ILogger<CombatLogController> _logger;

    public CombatLogController(IQueryService<CombatLogDto> queryCombatLogService, IMutationService<CombatLogDto> mutationCombatLogService, 
        IMapper mapper, ILogger<CombatLogController> logger)
    {
        _queryCombatLogService = queryCombatLogService;
        _mutationCombatLogService = mutationCombatLogService;
        _mapper = mapper;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var combatLogs = await _queryCombatLogService.GetAllAsync();

            return Ok(combatLogs);
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
            var combatLog = await _queryCombatLogService.GetByIdAsync(id);

            return Ok(combatLog);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);

            return BadRequest();
        }
    }

    [HttpPost]
    public async Task<IActionResult> Create(CombatLogModel model)
    {
        try
        {
            var map = _mapper.Map<CombatLogDto>(model);
            var createdItem = await _mutationCombatLogService.CreateAsync(map);

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

    [HttpPut]
    public async Task<IActionResult> Update(CombatLogModel value)
    {
        try
        {
            var map = _mapper.Map<CombatLogDto>(value);
            var rowsAffected = await _mutationCombatLogService.UpdateAsync(map);

            return Ok(rowsAffected);
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
            var map = _mapper.Map<CombatLogDto>(item);

            var rowsAffected = await _mutationCombatLogService.DeleteAsync(map);

            return Ok(rowsAffected);
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
