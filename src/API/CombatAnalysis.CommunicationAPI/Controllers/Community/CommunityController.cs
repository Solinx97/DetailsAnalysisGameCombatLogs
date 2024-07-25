using AutoMapper;
using CombatAnalysis.CommunicationAPI.Models.Community;
using CombatAnalysis.CommunicationBL.DTO.Community;
using CombatAnalysis.CommunicationBL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CombatAnalysis.CommunicationAPI.Controllers.Community;

[Route("api/v1/[controller]")]
[ApiController]
[Authorize]
public class CommunityController : ControllerBase
{
    private readonly IService<CommunityDto, int> _service;
    private readonly IMapper _mapper;
    private readonly ILogger<CommunityController> _logger;

    public CommunityController(IService<CommunityDto, int> service, IMapper mapper, ILogger<CommunityController> logger)
    {
        _service = service;
        _mapper = mapper;
        _logger = logger;
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetAll()
    {
        var result = await _service.GetAllAsync();

        return Ok(result);
    }

    [HttpGet("{id:int:min(1)}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _service.GetByIdAsync(id);

        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CommunityModel model)
    {
        try
        {
            var map = _mapper.Map<CommunityDto>(model);
            var result = await _service.CreateAsync(map);

            return Ok(result);
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, $"Create Community failed: ${ex.Message}", model);

            return BadRequest();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Create Community failed: ${ex.Message}", model);

            return BadRequest();
        }
    }

    [HttpPut]
    public async Task<IActionResult> Update(CommunityModel model)
    {
        try
        {
            var map = _mapper.Map<CommunityDto>(model);
            var result = await _service.UpdateAsync(map);

            return Ok(result);
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, $"Update Community failed: ${ex.Message}", model);

            return BadRequest();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Update Community failed: ${ex.Message}", model);

            return BadRequest();
        }
    }

    [HttpDelete("{id:int:min(1)}")]
    public async Task<IActionResult> Delete(int id)
    {
        var rowsAffected = await _service.DeleteAsync(id);

        return Ok(rowsAffected);
    }
}
