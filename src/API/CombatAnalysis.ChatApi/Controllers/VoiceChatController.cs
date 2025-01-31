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
public class VoiceChatController : ControllerBase
{
    private readonly IService<VoiceChatDto, string> _service;
    private readonly IMapper _mapper;
    private readonly ILogger<VoiceChatController> _logger;

    public VoiceChatController(IService<VoiceChatDto, string> service, IMapper mapper, ILogger<VoiceChatController> logger)
    {
        _service = service;
        _mapper = mapper;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _service.GetAllAsync();
        var map = _mapper.Map<IEnumerable<VoiceChatModel>>(result);

        return Ok(map);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(string id)
    {
        var result = await _service.GetByIdAsync(id);
        var map = _mapper.Map<VoiceChatModel>(result);

        return Ok(map);
    }

    [HttpPost]
    public async Task<IActionResult> Create(VoiceChatModel model)
    {
        try
        {
            var map = _mapper.Map<VoiceChatDto>(model);
            var result = await _service.CreateAsync(map);
            var resultMap = _mapper.Map<VoiceChatModel>(result);

            return Ok(resultMap);
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, $"Create Voice Chat failed: ${ex.Message}", model);

            return BadRequest();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Create Voice Chat failed: ${ex.Message}", model);

            return BadRequest();
        }
    }

    [HttpPut]
    public async Task<IActionResult> Update(VoiceChatModel model)
    {
        try
        {
            var map = _mapper.Map<VoiceChatDto>(model);
            var result = await _service.UpdateAsync(map);

            return Ok(result);
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, $"Update Voice Chat failed: ${ex.Message}", model);

            return BadRequest();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Update Voice Chat failed: ${ex.Message}", model);

            return BadRequest();
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        var affectedRows = await _service.DeleteAsync(id);

        return Ok(affectedRows);
    }
}
