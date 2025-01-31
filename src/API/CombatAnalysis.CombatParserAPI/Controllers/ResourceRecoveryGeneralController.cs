using AutoMapper;
using CombatAnalysis.BL.DTO;
using CombatAnalysis.BL.Interfaces;
using CombatAnalysis.BL.Interfaces.General;
using CombatAnalysis.CombatParserAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace CombatAnalysis.CombatParserAPI.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class ResourceRecoveryGeneralController : ControllerBase
{
    private readonly IMutationService<ResourceRecoveryGeneralDto> _mutationService;
    private readonly IPlayerInfoService<ResourceRecoveryGeneralDto> _playerInfoService;
    private readonly IMapper _mapper;
    private readonly ILogger<ResourceRecoveryGeneralController> _logger;

    public ResourceRecoveryGeneralController(IMutationService<ResourceRecoveryGeneralDto> mutationService, IPlayerInfoService<ResourceRecoveryGeneralDto> playerInfoService,
        IMapper mapper, ILogger<ResourceRecoveryGeneralController> logger)
    {
        _mutationService = mutationService;
        _playerInfoService = playerInfoService;
        _mapper = mapper;
        _logger = logger;
    }

    [HttpGet("getByCombatPlayerId/{combatPlayerId:int:min(1)}")]
    public async Task<IActionResult> GetByCombatPlayerId(int combatPlayerId)
    {
        var resourceRecoveryGenerals = await _playerInfoService.GetByCombatPlayerIdAsync(combatPlayerId);

        return Ok(resourceRecoveryGenerals);
    }

    [HttpPost]
    public async Task<IActionResult> Create(ResourceRecoveryGeneralModel model)
    {
        try
        {
            var map = _mapper.Map<ResourceRecoveryGeneralDto>(model);
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
