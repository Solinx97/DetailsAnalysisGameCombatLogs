using AutoMapper;
using CombatAnalysis.BL.Interfaces;
using CombatAnalysis.BL.Interfaces.General;
using CombatAnalysis.CombatParserAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace CombatAnalysis.CombatParserAPI.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class PlayerDeathController : ControllerBase
{
    private readonly IMutationService<PlayerDeathDto> _mutationService;
    private readonly IPlayerInfoService<PlayerDeathDto> _playerInfoService;
    private readonly IMapper _mapper;
    private readonly ILogger<PlayerDeathController> _logger;

    public PlayerDeathController(IMutationService<PlayerDeathDto> mutationService, IPlayerInfoService<PlayerDeathDto> service,
        IMapper mapper, ILogger<PlayerDeathController> logger)
    {
        _mutationService = mutationService;
        _playerInfoService = service;
        _mapper = mapper;
        _logger = logger;
    }

    [HttpGet("getByCombatPlayerId/{combatPlayerId:int:min(1)}")]
    public async Task<IActionResult> Find(int combatPlayerId)
    {
        var damageTakens = await _playerInfoService.GetByCombatPlayerIdAsync(combatPlayerId);

        return Ok(damageTakens);
    }

    [HttpPost]
    public async Task<IActionResult> Create(PlayerDeathModel model)
    {
        try
        {
            var map = _mapper.Map<PlayerDeathDto>(model);
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
