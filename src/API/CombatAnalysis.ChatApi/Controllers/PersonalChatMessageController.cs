using AutoMapper;
using CombatAnalysis.ChatBL.DTO;
using CombatAnalysis.ChatBL.Interfaces;
using CombatAnalysis.ChatApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CombatAnalysis.ChatApi.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
[Authorize]
public class PersonalChatMessageController : ControllerBase
{
    private readonly IService<PersonalChatMessageDto, int> _service;
    private readonly IMapper _mapper;
    private readonly ILogger _logger;

    public PersonalChatMessageController(IService<PersonalChatMessageDto, int> service, IMapper mapper, ILogger logger)
    {
        _service = service;
        _mapper = mapper;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _service.GetAllAsync();
        var map = _mapper.Map<IEnumerable<PersonalChatMessageModel>>(result);

        return Ok(map);
    }

    [HttpGet("{id:int:min(1)}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _service.GetByIdAsync(id);
        var map = _mapper.Map<PersonalChatMessageModel>(result);

        return Ok(map);
    }

    [HttpGet("findByChatId/{chatId:int:min(1)}")]
    public async Task<IActionResult> Find(int chatId)
    {
        var groupChatMessages = await _service.GetByParamAsync("PersonalChatId", chatId);

        return Ok(groupChatMessages);
    }

    [HttpPost]
    public async Task<IActionResult> Create(PersonalChatMessageModel model)
    {
        try
        {
            var map = _mapper.Map<PersonalChatMessageDto>(model);
            var result = await _service.CreateAsync(map);
            var resultMap = _mapper.Map<PersonalChatMessageModel>(result);

            return Ok(resultMap);
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, ex.Message);

            return BadRequest();
        }
    }

    [HttpPut]
    public async Task<IActionResult> Update(PersonalChatMessageModel model)
    {
        try
        {
            var map = _mapper.Map<PersonalChatMessageDto>(model);
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
