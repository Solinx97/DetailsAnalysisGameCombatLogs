﻿using AutoMapper;
using CombatAnalysis.BL.DTO;
using CombatAnalysis.BL.Interfaces;
using CombatAnalysis.CombatParserAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace CombatAnalysis.CombatParserAPI.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class ResourceRecoveryController : ControllerBase
{
    private readonly IPlayerInfoService<ResourceRecoveryDto, int> _service;
    private readonly IMapper _mapper;
    private readonly ILogger<ResourceRecoveryController> _logger;

    public ResourceRecoveryController(IPlayerInfoService<ResourceRecoveryDto, int> service, IMapper mapper, ILogger<ResourceRecoveryController> logger)
    {
        _service = service;
        _mapper = mapper;
        _logger = logger;
    }

    [HttpGet("findByCombatPlayerId/{combatPlayerId:int:min(1)}")]
    public async Task<IActionResult> Find(int combatPlayerId)
    {
        var resourceRecoveryes = await _service.GetByCombatPlayerIdAsync(combatPlayerId);

        return Ok(resourceRecoveryes);
    }

    [HttpPost]
    public async Task<IActionResult> Create(ResourceRecoveryModel model)
    {
        try
        {
            var map = _mapper.Map<ResourceRecoveryDto>(model);
            var createdItem = await _service.CreateAsync(map);

            return Ok(_mapper);
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
}
