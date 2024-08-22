using AutoMapper;
using CombatAnalysis.ChatApi.Models;
using CombatAnalysis.ChatBL.DTO;
using CombatAnalysis.ChatBL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CombatAnalysis.ChatApi.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
[Authorize]
public class GroupChatUserController : ControllerBase
{
    private readonly IServiceTransaction<GroupChatUserDto, string> _service;
    private readonly IMapper _mapper;
    private readonly ILogger<GroupChatUserController> _logger;

    public GroupChatUserController(IServiceTransaction<GroupChatUserDto, string> service, IMapper mapper, ILogger<GroupChatUserController> logger)
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

    [HttpGet("findByUserId/{id}")]
    public async Task<IActionResult> FindByUserId(string id)
    {
        var result = await _service.GetByParamAsync(nameof(GroupChatUserModel.AppUserId), id);

        return Ok(result);
    }

    [HttpGet("findByChatId/{id:int:min(1)}")]
    public async Task<IActionResult> FindByChatId(int id)
    {
        var result = await _service.GetByParamAsync(nameof(GroupChatUserModel.ChatId), id);

        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create(GroupChatUserModel model)
    {
        try
        {
            model.Id = Guid.NewGuid().ToString();

            var map = _mapper.Map<GroupChatUserDto>(model);
            var result = await _service.CreateAsync(map);

            return Ok(result);
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, $"Create Group Chat User failed: ${ex.Message}", model);

            return BadRequest();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Create Group Chat User failed: ${ex.Message}", model);

            return BadRequest();
        }
    }

    [HttpPut]
    public async Task<IActionResult> Update(GroupChatUserModel model)
    {
        try
        {
            var map = _mapper.Map<GroupChatUserDto>(model);
            var result = await _service.UpdateAsync(map);

            return Ok(result);
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, $"Update Group Chat User failed: ${ex.Message}", model);

            return BadRequest();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Update Group Chat User failed: ${ex.Message}", model);

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
