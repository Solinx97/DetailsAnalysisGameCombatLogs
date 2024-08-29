using AutoMapper;
using CombatAnalysis.CommunicationAPI.Models.Post;
using CombatAnalysis.CommunicationBL.DTO.Post;
using CombatAnalysis.CommunicationBL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CombatAnalysis.CommunicationAPI.Controllers.Post;

[Route("api/v1/[controller]")]
[ApiController]
[Authorize]
public class UserPostController : ControllerBase
{
    private readonly IUserPostService _service;
    private readonly IMapper _mapper;
    private readonly ILogger<UserPostController> _logger;

    public UserPostController(IUserPostService service, IMapper mapper, ILogger<UserPostController> logger)
    {
        _service = service;
        _mapper = mapper;
        _logger = logger;
    }

    [HttpGet("count/{appUserId}")]
    public async Task<IActionResult> Count(string appUserId)
    {
        var count = await _service.CountByAppUserIdAsync(appUserId);

        return Ok(count);
    }

    [HttpGet("countByListOfAppUsers/{appUserIds}")]
    public async Task<IActionResult> CountByListOfAppUsers(string appUserIds)
    {
        var appUserIdList = appUserIds.Split(',');
        var count = await _service.CountByListOfAppUserIdAsync(appUserIdList);

        return Ok(count);
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

    [HttpGet("getByUserId")]
    public async Task<IActionResult> GetByUserId(string appUserId, int pageSize)
    {
        var posts = await _service.GetByAppUserIdAsync(appUserId, pageSize);

        return Ok(posts);
    }

    [HttpGet("getMoreByUserId")]
    public async Task<IActionResult> GetMoreByUserId(string appUserId, int offset, int pageSize)
    {
        var posts = await _service.GetMoreByAppUserIdAsync(appUserId, offset, pageSize);

        return Ok(posts);
    }

    [HttpGet("getNewPosts")]
    public async Task<IActionResult> GetNewPosts(string appUserId, string checkFrom)
    {
        var checkFromData = DateTimeOffset.Parse(checkFrom);
        var posts = await _service.GetNewByAppUserIdAsync(appUserId, checkFromData);

        return Ok(posts);
    }

    [HttpGet("getByListOfUserIds")]
    public async Task<IActionResult> GetByListOfUserIds(string appUserIds, int pageSize)
    {
        var posts = await _service.GetByListOfAppUserIdAsync(appUserIds, pageSize);

        return Ok(posts);
    }

    [HttpGet("getMoreByListOfUserIds")]
    public async Task<IActionResult> GetMoreByListOfUserIds(string appUserIds, int offset, int pageSize)
    {
        var posts = await _service.GetMoreByListOfAppUserIdAsync(appUserIds, offset, pageSize);

        return Ok(posts);
    }

    [HttpGet("getNewByListOfUserIds")]
    public async Task<IActionResult> GetNewByListOfUserIds(string appUserIds, string checkFrom)
    {
        var checkFromData = DateTimeOffset.Parse(checkFrom);
        var posts = await _service.GetNewByListOfAppUserIdAsync(appUserIds, checkFromData);

        return Ok(posts);
    }

    [HttpPost]
    public async Task<IActionResult> Create(UserPostModel model)
    {
        try
        {
            var map = _mapper.Map<UserPostDto>(model);
            var result = await _service.CreateAsync(map);

            return Ok(result);
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, $"Create Post failed: ${ex.Message}", model);

            return BadRequest();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Create Post failed: ${ex.Message}", model);

            return BadRequest();
        }
    }

    [HttpPut]
    public async Task<IActionResult> Update(UserPostModel model)
    {
        try
        {
            var map = _mapper.Map<UserPostDto>(model);
            var result = await _service.UpdateAsync(map);

            return Ok(result);
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, $"Update Post failed: ${ex.Message}", model);

            return BadRequest();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Update Post failed: ${ex.Message}", model);

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