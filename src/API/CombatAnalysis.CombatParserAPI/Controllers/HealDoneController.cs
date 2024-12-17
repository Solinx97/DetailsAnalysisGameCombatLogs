using AutoMapper;
using CombatAnalysis.BL.DTO;
using CombatAnalysis.BL.Interfaces;
using CombatAnalysis.BL.Interfaces.Filters;
using CombatAnalysis.BL.Interfaces.General;
using CombatAnalysis.CombatParserAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace CombatAnalysis.CombatParserAPI.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class HealDoneController : ControllerBase
{
    private readonly IMutationService<HealDoneDto> _mutationService;
    private readonly IPlayerInfoService<HealDoneDto> _playerInfoService;
    private readonly ICountService<HealDoneDto> _countService;
    private readonly IGeneralFilterService<HealDoneDto> _filterService;
    private readonly IMapper _mapper;
    private readonly ILogger<HealDoneController> _logger;

    public HealDoneController(IMutationService<HealDoneDto> mutationService, IPlayerInfoService<HealDoneDto> playerInfoService, 
        ICountService<HealDoneDto> countService, IGeneralFilterService<HealDoneDto> filterService,
        IMapper mapper, ILogger<HealDoneController> logger)
    {
        _mutationService = mutationService;
        _playerInfoService = playerInfoService;
        _countService = countService;
        _filterService = filterService;
        _mapper = mapper;
        _logger = logger;
    }

    [HttpGet("getByCombatPlayerId")]
    public async Task<IActionResult> GetByCombatPlayerId(int combatPlayerId, int page, int pageSize)
    {
        try
        {
            var healDones = await _playerInfoService.GetByCombatPlayerIdAsync(combatPlayerId, page, pageSize);

            return Ok(healDones);

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error get heal done by combat player id: {Message}", ex.Message);

            return BadRequest();
        }
    }

    [HttpGet("getUniqueTargets/{combatPlayerId}")]
    public async Task<IActionResult> GetUniqueTargets(int combatPlayerId)
    {
        try
        {
            var uniqueTargets = await _filterService.GetTargetNamesByCombatPlayerIdAsync(combatPlayerId);

            return Ok(uniqueTargets);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error get unique heal done targets: {Message}", ex.Message);

            return BadRequest();
        }
    }

    [HttpGet("count/{combatPlayerId}")]
    public async Task<IActionResult> Count(int combatPlayerId)
    {
        try
        {
            var count = await _countService.CountByCombatPlayerIdAsync(combatPlayerId);

            return Ok(count);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error get heal done count by target: {Message}", ex.Message);

            return BadRequest();
        }
    }

    [HttpGet("getByTarget")]
    public async Task<IActionResult> GetByTarget(int combatPlayerId, string target, int page, int pageSize)
    {
        try
        {
            var healDones = await _filterService.GetTargetsByCombatPlayerIdAsync(combatPlayerId, target, page, pageSize);

            return Ok(healDones);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error find heal done by target: {Message}", ex.Message);

            return BadRequest();
        }
    }

    [HttpGet("countByTarget")]
    public async Task<IActionResult> CountByTarget(int combatPlayerId, string target)
    {
        try
        {
            var count = await _filterService.CountTargetsByCombatPlayerIdAsync(combatPlayerId, target);

            return Ok(count);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error get heal done count by target: {Message}", ex.Message);

            return BadRequest();
        }
    }

    [HttpPost]
    public async Task<IActionResult> Create(HealDoneModel model)
    {
        try
        {
            var map = _mapper.Map<HealDoneDto>(model);
            var createdItem = await _mutationService.CreateAsync(map);

            return Ok(createdItem);
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, ex.Message);

            return BadRequest();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);

            return BadRequest();
        }
    }
}
