//using AutoMapper;
//using CombatAnalysis.CommunicationAPI.Models.Post;
//using CombatAnalysis.CommunicationBL.DTO.Post;
//using CombatAnalysis.CommunicationBL.Interfaces;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;

//namespace CombatAnalysis.CommunicationAPI.Controllers.Post;

//[Route("api/v1/[controller]")]
//[ApiController]
//[Authorize]
//public class UserPostController(IUserPostService service, IMapper mapper, ILogger<UserPostController> logger) : ControllerBase
//{
//    private readonly IUserPostService _service = service;
//    private readonly IMapper _mapper = mapper;
//    private readonly ILogger<UserPostController> _logger = logger;

//    [HttpGet("count/{appUserId}")]
//    public async Task<IActionResult> Count(string appUserId)
//    {
//        var count = await _service.CountByAppUserIdAsync(appUserId);

//        return Ok(count);
//    }

//    [HttpGet("countByListOfUserId/{collectionUserId}")]
//    public async Task<IActionResult> CountByListOfAppUsers(string collectionUserId)
//    {
//        var appUserIdList = collectionUserId.Split(',');
//        var count = await _service.CountByListOfAppUserIdAsync(appUserIdList);

//        return Ok(count);
//    }

//    [HttpGet]
//    public async Task<IActionResult> GetAll()
//    {
//        var result = await _service.GetAllAsync();

//        return Ok(result);
//    }

//    [HttpGet("{id:int:min(1)}")]
//    public async Task<IActionResult> GetById(int id)
//    {
//        var result = await _service.GetByIdAsync(id);

//        return Ok(result);
//    }

//    [HttpGet("getByUserId")]
//    public async Task<IActionResult> GetByUserId(string appUserId, int pageSize)
//    {
//        var posts = await _service.GetByAppUserIdAsync(appUserId, pageSize);

//        return Ok(posts);
//    }

//    [HttpGet("getMoreByUserId")]
//    public async Task<IActionResult> GetMoreByUserId(string appUserId, int offset, int pageSize)
//    {
//        var posts = await _service.GetMoreByAppUserIdAsync(appUserId, offset, pageSize);

//        return Ok(posts);
//    }

//    [HttpGet("getNewPosts")]
//    public async Task<IActionResult> GetNewPosts(string appUserId, string checkFrom)
//    {
//        var checkFromData = DateTimeOffset.Parse(checkFrom);
//        var posts = await _service.GetNewByAppUserIdAsync(appUserId, checkFromData);

//        return Ok(posts);
//    }

//    [HttpGet("getByListOfUserId")]
//    public async Task<IActionResult> GetByListOfUserId(string collectionUserId, int pageSize)
//    {
//        var posts = await _service.GetByListOfAppUserIdAsync(collectionUserId, pageSize);

//        return Ok(posts);
//    }

//    [HttpGet("getMoreByListOfUserId")]
//    public async Task<IActionResult> GetMoreByListOfUserId(string collectionUserId, int offset, int pageSize)
//    {
//        var posts = await _service.GetMoreByListOfAppUserIdAsync(collectionUserId, offset, pageSize);

//        return Ok(posts);
//    }

//    [HttpGet("getNewByListOfUserId")]
//    public async Task<IActionResult> GetNewByListOfUserId(string collectionUserId, string checkFrom)
//    {
//        var checkFromData = DateTimeOffset.Parse(checkFrom);
//        var posts = await _service.GetNewByListOfAppUserIdAsync(collectionUserId, checkFromData);

//        return Ok(posts);
//    }

//    [HttpPost]
//    public async Task<IActionResult> Create([FromBody] UserPostModel userPost)
//    {
//        try
//        {
//            if (!ModelState.IsValid)
//            {
//                _logger.LogWarning("Invalid UserPost cretae request received: {@UserPost}", userPost);

//                return ValidationProblem(ModelState);
//            }

//            var map = _mapper.Map<UserPostDto>(userPost);
//            var result = await _service.CreateAsync(map);

//            return Ok(result);
//        }
//        catch (DbUpdateException ex)
//        {
//            _logger.LogError(ex, "Failed to create user post.");

//            return StatusCode(500, "Internal server error.");
//        }
//    }

//    [HttpPut("{id:int:min(1)}")]
//    public async Task<IActionResult> Update(int id, [FromBody] UserPostModel userPost)
//    {
//        try
//        {
//            if (!ModelState.IsValid)
//            {
//                _logger.LogWarning("Invalid UserPost update request received: {@UserPost}", userPost);

//                return ValidationProblem(ModelState);
//            }

//            if (id != userPost.Id)
//            {
//                return BadRequest("Route ID and body ID do not match.");
//            }

//            var map = _mapper.Map<UserPostDto>(userPost);
//            await _service.UpdateAsync(id, map);

//            return NoContent();
//        }
//        catch (DbUpdateException ex)
//        {
//            _logger.LogError(ex, "Failed to update user post.");

//            return StatusCode(500, "Internal server error.");
//        }
//    }

//    [HttpDelete("{id:int:min(1)}")]
//    public async Task<IActionResult> Delete(int id)
//    {
//        try
//        {
//            await _service.DeleteAsync(id);

//            return NoContent();
//        }
//        catch (DbUpdateConcurrencyException ex)
//        {
//            _logger.LogWarning(ex, "The resource was modified by another user. Please refresh and try again.");

//            return Conflict(new { message = "The resource was modified by another user. Please refresh and try again." });
//        }
//    }
//}