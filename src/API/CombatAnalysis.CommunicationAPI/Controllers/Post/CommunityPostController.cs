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
//public class CommunityPostController(ICommunityPostService service, IMapper mapper, ILogger<CommunityPostController> logger) : ControllerBase
//{
//    private readonly ICommunityPostService _service = service;
//    private readonly IMapper _mapper = mapper;
//    private readonly ILogger<CommunityPostController> _logger = logger;

//    [HttpGet("count/{communityId}")]
//    public async Task<IActionResult> Count(int communityId)
//    {
//        var count = await _service.CountByCommunityIdAsync(communityId);

//        return Ok(count);
//    }

//    [HttpGet("countByListOfCommunityId/{collectionCommunityId}")]
//    public async Task<IActionResult> CountByListOfAppUsers(string collectionCommunityId)
//    {
//        var collectionCommunityIdAsArray = collectionCommunityId.Split(',').Select(int.Parse).ToArray();
//        var count = await _service.CountByListOfCommunityIdAsync(collectionCommunityIdAsArray);

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

//    [HttpGet("getByCommunityId")]
//    public async Task<IActionResult> GetByCommunityId(int communityId, int pageSize)
//    {
//        var posts = await _service.GetByCommunityIdAsync(communityId, pageSize);

//        return Ok(posts);
//    }

//    [HttpGet("getMoreByCommunityId")]
//    public async Task<IActionResult> GetMoreByCommunityId(int communityId, int offset, int pageSize)
//    {
//        var posts = await _service.GetMoreByCommunityIdAsync(communityId, offset, pageSize);

//        return Ok(posts);
//    }

//    [HttpGet("getNewPosts")]
//    public async Task<IActionResult> GetNewPosts(int communityId, string checkFrom)
//    {
//        var checkFromData = DateTimeOffset.Parse(checkFrom);
//        var posts = await _service.GetNewByCommunityIdAsync(communityId, checkFromData);

//        return Ok(posts);
//    }

//    [HttpGet("getByListOfCommunityId")]
//    public async Task<IActionResult> GetByListOfCommunityId(string collectionCommunityId, int pageSize)
//    {
//        var posts = await _service.GetByListOfCommunityIdAsync(collectionCommunityId, pageSize);

//        return Ok(posts);
//    }

//    [HttpGet("getMoreByListOfCommunityId")]
//    public async Task<IActionResult> GetMoreByListOfCommunityId(string collectionCommunityId, int offset, int pageSize)
//    {
//        var posts = await _service.GetMoreByListOfCommunityIdAsync(collectionCommunityId, offset, pageSize);

//        return Ok(posts);
//    }

//    [HttpGet("getNewByListOfCommunityId")]
//    public async Task<IActionResult> GetNewByListOfCommunityId(string collectionCommunityId, string checkFrom)
//    {
//        var checkFromData = DateTimeOffset.Parse(checkFrom);
//        var posts = await _service.GetNewByListOfCommunityIdAsync(collectionCommunityId, checkFromData);

//        return Ok(posts);
//    }

//    [HttpPost]
//    public async Task<IActionResult> Create([FromBody] CommunityPostModel communityPost)
//    {
//        try
//        {
//            if (!ModelState.IsValid)
//            {
//                _logger.LogWarning("Invalid CommunityPost cretae request received: {@CommunityPost}", communityPost);

//                return ValidationProblem(ModelState);
//            }

//            var map = _mapper.Map<CommunityPostDto>(communityPost);
//            var result = await _service.CreateAsync(map);

//            return Ok(result);
//        }
//        catch (DbUpdateException ex)
//        {
//            _logger.LogError(ex, "Failed to create community post.");

//            return StatusCode(500, "Internal server error.");
//        }
//    }

//    [HttpPut("{id:int:min(1)}")]
//    public async Task<IActionResult> Update(int id, [FromBody] CommunityPostModel communityPost)
//    {
//        try
//        {
//            if (!ModelState.IsValid)
//            {
//                _logger.LogWarning("Invalid CommunityPost update request received: {@CommunityPost}", communityPost);

//                return ValidationProblem(ModelState);
//            }

//            if (id != communityPost.Id)
//            {
//                return BadRequest("Route ID and body ID do not match.");
//            }

//            var map = _mapper.Map<CommunityPostDto>(communityPost);
//            await _service.UpdateAsync(id, map);

//            return NoContent();
//        }
//        catch (DbUpdateException ex)
//        {
//            _logger.LogError(ex, "Failed to update community post.");

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
