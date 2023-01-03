using AutoMapper;
using CombatAnalysis.BL.DTO;
using CombatAnalysis.BL.Interfaces;
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
    private readonly ISqlContextService _sqlContextService;
    private readonly ISaveCombatDataHelper _saveCombatDataHelper;

    public CombatPlayerController(IService<CombatPlayerDto, int> service, ISaveCombatDataHelper saveCombatDataHelper, IMapper mapper, ISqlContextService sqlContextService, ILogger logger)
    {
        _service = service;
        _mapper = mapper;
        _sqlContextService= sqlContextService;
        _logger = logger;
        _saveCombatDataHelper = saveCombatDataHelper;
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
    public async Task<IActionResult> Create(List<CombatPlayerModel> model)
    {
        using var transaction = await _sqlContextService.BeginTransactionAsync();
        try
        {
            foreach (var item in model)
            {
                var map = _mapper.Map<CombatPlayerDto>(item);
                var createdItem = await _service.CreateAsync(map);
                var createdItemToModel = _mapper.Map<CombatPlayerModel>(createdItem);

                await _saveCombatDataHelper.SaveCombatPlayerDataAsync(createdItem.CombatId, createdItemToModel);
            }

            await transaction.CommitAsync();

            return Ok();
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, ex.Message);

            return BadRequest();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);

            await transaction.RollbackAsync();

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
