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
public class UserPostDislikeController : ControllerBase
{
    private readonly IService<UserPostDislikeDto, int> _service;
    private readonly IMapper _mapper;
    private readonly ILogger<UserPostDislikeController> _logger;

    public UserPostDislikeController(IService<UserPostDislikeDto, int> service, IMapper mapper, ILogger<UserPostDislikeController> logger)
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

    [HttpGet("searchByPostId/{id:int:min(1)}")]
    public async Task<IActionResult> SearchByPostId(int id)
    {
        var result = await _service.GetByParamAsync(nameof(UserPostDislikeModel.UserPostId), id);

        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create(UserPostDislikeModel model)
    {
        try
        {
            var map = _mapper.Map<UserPostDislikeDto>(model);
            var result = await _service.CreateAsync(map);

            return Ok(result);
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, $"Create Post Dislike failed: ${ex.Message}", model);

            return BadRequest();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Create Post Dislike failed: ${ex.Message}", model);

            return BadRequest();
        }
    }

    [HttpPut]
    public async Task<IActionResult> Update(UserPostDislikeModel model)
    {
        try
        {
            var map = _mapper.Map<UserPostDislikeDto>(model);
            var result = await _service.UpdateAsync(map);

            return Ok(result);
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, $"Update Post Dislike failed: ${ex.Message}", model);

            return BadRequest();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Update Post Dislike failed: ${ex.Message}", model);

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
