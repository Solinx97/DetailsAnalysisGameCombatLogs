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
    private readonly IService<CombatPlayerDto, int> _combatPlayerService;
    private readonly IPlayerInfoService<PlayerDeathDto, int> _plaнerDeathService;
    private readonly IMapper _mapper;
    private readonly ILogger _logger;
    private readonly ISqlContextService _sqlContextService;
    private readonly ICombatDataHelper _saveCombatDataHelper;

    public CombatController(IService<CombatDto, int> service, IMapper mapper, ILogger logger,
        ISqlContextService sqlContextService, ICombatDataHelper saveCombatDataHelper, IService<CombatPlayerDto, int> combatPlayerService, IPlayerInfoService<PlayerDeathDto, int> plaнerDeathService)
    {
        _service = service;
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
        var combat = await _service.GetByIdAsync(id);

        return Ok(combat);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var combat = await _service.GetAllAsync();

        return Ok(combat);
    }

    [HttpGet("findByCombatLogId/{combatLogId:int:min(1)}")]
    public async Task<IActionResult> Find(int combatLogId)
    {
        var combats = await _service.GetByParamAsync("CombatLogId", combatLogId);

        return Ok(combats);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CombatModel model)
    {
        using var transaction = await _sqlContextService.BeginTransactionAsync(false);
        try
        {
            var map = _mapper.Map<CombatDto>(model);
            var createdCombat = await _service.CreateAsync(map);

            foreach (var player in model.Players)
            {
                player.CombatId = createdCombat.Id;
                if (string.IsNullOrEmpty(player.Username))
                {
                    continue;
                }

                var createdCombatPlayer = await UploadCombatPlayerAsync(player);
                player.Id = createdCombatPlayer.Id;

                await _saveCombatDataHelper.SaveCombatPlayerAsync(createdCombat, model.PetsId, createdCombatPlayer, model.Data);
            }

            if (model.DeathInfo.Any())
            {
                foreach (var item in model.DeathInfo)
                {
                    var player = model.Players.FirstOrDefault(x => x.Username == item.Username);
                    item.CombatPlayerId = player.Id;

                    await UploadPlayerDeathAsync(item);
                }
            }

            createdCombat.IsReady = true;
            var rowsAffected = await _service.UpdateAsync(createdCombat);
            if (rowsAffected == 0)
            {
                await transaction.RollbackAsync();

                return BadRequest();
            }

            await transaction.CommitAsync();

            return Ok(createdCombat);
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, ex.Message);

            await transaction.RollbackAsync();

            return BadRequest();
        }
        catch (ArgumentException ex)
        {
            _logger.LogError(ex, ex.Message);

            await transaction.RollbackAsync();

            return BadRequest();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);

            await transaction.RollbackAsync();

            return BadRequest();
        }
    }

    [HttpPut]
    public async Task<IActionResult> Update(CombatModel value)
    {
        try
        {
            var map = _mapper.Map<CombatDto>(value);
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
}
