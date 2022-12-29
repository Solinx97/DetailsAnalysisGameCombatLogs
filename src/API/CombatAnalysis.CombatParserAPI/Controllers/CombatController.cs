using AutoMapper;
using CombatAnalysis.BL.DTO;
using CombatAnalysis.BL.Interfaces;
using CombatAnalysis.CombatParserAPI.Helpers;
using CombatAnalysis.CombatParserAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace CombatAnalysis.CombatParserAPI.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class CombatController : ControllerBase
{
    private readonly IService<CombatDto, int> _service;
    private readonly IMapper _mapper;
    private readonly ILogger _logger;

    public CombatController(IService<CombatDto, int> service, IMapper mapper, ILogger logger)
    {
        _service = service;
        _mapper = mapper;
        _logger = logger;
    }

    [HttpGet("{id:int:min(1)}")]
    public async Task<IActionResult> GetById(int id)
    {
        var combat = await _service.GetByIdAsync(id);

        return Ok(combat);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var combat = await _service.GetAllAsync();

        return Ok(combat);
    }

    [HttpGet("findByCombatLogId/{combatLogId:int:min(1)}")]
    public async Task<IActionResult> Find(int combatLogId)
    {
        var combats = await _service.GetByParamAsync("CombatLogId", combatLogId);

        return Ok(combats);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CombatModel model)
    {
        try
        {
            SaveCombatDataHelper.CombatData = model.Data;

            var map = _mapper.Map<CombatDto>(model);
            var createdItem = await _service.CreateAsync(map);

            return Ok(createdItem);
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, ex.Message);

            return BadRequest();
        }
    }

    [HttpPut]
    public async Task<IActionResult> Update(CombatModel value)
    {
        try
        {
            var map = _mapper.Map<CombatDto>(value);
            var rowsAffected = await _service.UpdateAsync(map);

            return Ok(rowsAffected);
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, ex.Message);

            return BadRequest();
        }
    }

    [HttpDelete]
    public async Task<IActionResult> Delete(CombatModel combat)
    {
        try
        {
            var map = _mapper.Map<CombatDto>(combat);
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
