using AutoMapper;
using CombatAnalysis.BL.DTO;
using CombatAnalysis.BL.Interfaces;
using CombatAnalysis.CombatParserAPI.Helpers;
using CombatAnalysis.CombatParserAPI.Interfaces;
using CombatAnalysis.CombatParserAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace CombatAnalysis.CombatParserAPI.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class CombatPlayerController : ControllerBase
{
    private readonly IService<CombatPlayerDto, int> _service;
    private readonly IMapper _mapper;
    private readonly ILogger _logger;
    private readonly SaveCombatDataHelper _saveCombatDataHelper;

    public CombatPlayerController(IService<CombatPlayerDto, int> service, IMapper mapper, IHttpClientHelper httpClient, ILogger logger)
    {
        _service = service;
        _mapper = mapper;
        _logger = logger;
        _saveCombatDataHelper = new SaveCombatDataHelper(mapper, httpClient, logger);
    }

    [HttpGet("findByCombatId/{combatId:int:min(1)}")]
    public async Task<IActionResult> Find(int combatId)
    {
        var players = await _service.GetByParamAsync("CombatId", combatId);

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
            var combatPlayerModel = _mapper.Map<CombatPlayerModel>(createdItem);

            await _saveCombatDataHelper.SaveCombatPlayerDataAsync(createdItem.CombatId, combatPlayerModel);

            return Ok(createdItem);
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, ex.Message);

            return BadRequest();
        }
    }

    [HttpDelete]
    public async Task<IActionResult> Delete(CombatPlayerModel model)
    {
        try
        {
            var map = _mapper.Map<CombatPlayerDto>(model);
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
