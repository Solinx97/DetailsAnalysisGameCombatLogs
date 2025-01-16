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
public class GroupChatMessageCountController : ControllerBase
{
    private readonly IService<GroupChatMessageCountDto, int> _groupChatMessageCountService;
    private readonly IMapper _mapper;
    private readonly ILogger<GroupChatMessageCountController> _logger;

    public GroupChatMessageCountController(IService<GroupChatMessageCountDto, int> groupChatMessageCountService, IMapper mapper, ILogger<GroupChatMessageCountController> logger)
    {
        _groupChatMessageCountService = groupChatMessageCountService;
        _mapper = mapper;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _groupChatMessageCountService.GetAllAsync();
        var map = _mapper.Map<IEnumerable<GroupChatMessageCountModel>>(result);

        return Ok(map);
    }

    [HttpGet("{id:int:min(1)}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _groupChatMessageCountService.GetByIdAsync(id);
        var map = _mapper.Map<GroupChatMessageCountModel>(result);

        return Ok(map);
    }

    [HttpGet("findMe/{meInChatId}")]
    public async Task<IActionResult> FindMe(string meInChatId)
    {
        var myMessageCounts = await _groupChatMessageCountService.GetByParamAsync(nameof(GroupChatMessageCountModel.GroupChatUserId), meInChatId);
        var myMessageCount = myMessageCounts.FirstOrDefault();

        return Ok(myMessageCount);
    }

    [HttpPost]
    public async Task<IActionResult> Create(GroupChatMessageCountModel model)
    {
        try
        {
            var map = _mapper.Map<GroupChatMessageCountDto>(model);
            var result = await _groupChatMessageCountService.CreateAsync(map);

            return Ok(result);
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, $"Create Group Chat Message Count failed: ${ex.Message}", model);

            return BadRequest();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Create Group Chat Message Count failed: ${ex.Message}", model);

            return BadRequest();
        }
    }

    [HttpPut]
    public async Task<IActionResult> Update(GroupChatMessageCountModel model)
    {
        try
        {
            var map = _mapper.Map<GroupChatMessageCountDto>(model);
            var result = await _groupChatMessageCountService.UpdateAsync(map);

            return Ok(result);
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, $"Update Group Chat Message Count failed: ${ex.Message}", model);

            return BadRequest();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Update Group Chat Message Count failed: ${ex.Message}", model);

            return BadRequest();
        }
    }

    [HttpDelete("{id:int:min(1)}")]
    public async Task<IActionResult> Delete(int id)
    {
        var rowsAffected = await _groupChatMessageCountService.DeleteAsync(id);

        return Ok(rowsAffected);
    }
}
