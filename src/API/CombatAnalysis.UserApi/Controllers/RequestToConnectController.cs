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
public class RequestToConnectController : ControllerBase
{
    private readonly IService<RequestToConnectDto, int> _service;
    private readonly IMapper _mapper;
    private readonly ILogger<RequestToConnectController> _logger;

    public RequestToConnectController(IService<RequestToConnectDto, int> service, IMapper mapper, ILogger<RequestToConnectController> logger)
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

    [HttpGet("searchByOwnerId/{id}")]
    public async Task<IActionResult> SearchByOwnerId(string id)
    {
        var result = await _service.GetByParamAsync(nameof(RequestToConnectModel.CustomerId), id);

        return Ok(result);
    }

    [HttpGet("searchByToUserId/{id}")]
    public async Task<IActionResult> SearchByToUserId(string id)
    {
        var result = await _service.GetByParamAsync(nameof(RequestToConnectModel.ToUserId), id);

        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create(RequestToConnectModel model)
    {
        try
        {
            var map = _mapper.Map<RequestToConnectDto>(model);
            var result = await _service.CreateAsync(map);

            return Ok(result);
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, $"Create Request to Connect failed: ${ex.Message}", model);

            return BadRequest();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Create Request to Connect failed: ${ex.Message}", model);

            return BadRequest();
        }
    }

    [HttpPut]
    public async Task<IActionResult> Update(RequestToConnectModel model)
    {
        try
        {
            var map = _mapper.Map<RequestToConnectDto>(model);
            var result = await _service.UpdateAsync(map);

            return Ok(result);
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, $"Update Request to Connect failed: ${ex.Message}", model);

            return BadRequest();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Update Request to Connect failed: ${ex.Message}", model);

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
