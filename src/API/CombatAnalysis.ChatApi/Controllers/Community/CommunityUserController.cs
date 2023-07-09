using AutoMapper;
using CombatAnalysis.BL.DTO.Community;
using CombatAnalysis.BL.Interfaces;
using CombatAnalysis.ChatApi.Models.Community;
using Microsoft.AspNetCore.Mvc;

namespace CombatAnalysis.ChatApi.Controllers.Community;

[Route("api/v1/[controller]")]
[ApiController]
public class CommunityUserController : ControllerBase
{
    private readonly IService<CommunityUserDto, int> _service;
    private readonly IMapper _mapper;
    private readonly ILogger _logger;

    public CommunityUserController(IService<CommunityUserDto, int> service, IMapper mapper, ILogger logger)
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

    [HttpGet("findByUserId/{userId}")]
    public async Task<IActionResult> FindByUserId(string userId)
    {
        var result = await _service.GetByParamAsync("UserId", userId);

        return Ok(result);
    }

    [HttpGet("findByChatId/{groupId:int:min(1)}")]
    public async Task<IActionResult> FindByGroupId(int groupId)
    {
        var result = await _service.GetByParamAsync("GroupChatId", groupId);

        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CommunityUserModel model)
    {
        try
        {
            var map = _mapper.Map<CommunityUserDto>(model);
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
