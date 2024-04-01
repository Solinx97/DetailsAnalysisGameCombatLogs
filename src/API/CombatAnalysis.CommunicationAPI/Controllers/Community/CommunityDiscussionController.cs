using AutoMapper;
using CombatAnalysis.CommunicationBL.DTO.Community;
using CombatAnalysis.CommunicationBL.Interfaces;
using CombatAnalysis.CommunicationAPI.Models.Community;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CombatAnalysis.CommunicationAPI.Controllers.Community;

[Route("api/v1/[controller]")]
[ApiController]
[Authorize]
public class CommunityDiscussionController : ControllerBase
{
    private readonly IService<CommunityDiscussionDto, int> _service;
    private readonly IMapper _mapper;
    private readonly ILogger _logger;

    public CommunityDiscussionController(IService<CommunityDiscussionDto, int> service, IMapper mapper, ILogger logger)
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

    [HttpGet("{id:int:min(1)}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _service.GetByIdAsync(id);

        return Ok(result);
    }

    [HttpGet("findByCommunityId/{id:int:min(1)}")]
    public async Task<IActionResult> FindByCommunityId(int id)
    {
        var result = await _service.GetByParamAsync(nameof(CommunityDiscussionModel.CommunityId), id);

        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CommunityDiscussionModel model)
    {
        try
        {
            var map = _mapper.Map<CommunityDiscussionDto>(model);
            var result = await _service.CreateAsync(map);

            return Ok(result);
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, ex.Message);

            return BadRequest();
        }
    }

    [HttpPut]
    public async Task<IActionResult> Update(CommunityDiscussionModel model)
    {
        try
        {
            var map = _mapper.Map<CommunityDiscussionDto>(model);
            var result = await _service.UpdateAsync(map);

            return Ok(result);
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
            var result = await _service.DeleteAsync(id);

            return Ok(result);
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, ex.Message);

            return BadRequest();
        }
    }
}
