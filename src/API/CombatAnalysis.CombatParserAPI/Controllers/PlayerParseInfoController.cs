using AutoMapper;
using CombatAnalysis.BL.DTO;
using CombatAnalysis.BL.Interfaces.General;
using CombatAnalysis.CombatParserAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace CombatAnalysis.CombatParserAPI.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class PlayerParseInfoController : ControllerBase
{
    private readonly IQueryService<PlayerParseInfoDto> _queryService;
    private readonly IMutationService<PlayerParseInfoDto> _mutationService;
    private readonly IMapper _mapper;
    private readonly ILogger<PlayerParseInfoController> _logger;

    public PlayerParseInfoController(IQueryService<PlayerParseInfoDto> queryService, IMutationService<PlayerParseInfoDto> mutationService,
        IMapper mapper, ILogger<PlayerParseInfoController> logger)
    {
        _queryService = queryService;
        _mutationService = mutationService;
        _mapper = mapper;
        _logger = logger;
    }

    [HttpGet("getByCombatPlayerId/{combatPlayerId:int:min(1)}")]
    public async Task<IActionResult> GetByCombatPlayerId(int combatPlayerId)
    {
        try
        {
            var playerParseInfo = await _queryService.GetByParamAsync(nameof(PlayerParseInfoModel.CombatPlayerId), combatPlayerId);

            return Ok(playerParseInfo);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error get player parse info by combat player id: {Message}", ex.Message);

            return BadRequest();
        }
    }

    [HttpGet("{id:int:min(1)}")]
    public async Task<IActionResult> GetById(int id)
    {
        try
        {
            var playerParseInfo = await _queryService.GetByIdAsync(id);

            return Ok(playerParseInfo);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error get player parse info by id: {Message}", ex.Message);

            return BadRequest();
        }
    }

    [HttpPost]
    public async Task<IActionResult> Create(PlayerParseInfoModel model)
    {
        try
        {
            var map = _mapper.Map<PlayerParseInfoDto>(model);
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
