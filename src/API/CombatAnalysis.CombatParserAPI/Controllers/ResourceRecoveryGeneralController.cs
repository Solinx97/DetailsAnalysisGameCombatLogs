using AutoMapper;
using CombatAnalysis.BL.DTO;
using CombatAnalysis.BL.Interfaces;
using CombatAnalysis.CombatParserAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace CombatAnalysis.CombatParserAPI.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class ResourceRecoveryGeneralController : ControllerBase
{
    private readonly IService<ResourceRecoveryGeneralDto, int> _service;
    private readonly IMapper _mapper;
    private readonly ILogger _logger;

    public ResourceRecoveryGeneralController(IService<ResourceRecoveryGeneralDto, int> service, IMapper mapper, ILogger logger)
    {
        _service = service;
        _mapper = mapper;
        _logger = logger;
    }

    [HttpGet("findByCombatPlayerId/{combatPlayerId:int:min(1)}")]
    public async Task<IActionResult> Find(int combatPlayerId)
    {
        var resourceRecoveryGenerals = await _service.GetByParamAsync("CombatPlayerId", combatPlayerId);
        var map = _mapper.Map<IEnumerable<ResourceRecoveryGeneralModel>>(resourceRecoveryGenerals);

        return Ok(map);
    }

    [HttpPost]
    public async Task<IActionResult> Create(ResourceRecoveryGeneralModel model)
    {
        try
        {
            var map = _mapper.Map<ResourceRecoveryGeneralDto>(model);
            var createdItem = await _service.CreateAsync(map);
            var resultMap = _mapper.Map<ResourceRecoveryGeneralModel>(createdItem);

            return Ok(resultMap);
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, ex.Message);

            return BadRequest();
        }
    }

    [HttpDelete]
    public async Task<IActionResult> Delete(ResourceRecoveryGeneralModel model)
    {
        try
        {
            var map = _mapper.Map<ResourceRecoveryGeneralDto>(model);
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
