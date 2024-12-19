using AutoMapper;
using CombatAnalysis.BL.DTO;
using CombatAnalysis.BL.Interfaces;
using CombatAnalysis.BL.Interfaces.General;
using CombatAnalysis.CombatParserAPI.Interfaces;
using CombatAnalysis.CombatParserAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace CombatAnalysis.CombatParserAPI.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class CombatController : ControllerBase
{
    private readonly IQueryService<CombatDto> _queryCombatService;
    private readonly IMutationService<CombatDto> _mutationCombatService;
    private readonly IQueryService<CombatLogDto> _queryCombatLogService;
    private readonly IMutationService<CombatLogDto> _mutationCombatLogService;
    private readonly IMutationService<CombatPlayerDto> _mutationCombatPlayerService;
    private readonly IMapper _mapper;
    private readonly ILogger<CombatController> _logger;
    private readonly ICombatDataHelper _saveCombatDataHelper;
    private readonly ICombatTransactionService _combatTransactionService;

    public CombatController(IQueryService<CombatDto> queryCombatService, IMutationService<CombatDto> mutationCombatService,
        IQueryService<CombatLogDto> queryCombatLogService, IMutationService<CombatLogDto> mutationCombatLogService, 
        IMutationService<CombatPlayerDto> mutationCombatPlayerService, IMapper mapper, 
        ILogger<CombatController> logger, ICombatDataHelper saveCombatDataHelper,
        ICombatTransactionService combatTransactionService)
    {
        _queryCombatService = queryCombatService;
        _mutationCombatService = mutationCombatService;
        _queryCombatLogService = queryCombatLogService;
        _mutationCombatLogService = mutationCombatLogService;
        _mutationCombatPlayerService = mutationCombatPlayerService;
        _mapper = mapper;
        _logger = logger;
        _saveCombatDataHelper = saveCombatDataHelper;
        _combatTransactionService = combatTransactionService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var combats = await _queryCombatService.GetAllAsync();

            return Ok(combats);
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
            var combat = await _queryCombatService.GetByIdAsync(id);

            return Ok(combat);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);

            return BadRequest();
        }
    }

    [HttpGet("getByCombatLogId/{combatLogId:int:min(1)}")]
    public async Task<IActionResult> GetByCombatLogId(int combatLogId)
    {
        try
        {
            var combats = await _queryCombatService.GetByParamAsync(nameof(CombatModel.CombatLogId), combatLogId);

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
            var rowsAffected = await _mutationCombatService.UpdateAsync(map);

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
            var item = await GetById(id);
            var map = _mapper.Map<CombatDto>(item);

            var deletedId = await _mutationCombatService.DeleteAsync(map);

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
        var createdItem = await _mutationCombatPlayerService.CreateAsync(map);
        if (createdItem == null)
        {
            throw new ArgumentException("Combat player did not created");
        }

        return createdItem;
    }

    private async Task<CombatDto> CreateCombatAsync(CombatModel model)
    {
        var map = _mapper.Map<CombatDto>(model);
        var combat = await _mutationCombatService.CreateAsync(map);

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
        var rowsAffected = await _mutationCombatService.UpdateAsync(combat);
        if (rowsAffected == 0)
        {
            throw new InvalidOperationException("Failed to update combat");
        }
    }

    private async Task<int> UpdateCombatLog(int combatLogId)
    {
        var combatLog = await _queryCombatLogService.GetByIdAsync(combatLogId);
        combatLog.CombatsInQueue--;
        combatLog.NumberReadyCombats++;

        var affectedRows = await _mutationCombatLogService.UpdateAsync(combatLog);

        return affectedRows;
    }
}
