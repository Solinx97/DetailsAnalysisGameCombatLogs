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
public class CommunityDiscussionCommentController : ControllerBase
{
    private readonly IService<CommunityDiscussionCommentDto, int> _service;
    private readonly IMapper _mapper;
    private readonly ILogger<CommunityDiscussionCommentController> _logger;

    public CommunityDiscussionCommentController(IService<CommunityDiscussionCommentDto, int> service, IMapper mapper, ILogger<CommunityDiscussionCommentController> logger)
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

    [HttpGet("findByDiscussionId/{id:int:min(1)}")]
    public async Task<IActionResult> FindByDiscussionId(int id)
    {
        var result = await _service.GetByParamAsync(nameof(CommunityDiscussionCommentModel.CommunityDiscussionId), id);

        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CommunityDiscussionCommentModel model)
    {
        try
        {
            var map = _mapper.Map<CommunityDiscussionCommentDto>(model);
            var result = await _service.CreateAsync(map);

            return Ok(result);
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, $"Create Community Discussion Comment failed: ${ex.Message}", model);

            return BadRequest();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Create Community Discussion Comment failed: ${ex.Message}", model);

            return BadRequest();
        }
    }

    [HttpPut]
    public async Task<IActionResult> Update(CommunityDiscussionCommentModel model)
    {
        try
        {
            var map = _mapper.Map<CommunityDiscussionCommentDto>(model);
            var result = await _service.UpdateAsync(map);

            return Ok(result);
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, $"Update Community Discussion Comment failed: ${ex.Message}", model);

            return BadRequest();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Update Community Discussion Comment failed: ${ex.Message}", model);

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
