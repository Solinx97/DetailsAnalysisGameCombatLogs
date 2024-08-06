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
    private readonly IService<CombatDto, int> _service;
    private readonly IService<CombatLogDto, int> _combatLogService;
    private readonly IService<CombatPlayerDto, int> _combatPlayerService;
    private readonly IPlayerInfoService<PlayerDeathDto, int> _plaнerDeathService;
    private readonly IMapper _mapper;
    private readonly ILogger<CombatController> _logger;
    private readonly ISqlContextService _sqlContextService;
    private readonly ICombatDataHelper _saveCombatDataHelper;

    public CombatController(IService<CombatDto, int> service, IService<CombatLogDto, int> combatLogService, IMapper mapper, ILogger<CombatController> logger,
        ISqlContextService sqlContextService, ICombatDataHelper saveCombatDataHelper, IService<CombatPlayerDto, int> combatPlayerService, IPlayerInfoService<PlayerDeathDto, int> plaнerDeathService)
    {
        _service = service;
        _combatLogService = combatLogService;
        _mapper = mapper;
        _logger = logger;
        _sqlContextService = sqlContextService;
        _saveCombatDataHelper = saveCombatDataHelper;
        _combatPlayerService = combatPlayerService;
        _plaнerDeathService = plaнerDeathService;
    }

    [HttpGet("{id:int:min(1)}")]
    public async Task<IActionResult> GetById(int id)
    {
        try
        {
            var combat = await _service.GetByIdAsync(id);

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
            var combat = await _service.GetAllAsync();

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
            var combats = await _service.GetByParamAsync("CombatLogId", combatLogId);

            return Ok(combats);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error find Combat byt combat log Id: {Message}", ex.Message);

            return BadRequest();
        }
    }

    [HttpPost]
    public async Task<IActionResult> Create(CombatModel model)
    {
        if (model == null)
        {
            _logger.LogError("Create combat called with null model.");

            return BadRequest("Model cannot be null.");
        }

        using var transaction = await _sqlContextService.BeginTransactionAsync(false);
        try
        {
            var createdCombat = await CreateCombatAsync(model);
            await CreateCombatPlayersAsync(model, createdCombat);
            await CreatePlayerDeathsAsync(model);

            await UpdateCombatAsync(createdCombat);

            var affectedRows = await UpdateCombatLog(createdCombat.CombatLogId);
            if (affectedRows == 0)
            {
                await transaction.RollbackAsync();

                return BadRequest();
            }

            await transaction.CommitAsync();

            return Ok();
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "Argument(s) should not have a null value while Creating combat: {Message}", ex.Message);

            await transaction.RollbackAsync();

            return BadRequest();
        }
        catch (ArgumentException ex)
        {
            _logger.LogError(ex, "Argument(s) incorrect while Creating combat: {Message}", ex.Message);

            await transaction.RollbackAsync();

            return BadRequest();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating combat: {Message}", ex.Message);

            await transaction.RollbackAsync();

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
            var rowsAffected = await _service.UpdateAsync(map);

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
            var deletedId = await _service.DeleteAsync(id);

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

    private async Task<PlayerDeathDto> UploadPlayerDeathAsync(PlayerDeathModel model)
    {
        var map = _mapper.Map<PlayerDeathDto>(model);
        var createdItem = await _plaнerDeathService.CreateAsync(map);
        if (createdItem == null)
        {
            throw new ArgumentException("Player death did not created");
        }

        return createdItem;
    }

    private async Task<CombatDto> CreateCombatAsync(CombatModel model)
    {
        var map = _mapper.Map<CombatDto>(model);
        var combat = await _service.CreateAsync(map);

        return combat;
    }

    private async Task CreateCombatPlayersAsync(CombatModel model, CombatDto createdCombat)
    {
        foreach (var player in model.Players)
        {
            player.CombatId = createdCombat.Id;
            if (string.IsNullOrEmpty(player.Username))
            {
                continue;
            }

            var createdCombatPlayer = await UploadCombatPlayerAsync(player);
            player.Id = createdCombatPlayer.Id;

            await _saveCombatDataHelper.SaveCombatPlayerAsync(createdCombat, model.DeathInfo, model.PetsId, createdCombatPlayer, model.Data);
        }
    }

    private async Task CreatePlayerDeathsAsync(CombatModel model)
    {
        foreach (var item in model.DeathInfo)
        {
            var player = model.Players.FirstOrDefault(x => x.Username == item.Username);
            item.CombatPlayerId = player.Id;

            await UploadPlayerDeathAsync(item);
        }
    }

    private async Task UpdateCombatAsync(CombatDto combat)
    {
        combat.IsReady = true;
        var rowsAffected = await _service.UpdateAsync(combat);
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
