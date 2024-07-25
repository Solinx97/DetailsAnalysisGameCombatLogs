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
public class UnreadGroupChatMessageController : ControllerBase
{
    private readonly IService<UnreadGroupChatMessageDto, int> _service;
    private readonly IMapper _mapper;
    private readonly ILogger<PersonalChatMessageCountController> _logger;

    public UnreadGroupChatMessageController(IService<UnreadGroupChatMessageDto, int> service, IMapper mapper, ILogger<PersonalChatMessageCountController> logger)
    {
        _service = service;
        _mapper = mapper;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _service.GetAllAsync();
        var map = _mapper.Map<IEnumerable<UnreadGroupChatMessageModel>>(result);

        return Ok(map);
    }

    [HttpGet("{id:int:min(1)}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _service.GetByIdAsync(id);
        var map = _mapper.Map<UnreadGroupChatMessageModel>(result);

        return Ok(map);
    }

    [HttpGet("findByMessageId/{id:int:min(1)}")]
    public async Task<IActionResult> FindByMessageId(int id)
    {
        var result = await _service.GetByParamAsync("GroupChatMessageId", id);

        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create(UnreadGroupChatMessageModel model)
    {
        try
        {
            var map = _mapper.Map<UnreadGroupChatMessageDto>(model);
            var result = await _service.CreateAsync(map);
            var resultMap = _mapper.Map<UnreadGroupChatMessageModel>(result);

            return Ok(resultMap);
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, $"Create Unread Group Chat Message Count failed: ${ex.Message}", model);

            return BadRequest();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Create Unread Group Chat Message Count failed: ${ex.Message}", model);

            return BadRequest();
        }
    }

    [HttpPut]
    public async Task<IActionResult> Update(UnreadGroupChatMessageModel model)
    {
        try
        {
            var map = _mapper.Map<UnreadGroupChatMessageDto>(model);
            var result = await _service.UpdateAsync(map);

            return Ok(result);
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, $"Update Unread Group Chat Message Count failed: ${ex.Message}", model);

            return BadRequest();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Update Unread Group Chat Message Count failed: ${ex.Message}", model);

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
