using AutoMapper;
using CombatAnalysis.CustomerBL.DTO;
using CombatAnalysis.CustomerBL.Interfaces;
using CombatAnalysis.UserApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CombatAnalysis.UserApi.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
[Authorize]
public class FriendController : ControllerBase
{
    private readonly IService<FriendDto, int> _service;
    private readonly IMapper _mapper;
    private readonly ILogger<FriendController> _logger;

    public FriendController(IService<FriendDto, int> service, IMapper mapper, ILogger<FriendController> logger)
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

    [HttpGet("searchByUserId/{id}")]
    public async Task<IActionResult> SearchByUserId(string id)
    {
        var forWhomId = await _service.GetByParamAsync(nameof(FriendModel.ForWhomId), id);
        var whoFriendId = await _service.GetByParamAsync(nameof(FriendModel.WhoFriendId), id);
        var friends = forWhomId.Concat(whoFriendId);

        return Ok(friends);
    }

    [HttpPost]
    public async Task<IActionResult> Create(FriendModel model)
    {
        try
        {
            var map = _mapper.Map<FriendDto>(model);
            var result = await _service.CreateAsync(map);

            return Ok(result);
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, $"Create Friend failed: ${ex.Message}", model);

            return BadRequest();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Create Friend failed: ${ex.Message}", model);

            return BadRequest();
        }
    }

    [HttpPut]
    public async Task<IActionResult> Update(CustomerModel model)
    {
        try
        {
            var map = _mapper.Map<FriendDto>(model);
            var result = await _service.UpdateAsync(map);

            return Ok(result);
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, $"Update Friend failed: ${ex.Message}", model);

            return BadRequest();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Update Friend failed: ${ex.Message}", model);

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

