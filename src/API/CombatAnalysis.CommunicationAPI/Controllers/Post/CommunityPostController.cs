using AutoMapper;
using CombatAnalysis.CommunicationAPI.Models.Post;
using CombatAnalysis.CommunicationBL.DTO.Post;
using CombatAnalysis.CommunicationBL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CombatAnalysis.CommunicationAPI.Controllers.Post;

[Route("api/v1/[controller]")]
[ApiController]
[Authorize]
public class CommunityPostController : ControllerBase
{
    private readonly ICommunityPostService _service;
    private readonly IMapper _mapper;
    private readonly ILogger<CommunityPostController> _logger;

    public CommunityPostController(ICommunityPostService service, IMapper mapper, ILogger<CommunityPostController> logger)
    {
        _service = service;
        _mapper = mapper;
        _logger = logger;
    }

    [HttpGet("count/{communityId}")]
    public async Task<IActionResult> Count(int communityId)
    {
        var count = await _service.CountByCommunityIdAsync(communityId);

        return Ok(count);
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

    [HttpGet("getByCommunityId")]
    public async Task<IActionResult> GetByCommunityId(int communityId, int pageSize)
    {
        var posts = await _service.GetByCommunityIdAsync(communityId, pageSize);

        return Ok(posts);
    }

    [HttpGet("getMoreByCommunityId")]
    public async Task<IActionResult> GetMoreByCommunityId(int communityId, int offset, int pageSize)
    {
        var posts = await _service.GetMoreByCommunityIdAsync(communityId, offset, pageSize);

        return Ok(posts);
    }

    [HttpGet("getNewPosts")]
    public async Task<IActionResult> GetNewPosts(int communityId, string checkFrom)
    {
        var checkFromData = DateTimeOffset.Parse(checkFrom);
        var posts = await _service.GetNewByCommunityIdAsync(communityId, checkFromData);

        return Ok(posts);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CommunityPostModel model)
    {
        try
        {
            var map = _mapper.Map<CommunityPostDto>(model);
            var result = await _service.CreateAsync(map);

            return Ok(result);
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, $"Create Post failed: ${ex.Message}", model);

            return BadRequest();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Create Post failed: ${ex.Message}", model);

            return BadRequest();
        }
    }

    [HttpPut]
    public async Task<IActionResult> Update(CommunityPostModel model)
    {
        try
        {
            var map = _mapper.Map<CommunityPostDto>(model);
            var result = await _service.UpdateAsync(map);

            return Ok(result);
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, $"Update Post failed: ${ex.Message}", model);

            return BadRequest();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Update Post failed: ${ex.Message}", model);

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
