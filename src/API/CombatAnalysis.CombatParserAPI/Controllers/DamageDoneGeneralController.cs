using AutoMapper;
using CombatAnalysis.BL.DTO;
using CombatAnalysis.BL.Interfaces;
using CombatAnalysis.BL.Interfaces.General;
using CombatAnalysis.CombatParserAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace CombatAnalysis.CombatParserAPI.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class DamageDoneGeneralController : ControllerBase
{
    private readonly IMutationService<DamageDoneGeneralDto> _mutationService;
    private readonly IPlayerInfoService<DamageDoneGeneralDto> _playerInfoService;
    private readonly IMapper _mapper;
    private readonly ILogger<DamageDoneGeneralController> _logger;

    public DamageDoneGeneralController(IMutationService<DamageDoneGeneralDto> mutationService, IPlayerInfoService<DamageDoneGeneralDto> playerInfoService,
        IMapper mapper, ILogger<DamageDoneGeneralController> logger)
    {
        _mutationService = mutationService;
        _playerInfoService = playerInfoService;
        _mapper = mapper;
        _logger = logger;
    }

    [HttpGet("findByCombatPlayerId/{combatPlayerId:int:min(1)}")]
    public async Task<IActionResult> Find(int combatPlayerId)
    {
        var damageDoneGenerals = await _playerInfoService.GetByCombatPlayerIdAsync(combatPlayerId);

        return Ok(damageDoneGenerals);
    }

    [HttpPost]
    public async Task<IActionResult> Create(DamageDoneGeneralModel model)
    {
        try
        {
            var map = _mapper.Map<DamageDoneGeneralDto>(model);
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
