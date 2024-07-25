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
public class CommunityUserController : ControllerBase
{
    private readonly IService<CommunityUserDto, string> _service;
    private readonly IMapper _mapper;
    private readonly ILogger<CommunityUserController> _logger;

    public CommunityUserController(IService<CommunityUserDto, string> service, IMapper mapper, ILogger<CommunityUserController> logger)
    {
        _service = service;
        _mapper = mapper;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _service.GetAllAsync();

        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(string id)
    {
        var result = await _service.GetByIdAsync(id);

        return Ok(result);
    }

    [HttpGet("searchByCommunityId/{id:int:min(1)}")]
    public async Task<IActionResult> SearchByCommunityId(int id)
    {
        var result = await _service.GetByParamAsync(nameof(CommunityUserModel.CommunityId), id);

        return Ok(result);
    }

    [HttpGet("searchByUserId/{id}")]
    public async Task<IActionResult> SearchByUserId(string id)
    {
        var result = await _service.GetByParamAsync(nameof(CommunityUserModel.AppUserId), id);

        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CommunityUserModel model)
    {
        try
        {
            model.Id = Guid.NewGuid().ToString();

            var map = _mapper.Map<CommunityUserDto>(model);
            var result = await _service.CreateAsync(map);

            return Ok(result);
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, $"Create Community User failed: ${ex.Message}", model);

            return BadRequest();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Create Community User failed: ${ex.Message}", model);

            return BadRequest();
        }
    }

    [HttpPut]
    public async Task<IActionResult> Update(CommunityUserModel model)
    {
        try
        {
            var map = _mapper.Map<CommunityUserDto>(model);
            var result = await _service.UpdateAsync(map);

            return Ok(result);
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, $"Update Community User failed: ${ex.Message}", model);

            return BadRequest();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Update Community User failed: ${ex.Message}", model);

            return BadRequest();
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        var rowsAffected = await _service.DeleteAsync(id);

        return Ok(rowsAffected);
    }
}
