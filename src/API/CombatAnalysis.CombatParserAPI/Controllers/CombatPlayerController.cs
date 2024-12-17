using AutoMapper;
using CombatAnalysis.BL.DTO;
using CombatAnalysis.BL.Interfaces.General;
using CombatAnalysis.CombatParserAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace CombatAnalysis.CombatParserAPI.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class CombatPlayerController : ControllerBase
{
    private readonly IQueryService<CombatPlayerDto> _queryCombatPlayerService;
    private readonly IMutationService<CombatPlayerDto> _mutationCombatPlayerService;
    private readonly IMapper _mapper;
    private readonly ILogger<CombatPlayerController> _logger;

    public CombatPlayerController(IQueryService<CombatPlayerDto> queryCombatPlayerService, IMutationService<CombatPlayerDto> mutationCombatPlayerService, 
        IMapper mapper, ILogger<CombatPlayerController> logger)
    {
        _queryCombatPlayerService = queryCombatPlayerService;
        _mutationCombatPlayerService = mutationCombatPlayerService;
        _mapper = mapper;
        _logger = logger;
    }

    [HttpGet("findByCombatId/{combatId:int:min(1)}")]
    public async Task<IActionResult> Find(int combatId)
    {
        var players = await _queryCombatPlayerService.GetByParamAsync(nameof(CombatPlayerModel.CombatId), combatId);

        return Ok(players);
    }

    [HttpGet("{id:int:min(1)}")]
    public async Task<IActionResult> GetById(int id)
    {
        var player = await _queryCombatPlayerService.GetByIdAsync(id);

        return Ok(player);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CombatPlayerModel model)
    {
        try
        {
            var map = _mapper.Map<CombatPlayerDto>(model);
            var createdItem = await _mutationCombatPlayerService.CreateAsync(map);

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

    [HttpDelete("{id:int:min(1)}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var item = await GetById(id);
            var map = _mapper.Map<CombatPlayerDto>(item);

            var deletedId = await _mutationCombatPlayerService.DeleteAsync(map);

            return Ok(deletedId);
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, ex.Message);

            return BadRequest();
        }
    }
}
