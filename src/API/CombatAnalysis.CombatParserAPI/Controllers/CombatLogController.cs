using AutoMapper;
using CombatAnalysis.BL.DTO;
using CombatAnalysis.BL.Interfaces;
using CombatAnalysis.CombatParserAPI.Interfaces;
using CombatAnalysis.CombatParserAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace CombatAnalysis.CombatParserAPI.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class CombatLogController : ControllerBase
{
    private readonly IService<CombatLogDto, int> _service;
    private readonly IMapper _mapper;
    private readonly ILogger _logger;
    private readonly ISaveCombatDataHelper _saveCombatDataHelper;

    public CombatLogController(IService<CombatLogDto, int> service, IMapper mapper, ILogger logger, ISaveCombatDataHelper saveCombatDataHelper)
    {
        _service = service;
        _mapper = mapper;
        _logger = logger;
        _saveCombatDataHelper = saveCombatDataHelper;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var combatLogs = await _service.GetAllAsync();

        return Ok(combatLogs);
    }

    [HttpGet("{id:int:min(1)}")]
    public async Task<IActionResult> GetById(int id)
    {
        var combatLog = await _service.GetByIdAsync(id);

        return Ok(combatLog);
    }

    [HttpPost]
    public async Task<IActionResult> Create(List<string> dungeonNames)
    {
        try
        {
            var combatLog = _saveCombatDataHelper.CreateCombatLog(dungeonNames);

            var map = _mapper.Map<CombatLogDto>(combatLog);
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

    [HttpPut]
    public async Task<IActionResult> Update(CombatLogModel value)
    {
        try
        {
            var map = _mapper.Map<CombatLogDto>(value);
            var rowsAffected = await _service.UpdateAsync(map);

            return Ok(rowsAffected);
        }
        catch (ArgumentNullException ex)
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
