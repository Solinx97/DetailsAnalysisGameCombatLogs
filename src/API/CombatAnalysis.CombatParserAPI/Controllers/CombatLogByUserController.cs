using AutoMapper;
using CombatAnalysis.BL.DTO;
using CombatAnalysis.BL.Interfaces.General;
using CombatAnalysis.CombatParserAPI.Interfaces;
using CombatAnalysis.CombatParserAPI.Models;
using Microsoft.AspNetCore.Mvc;
using System.Transactions;

namespace CombatAnalysis.CombatParserAPI.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class CombatLogByUserController : ControllerBase
{
    private readonly IQueryService<CombatLogByUserDto> _queryCombatLogByUserService;
    private readonly IMutationService<CombatLogByUserDto> _mutationCombatLogByUserService;
    private readonly IQueryService<CombatLogDto> _queryCombatLogService;
    private readonly IMutationService<CombatLogDto> _mutationCombatLogService;
    private readonly IQueryService<CombatDto> _queryCombatService;
    private readonly IMutationService<CombatDto> _mutationCombatService;
    private readonly IQueryService<CombatPlayerDto> _queryCombatPlayerService;
    private readonly IMutationService<CombatPlayerDto> _mutationCombatPlayerService;
    private readonly IMapper _mapper;
    private readonly ILogger<CombatLogByUserController> _logger;
    private readonly ICombatDataHelper _saveCombatDataHelper;

    public CombatLogByUserController(IQueryService<CombatLogByUserDto> queryCombatLogByUserService, IMutationService<CombatLogByUserDto> mutationCombatLogByUserService, 
        IQueryService<CombatLogDto> queryCombatLogService, IMutationService<CombatLogDto> mutationCombatLogService, 
        IQueryService<CombatDto> queryCombatService, IMutationService<CombatDto> mutationCombatService,
        IQueryService<CombatPlayerDto> queryCombatPlayerService, IMutationService<CombatPlayerDto> mutationCombatPlayerService, 
        IMapper mapper, ILogger<CombatLogByUserController> logger,
        ICombatDataHelper saveCombatDataHelper)
    {
        _queryCombatLogByUserService = queryCombatLogByUserService;
        _mutationCombatLogByUserService = mutationCombatLogByUserService;
        _queryCombatLogService = queryCombatLogService;
        _mutationCombatLogService = mutationCombatLogService;
        _queryCombatService = queryCombatService;
        _mutationCombatService = mutationCombatService;
        _queryCombatPlayerService = queryCombatPlayerService;
        _mutationCombatPlayerService = mutationCombatPlayerService;
        _mapper = mapper;
        _logger = logger;
        _saveCombatDataHelper = saveCombatDataHelper;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var combatLogsByUser = await _queryCombatLogByUserService.GetAllAsync();

            return Ok(combatLogsByUser);
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
            var combatLogByUser = await _queryCombatLogByUserService.GetByIdAsync(id);

            return Ok(combatLogByUser);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);

            return BadRequest();
        }
    }

    [HttpGet("getByUserId/{id}")]
    public async Task<IActionResult> GetByUserId(string id)
    {
        try
        {
            var combatLogsByUser = await _queryCombatLogByUserService.GetByParamAsync(nameof(CombatLogByUserModel.AppUserId), id);

            return Ok(combatLogsByUser);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);

            return BadRequest();
        }
    }

    [HttpPost]
    public async Task<IActionResult> Create(CombatLogByUserModel model)
    {
        try
        {
            var map = _mapper.Map<CombatLogByUserDto>(model);
            var createdItem = await _mutationCombatLogByUserService.CreateAsync(map);

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
            var rowsAffected = await _mutationCombatLogByUserService.UpdateAsync(map);

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
            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            await DeleteCombatLogAsync(id);

            var item = await GetById(id);
            var map = _mapper.Map<CombatLogByUserDto>(item);

            var deletedId = await _mutationCombatLogByUserService.DeleteAsync(map);

            scope.Complete();

            return Ok(deletedId);
        }
        catch (ArgumentException ex)
        {
            _logger.LogError(ex, ex.Message);

            return BadRequest();
        }
    }

    private async Task DeleteCombatLogAsync(int id)
    {
        await DeleteCombatsAsync(id);

        var item = await _queryCombatLogService.GetByIdAsync(id);
        var map = _mapper.Map<CombatLogDto>(item);

        var rowsAffected = await _mutationCombatLogService.DeleteAsync(map);
        if (rowsAffected == 0)
        {
            throw new ArgumentException("Combat log did not deleted");
        }
    }

    private async Task DeleteCombatsAsync(int combatLogId)
    {
        var combats = await _queryCombatService.GetByParamAsync(nameof(CombatModel.CombatLogId), combatLogId);
        foreach (var item in combats)
        {
            await DeleteCombatPlayersAsync(item.Id);

            var rowsAffected = await _mutationCombatService.DeleteAsync(item);
            if (rowsAffected == 0)
            {
                throw new ArgumentException("Combat did not deleted");
            }
        }
    }

    private async Task DeleteCombatPlayersAsync(int combatId)
    {
        var combatPlayers = await _queryCombatPlayerService.GetByParamAsync(nameof(CombatPlayerModel.CombatId), combatId);
        foreach (var item in combatPlayers)
        {
            await _saveCombatDataHelper.DeleteCombatPlayerDataAsync(item);

            var rowsAffected = await _mutationCombatPlayerService.DeleteAsync(item);
            if (rowsAffected == 0)
            {
                throw new ArgumentException("Combat player did not deleted");
            }
        }
    }
}
