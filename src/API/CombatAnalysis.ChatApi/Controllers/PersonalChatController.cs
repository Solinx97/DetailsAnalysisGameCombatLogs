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
public class PersonalChatController : ControllerBase
{
    private readonly IService<PersonalChatDto, int> _service;
    private readonly IMapper _mapper;
    private readonly ILogger<PersonalChatController> _logger;

    public PersonalChatController(IService<PersonalChatDto, int> service, IMapper mapper, ILogger<PersonalChatController> logger)
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

    [HttpPost]
    public async Task<IActionResult> Create(PersonalChatModel model)
    {
        try
        {
            var map = _mapper.Map<PersonalChatDto>(model);
            var result = await _service.CreateAsync(map);

            return Ok(result);
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, $"Create Personal Chat failed: ${ex.Message}", model);

            return BadRequest();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Create Personal Chat failed: ${ex.Message}", model);

            return BadRequest();
        }
    }

    [HttpPost("personalChatIsAlreadyExists")]
    public async Task<IActionResult> PersonalChatCheck(PersonalChatModel model)
    {
        var allData = await _service.GetAllAsync();
        foreach (var item in allData)
        {
            if ((item.InitiatorId == model.InitiatorId || item.InitiatorId == model.CompanionId)
                || (item.CompanionId == model.InitiatorId || item.CompanionId == model.CompanionId))
            {
                return Ok(item.Id);
            }
        }

        return Ok(0);
    }

    [HttpPut]
    public async Task<IActionResult> Update(PersonalChatModel model)
    {
        try
        {
            var map = _mapper.Map<PersonalChatDto>(model);
            var result = await _service.UpdateAsync(map);

            return Ok(result);
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, $"Update Personal Chat failed: ${ex.Message}", model);

            return BadRequest();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Update Personal Chat failed: ${ex.Message}", model);

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
