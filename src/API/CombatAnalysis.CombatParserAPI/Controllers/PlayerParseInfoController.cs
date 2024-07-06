using AutoMapper;
using CombatAnalysis.BL.DTO;
using CombatAnalysis.BL.Interfaces;
using CombatAnalysis.CombatParserAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace CombatAnalysis.CombatParserAPI.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class PlayerParseInfoController : ControllerBase
{
    private readonly IService<PlayerParseInfoDto, int> _service;
    private readonly IMapper _mapper;
    private readonly ILogger<PlayerParseInfoController> _logger;

    public PlayerParseInfoController(IService<PlayerParseInfoDto, int> service, IMapper mapper, ILogger<PlayerParseInfoController> logger)
    {
        _service = service;
        _mapper = mapper;
        _logger = logger;
    }

    [HttpGet("findByCombatPlayerId/{combatPlayerId:int:min(1)}")]
    public async Task<IActionResult> Find(int combatPlayerId)
    {
        var playerParseInfo = await _service.GetByParamAsync("CombatPlayerId", combatPlayerId);

        return Ok(playerParseInfo);
    }

    [HttpGet("{id:int:min(1)}")]
    public async Task<IActionResult> GetById(int id)
    {
        var combatLog = await _service.GetByIdAsync(id);

        return Ok(combatLog);
    }

    [HttpPost]
    public async Task<IActionResult> Create(PlayerParseInfoModel model)
    {
        try
        {
            var map = _mapper.Map<PlayerParseInfoDto>(model);
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
