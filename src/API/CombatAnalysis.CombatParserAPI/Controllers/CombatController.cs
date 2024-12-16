using AutoMapper;
using CombatAnalysis.BL.DTO;
using CombatAnalysis.BL.Interfaces;
using CombatAnalysis.CombatParserAPI.Interfaces;
using CombatAnalysis.CombatParserAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace CombatAnalysis.CombatParserAPI.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class CombatController : ControllerBase
{
    private readonly IService<CombatDto> _combatService;
    private readonly IService<CombatLogDto> _combatLogService;
    private readonly IService<CombatPlayerDto> _combatPlayerService;
    private readonly IMapper _mapper;
    private readonly ILogger<CombatController> _logger;
    private readonly ICombatDataHelper _saveCombatDataHelper;
    private readonly ICombatTransactionService _combatTransactionService;

    public CombatController(IService<CombatDto> combatService, IService<CombatLogDto> combatLogService, IMapper mapper, ILogger<CombatController> logger,
        ICombatDataHelper saveCombatDataHelper, IService<CombatPlayerDto> combatPlayerService, ICombatTransactionService combatTransactionService)
    {
        _combatService = combatService;
        _combatLogService = combatLogService;
        _mapper = mapper;
        _logger = logger;
        _saveCombatDataHelper = saveCombatDataHelper;
        _combatPlayerService = combatPlayerService;
        _combatTransactionService = combatTransactionService;
    }

    [HttpGet("{id:int:min(1)}")]
    public async Task<IActionResult> GetById(int id)
    {
        try
        {
            var combat = await _combatService.GetByIdAsync(id);

            return Ok(combat);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error get Combat byt Id: {Message}", ex.Message);

            return BadRequest();
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var combat = await _combatService.GetAllAsync();

            return Ok(combat);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error get all Combats: {Message}", ex.Message);

            return BadRequest();
        }
    }

    [HttpGet("findByCombatLogId/{combatLogId:int:min(1)}")]
    public async Task<IActionResult> Find(int combatLogId)
    {
        try
        {
            var combats = await _combatService.GetByParamAsync(nameof(CombatModel.CombatLogId), combatLogId);

            return Ok(combats);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error find Combat byt combat log Id: {Message}", ex.Message);

            return BadRequest();
        }
    }

    [HttpPost]
    public async Task<IActionResult> Create(CombatModel combat)
    {
        if (combat == null)
        {
            _logger.LogError("Create combat called with null model.");

            return BadRequest("Model cannot be null.");
        }

        try
        {
            await _combatTransactionService.BeginTransactionAsync();

            var createdCombat = await CreateCombatAsync(combat);
            combat.Id = createdCombat.Id;

            await CreateCombatPlayersAsync(combat);

            await UpdateCombatAsync(createdCombat);

            var affectedRows = await UpdateCombatLog(createdCombat.CombatLogId);
            if (affectedRows == 0)
            {
                return BadRequest();
            }

            await _combatTransactionService.CommitTransactionAsync();

            return Ok();
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "Argument(s) should not have a null value while Creating combat: {Message}", ex.Message);

            await _combatTransactionService.RollbackTransactionAsync();

            return BadRequest();
        }
        catch (ArgumentException ex)
        {
            _logger.LogError(ex, "Argument(s) incorrect while Creating combat: {Message}", ex.Message);

            await _combatTransactionService.RollbackTransactionAsync();

            return BadRequest();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating combat: {Message}", ex.Message);

            await _combatTransactionService.RollbackTransactionAsync();

            return BadRequest();
        }
    }

    [HttpPut]
    public async Task<IActionResult> Update(CombatModel model)
    {
        if (model == null)
        {
            _logger.LogError("Update combat called with null model.");

            return BadRequest("Model cannot be null.");
        }

        try
        {
            var map = _mapper.Map<CombatDto>(model);
            var rowsAffected = await _combatService.UpdateAsync(map);

            return Ok(rowsAffected);
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "Error update Combat {Message}", ex.Message);

            return BadRequest();
        }
    }

    [HttpDelete("{id:int:min(1)}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var deletedId = await _combatService.DeleteAsync(id);

            return Ok(deletedId);
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, ex.Message);

            return BadRequest();
        }
    }

    private async Task<CombatPlayerDto> UploadCombatPlayerAsync(CombatPlayerModel model)
    {
        var map = _mapper.Map<CombatPlayerDto>(model);
        var createdItem = await _combatPlayerService.CreateAsync(map);
        if (createdItem == null)
        {
            throw new ArgumentException("Combat player did not created");
        }

        return createdItem;
    }

    private async Task<CombatDto> CreateCombatAsync(CombatModel model)
    {
        var map = _mapper.Map<CombatDto>(model);
        var combat = await _combatService.CreateAsync(map);

        return combat;
    }

    private async Task CreateCombatPlayersAsync(CombatModel combat)
    {
        foreach (var player in combat.Players)
        {
            player.CombatId = combat.Id;
            if (string.IsNullOrEmpty(player.Username))
            {
                continue;
            }

            var createdCombatPlayer = await UploadCombatPlayerAsync(player);
            player.Id = createdCombatPlayer.Id;
        }

        await _saveCombatDataHelper.SaveCombatPlayerAsync(combat);
    }

    private async Task UpdateCombatAsync(CombatDto combat)
    {
        combat.IsReady = true;
        var rowsAffected = await _combatService.UpdateAsync(combat);
        if (rowsAffected == 0)
        {
            throw new InvalidOperationException("Failed to update combat");
        }
    }

    private async Task<int> UpdateCombatLog(int combatLogId)
    {
        var combatLog = await _combatLogService.GetByIdAsync(combatLogId);
        combatLog.CombatsInQueue--;
        combatLog.NumberReadyCombats++;

        var affectedRows = await _combatLogService.UpdateAsync(combatLog);

        return affectedRows;
    }
}
