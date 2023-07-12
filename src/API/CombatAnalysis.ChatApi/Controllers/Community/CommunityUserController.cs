using AutoMapper;
using CombatAnalysis.BL.DTO.Community;
using CombatAnalysis.BL.Interfaces;
using CombatAnalysis.ChatApi.Models.Community;
using Microsoft.AspNetCore.Mvc;

namespace CombatAnalysis.ChatApi.Controllers.Community;

[Route("api/v1/[controller]")]
[ApiController]
public class CommunityUserController : ControllerBase
{
    private readonly IService<CommunityUserDto, int> _service;
    private readonly IMapper _mapper;
    private readonly ILogger _logger;
    private readonly ISqlContextService _sqlContextService;

    public CommunityUserController(IService<CommunityUserDto, int> service, IMapper mapper, ISqlContextService sqlContextService, ILogger logger)
    {
        _service = service;
        _mapper = mapper;
        _sqlContextService = sqlContextService;
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

    [HttpGet("searchByCommunityId/{id:int:min(1)}")]
    public async Task<IActionResult> SearchByCommunityId(int id)
    {
        var result = await _service.GetByParamAsync(nameof(CommunityUserModel.CommunityId), id);

        return Ok(result);
    }

    [HttpGet("searchByUserId/{id}")]
    public async Task<IActionResult> SearchByUserId(string id)
    {
        var result = await _service.GetByParamAsync(nameof(CommunityUserModel.CustomerId), id);

        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CommunityUserModel model)
    {
        try
        {
            var map = _mapper.Map<CommunityUserDto>(model);
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
    public async Task<IActionResult> Update(CommunityUserModel model)
    {
        try
        {
            var map = _mapper.Map<CommunityUserDto>(model);
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

    [HttpDelete]
    public async Task<IActionResult> Delete(List<int> usersId)
    {
        using var transaction = await _sqlContextService.BeginTransactionAsync();
        try
        {
            foreach (var item in usersId)
            {
                var result = await _service.DeleteAsync(item);
            }

            await transaction.CommitAsync();

            return Ok();
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, ex.Message);

            await transaction.RollbackAsync();

            return BadRequest();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);

            await transaction.RollbackAsync();

            return BadRequest();
        }
    }
}
