using AutoMapper;
using CombatAnalysis.BL.DTO.Chat;
using CombatAnalysis.BL.Interfaces;
using CombatAnalysis.ChatApi.Models;
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
    private readonly ILogger _logger;

    public UnreadGroupChatMessageController(IService<UnreadGroupChatMessageDto, int> service, IMapper mapper, ILogger logger)
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
            _logger.LogError(ex, ex.Message);

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
