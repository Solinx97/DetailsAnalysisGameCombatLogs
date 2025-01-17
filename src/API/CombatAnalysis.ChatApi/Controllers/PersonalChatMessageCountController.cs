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
public class PersonalChatMessageCountController : ControllerBase
{
    private readonly IService<PersonalChatMessageCountDto, int> _messageCountService;
    private readonly IMapper _mapper;
    private readonly ILogger<PersonalChatMessageCountController> _logger;

    public PersonalChatMessageCountController(IService<PersonalChatMessageCountDto, int> messageCountService, IMapper mapper, ILogger<PersonalChatMessageCountController> logger)
    {
        _messageCountService = messageCountService;
        _mapper = mapper;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _messageCountService.GetAllAsync();
        var map = _mapper.Map<IEnumerable<PersonalChatMessageCountModel>>(result);

        return Ok(map);
    }

    [HttpGet("{id:int:min(1)}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _messageCountService.GetByIdAsync(id);
        var map = _mapper.Map<PersonalChatMessageCountModel>(result);

        return Ok(map);
    }

    [HttpGet("findMe")]
    public async Task<IActionResult> FindMe(int chatId, string appUserId)
    {
        var myMessagesCount = await _messageCountService.GetByParamAsync(nameof(PersonalChatMessageCountModel.ChatId), chatId);
        var myMessageCount = myMessagesCount.FirstOrDefault(x => x.AppUserId == appUserId);

        return Ok(myMessageCount);
    }                                                                                                                                                   

    [HttpPost]
    public async Task<IActionResult> Create(PersonalChatMessageCountModel model)
    {
        try
        {
            var map = _mapper.Map<PersonalChatMessageCountDto>(model);
            var result = await _messageCountService.CreateAsync(map);

            return Ok(result);
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, $"Create Personal Chat Message Count failed: ${ex.Message}", model);

            return BadRequest();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Create Personal Chat Message Count failed: ${ex.Message}", model);

            return BadRequest();
        }
    }

    [HttpPut]
    public async Task<IActionResult> Update(PersonalChatMessageCountModel model)
    {
        try
        {
            var map = _mapper.Map<PersonalChatMessageCountDto>(model);
            var result = await _messageCountService.UpdateAsync(map);

            return Ok(result);
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, $"Update Personal Chat Message Count failed: ${ex.Message}", model);

            return BadRequest();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Update Personal Chat Message Count failed: ${ex.Message}", model);

            return BadRequest();
        }
    }

    [HttpDelete("{id:int:min(1)}")]
    public async Task<IActionResult> Delete(int id)
    {
        var rowsAffected = await _messageCountService.DeleteAsync(id);

        return Ok(rowsAffected);
    }
}
