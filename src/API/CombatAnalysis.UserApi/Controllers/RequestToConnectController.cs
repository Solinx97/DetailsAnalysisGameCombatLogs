using AutoMapper;
using CombatAnalysis.CustomerBL.DTO;
using CombatAnalysis.CustomerBL.Interfaces;
using CombatAnalysis.UserApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace CombatAnalysis.UserApi.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class RequestToConnectController : ControllerBase
{
    private readonly IService<RequestToConnectDto, int> _service;
    private readonly IMapper _mapper;
    private readonly ILogger _logger;

    public RequestToConnectController(IService<RequestToConnectDto, int> service, IMapper mapper, ILogger logger)
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
        var result = await _service.GetByParamAsync(nameof(RequestToConnectModel.OwnerId), id);

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
            _logger.LogError(ex, ex.Message);

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
