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
public class GroupChatRulesController : ControllerBase
{
    private readonly IService<GroupChatRulesDto, int> _service;
    private readonly IMapper _mapper;
    private readonly ILogger<GroupChatMessageCountController> _logger;

    public GroupChatRulesController(IService<GroupChatRulesDto, int> service, IMapper mapper, ILogger<GroupChatMessageCountController> logger)
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
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _service.GetByIdAsync(id);

        return Ok(result);
    }

    [HttpGet("findByChatId/{id:int:min(1)}")]
    public async Task<IActionResult> FindByChatId(int id)
    {
        var result = await _service.GetByParamAsync(nameof(GroupChatRulesModel.GroupChatId), id);

        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create(GroupChatRulesModel model)
    {
        try
        {
            var map = _mapper.Map<GroupChatRulesDto>(model);
            var result = await _service.CreateAsync(map);

            return Ok(result);
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, $"Create Group Chat Rules failed: ${ex.Message}", model);

            return BadRequest();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Create Group Chat Rules failed: ${ex.Message}", model);

            return BadRequest();
        }
    }

    [HttpPut]
    public async Task<IActionResult> Update(GroupChatRulesModel model)
    {
        try
        {
            var map = _mapper.Map<GroupChatRulesDto>(model);
            var result = await _service.UpdateAsync(map);

            return Ok(result);
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, $"Update Group Chat Rules failed: ${ex.Message}", model);

            return BadRequest();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Update Group Chat Rules failed: ${ex.Message}", model);

            return BadRequest();
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var rowsAffected = await _service.DeleteAsync(id);

        return Ok(rowsAffected);
    }
}
