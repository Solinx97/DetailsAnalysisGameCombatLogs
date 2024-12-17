using AutoMapper;
using CombatAnalysis.BL.DTO;
using CombatAnalysis.BL.Interfaces;
using CombatAnalysis.BL.Interfaces.General;
using CombatAnalysis.CombatParserAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace CombatAnalysis.CombatParserAPI.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class DamageTakenGeneralController : ControllerBase
{
    private readonly IMutationService<DamageTakenGeneralDto> _mutationService;
    private readonly IPlayerInfoService<DamageTakenGeneralDto> _playerInfoService;
    private readonly IMapper _mapper;
    private readonly ILogger<DamageTakenGeneralController> _logger;

    public DamageTakenGeneralController(IMutationService<DamageTakenGeneralDto> mutationService, 
        IPlayerInfoService<DamageTakenGeneralDto> playerInfoService, IMapper mapper,
        ILogger<DamageTakenGeneralController> logger)
    {
        _mutationService = mutationService;
        _playerInfoService = playerInfoService;
        _mapper = mapper;
        _logger = logger;
    }

    [HttpGet("findByCombatPlayerId/{combatPlayerId:int:min(1)}")]
    public async Task<IActionResult> Find(int combatPlayerId)
    {
        var damageTakenGenerals = await _playerInfoService.GetByCombatPlayerIdAsync(combatPlayerId);

        return Ok(damageTakenGenerals);
    }

    [HttpPost]
    public async Task<IActionResult> Create(DamageTakenGeneralModel model)
    {
        try
        {
            var map = _mapper.Map<DamageTakenGeneralDto>(model);
            var createdItem = await _mutationService.CreateAsync(map);

            return Ok(createdItem);
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, ex.Message);

            return BadRequest();
        }
    }
}
