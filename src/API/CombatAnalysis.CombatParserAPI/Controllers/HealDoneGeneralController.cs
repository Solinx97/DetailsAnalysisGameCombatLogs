using AutoMapper;
using CombatAnalysis.BL.DTO;
using CombatAnalysis.BL.Interfaces;
using CombatAnalysis.BL.Interfaces.General;
using CombatAnalysis.CombatParserAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace CombatAnalysis.CombatParserAPI.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class HealDoneGeneralController : ControllerBase
{
    private readonly IMutationService<HealDoneGeneralDto> _mutationService;
    private readonly IPlayerInfoService<HealDoneGeneralDto> _playerInfoService;
    private readonly IMapper _mapper;
    private readonly ILogger<CombatPlayerController> _logger;

    public HealDoneGeneralController(IMutationService<HealDoneGeneralDto> mutationService, IPlayerInfoService<HealDoneGeneralDto> playerInfoService,
        IMapper mapper, ILogger<CombatPlayerController> logger)
    {
        _mutationService = mutationService;
        _playerInfoService = playerInfoService;
        _mapper = mapper;
        _logger = logger;
    }

    [HttpGet("findByCombatPlayerId/{combatPlayerId:int:min(1)}")]
    public async Task<IActionResult> Find(int combatPlayerId)
    {
        var healDoneGenerals = await _playerInfoService.GetByCombatPlayerIdAsync(combatPlayerId);

        return Ok(healDoneGenerals);
    }

    [HttpPost]
    public async Task<IActionResult> Create(HealDoneGeneralModel model)
    {
        try
        {
            var map = _mapper.Map<HealDoneGeneralDto>(model);
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
