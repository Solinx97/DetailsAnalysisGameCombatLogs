using AutoMapper;
using CombatAnalysis.BL.DTO;
using CombatAnalysis.BL.Interfaces;
using CombatAnalysis.CombatParserAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace CombatAnalysis.CombatParserAPI.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class ResourceRecoveryController : ControllerBase
{
    private readonly IService<ResourceRecoveryDto, int> _service;
    private readonly IMapper _mapper;
    private readonly ILogger _logger;

    public ResourceRecoveryController(IService<ResourceRecoveryDto, int> service, IMapper mapper, ILogger logger)
    {
        _service = service;
        _mapper = mapper;
        _logger = logger;
    }

    [HttpGet("findByCombatPlayerId/{combatPlayerId:int:min(1)}")]
    public async Task<IActionResult> Find(int combatPlayerId)
    {
        var resourceRecoveryes = await _service.GetByParamAsync("CombatPlayerId", combatPlayerId);
        var map = _mapper.Map<IEnumerable<ResourceRecoveryModel>>(resourceRecoveryes);

        return Ok(map);
    }

    [HttpPost]
    public async Task<IActionResult> Create(ResourceRecoveryModel model)
    {
        try
        {
            var map = _mapper.Map<ResourceRecoveryDto>(model);
            var createdItem = await _service.CreateAsync(map);
            var resultMap = _mapper.Map<ResourceRecoveryModel>(createdItem);

            return Ok(resultMap);
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, ex.Message);

            return BadRequest();
        }
    }

    [HttpDelete]
    public async Task<IActionResult> Delete(ResourceRecoveryModel model)
    {
        try
        {
            var map = _mapper.Map<ResourceRecoveryDto>(model);
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
