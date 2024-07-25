using AutoMapper;
using CombatAnalysis.BL.DTO;
using CombatAnalysis.BL.Interfaces;
using CombatAnalysis.CombatParserAPI.Interfaces;
using CombatAnalysis.CombatParserAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace CombatAnalysis.CombatParserAPI.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class CombatLogByUserController : ControllerBase
{
    private readonly IService<CombatLogByUserDto, int> _service;
    private readonly IService<CombatLogDto, int> _combatLogService;
    private readonly IService<CombatDto, int> _combatService;
    private readonly IService<CombatPlayerDto, int> _combatPlayerService;
    private readonly IMapper _mapper;
    private readonly ILogger<CombatLogByUserController> _logger;
    private readonly ISqlContextService _sqlContextService;
    private readonly ICombatDataHelper _saveCombatDataHelper;

    public CombatLogByUserController(IService<CombatLogByUserDto, int> service, IMapper mapper, ILogger<CombatLogByUserController> logger,
        ISqlContextService sqlContextService, IService<CombatLogDto, int> combatLogService, IService<CombatDto, int> combatService,
        IService<CombatPlayerDto, int> combatPlayerService, ICombatDataHelper saveCombatDataHelper)
    {
        _service = service;
        _combatLogService = combatLogService;
        _combatService = combatService;
        _combatPlayerService = combatPlayerService;
        _mapper = mapper;
        _logger = logger;
        _sqlContextService = sqlContextService;
        _saveCombatDataHelper = saveCombatDataHelper;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var combatLogsByUser = await _service.GetAllAsync();

        return Ok(combatLogsByUser);
    }

    [HttpGet("{id:int:min(1)}")]
    public async Task<IActionResult> GetById(int id)
    {
        var combatLogByUser = await _service.GetByIdAsync(id);

        return Ok(combatLogByUser);
    }

    [HttpGet("byUserId/{id}")]
    public async Task<IActionResult> GetByUserId(string id)
    {
        var result = await _service.GetByParamAsync(nameof(CombatLogByUserModel.UserId), id);

        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CombatLogByUserModel model)
    {
        try
        {
            var map = _mapper.Map<CombatLogByUserDto>(model);
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
    public async Task<IActionResult> Update(CombatLogByUserModel value)
    {
        try
        {
            var map = _mapper.Map<CombatLogByUserDto>(value);
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
        using var transaction = await _sqlContextService.BeginTransactionAsync(false);
        try
        {
            await DeleteCombatLogAsync(id);

            var deletedId = await _service.DeleteAsync(id);

            await transaction.CommitAsync();

            return Ok(deletedId);
        }
        catch (ArgumentException ex)
        {
            _logger.LogError(ex, ex.Message);

            await transaction.RollbackAsync();

            return BadRequest();
        }
    }

    private async Task DeleteCombatLogAsync(int combatLogId)
    {
        await DeleteCombatsAsync(combatLogId);

        var rowsAffected = await _combatLogService.DeleteAsync(combatLogId);
        if (rowsAffected == 0)
        {
            throw new ArgumentException("Combat log did not deleted");
        }
    }

    private async Task DeleteCombatsAsync(int combatLogId)
    {
        var combats = await _combatService.GetByParamAsync(nameof(CombatModel.CombatLogId), combatLogId);
        foreach (var item in combats)
        {
            await DeleteCombatPlayersAsync(item.Id);

            var rowsAffected = await _combatService.DeleteAsync(item.Id);
            if (rowsAffected == 0)
            {
                throw new ArgumentException("Combat did not deleted");
            }
        }
    }

    private async Task DeleteCombatPlayersAsync(int combatId)
    {
        var combatPlayers = await _combatPlayerService.GetByParamAsync(nameof(CombatPlayerModel.CombatId), combatId);
        foreach (var item in combatPlayers)
        {
            await _saveCombatDataHelper.DeleteCombatPlayerDataAsync(item);

            var rowsAffected = await _combatPlayerService.DeleteAsync(item.Id);
            if (rowsAffected == 0)
            {
                throw new ArgumentException("Combat player did not deleted");
            }
        }
    }
}
