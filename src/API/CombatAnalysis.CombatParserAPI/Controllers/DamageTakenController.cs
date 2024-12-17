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
public class DamageTakenController : ControllerBase
{
    private readonly IMutationService<DamageTakenDto> _mutationService;
    private readonly IPlayerInfoService<DamageTakenDto> _playerInfoService;
    private readonly ICountService<DamageTakenDto> _countService;
    private readonly IGeneralFilterService<DamageTakenDto> _filterService;
    private readonly IMapper _mapper;
    private readonly ILogger<CombatPlayerController> _logger;

    public DamageTakenController(IMutationService<DamageTakenDto> mutationService, IPlayerInfoService<DamageTakenDto> playerInfoService, 
        ICountService<DamageTakenDto> countService, IGeneralFilterService<DamageTakenDto> filterService, 
        IMapper mapper, ILogger<CombatPlayerController> logger)
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
            var damageTakens = await _playerInfoService.GetByCombatPlayerIdAsync(combatPlayerId, page, pageSize);

            return Ok(damageTakens);

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error get damage taken by combat player id: {Message}", ex.Message);

            return BadRequest();
        }
    }

    [HttpGet("getUniqueCreators/{combatPlayerId}")]
    public async Task<IActionResult> GetUniqueCreators(int combatPlayerId)
    {
        try
        {
            var uniqueTargets = await _filterService.GetCreatorNamesByCombatPlayerIdAsync(combatPlayerId);

            return Ok(uniqueTargets);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error get unique damage taken creators: {Message}", ex.Message);

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
            _logger.LogError(ex, "Error get damage taken count by target: {Message}", ex.Message);

            return BadRequest();
        }
    }

    [HttpGet("getByCreator")]
    public async Task<IActionResult> GetByCreator(int combatPlayerId, string creator, int page, int pageSize)
    {
        try
        {
            var damageTakens = await _filterService.GetCreatorByCombatPlayerIdAsync(combatPlayerId, creator, page, pageSize);

            return Ok(damageTakens);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error find damage taken by creator: {Message}", ex.Message);

            return BadRequest();
        }
    }

    [HttpGet("countByCreator")]
    public async Task<IActionResult> CountByCreator(int combatPlayerId, string creator)
    {
        try
        {
            var count = await _filterService.CountCreatorByCombatPlayerIdAsync(combatPlayerId, creator);

            return Ok(count);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error get damage taken count by creator: {Message}", ex.Message);

            return BadRequest();
        }
    }

    [HttpPost]
    public async Task<IActionResult> Create(DamageTakenModel model)
    {
        try
        {
            var map = _mapper.Map<DamageTakenDto>(model);
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
