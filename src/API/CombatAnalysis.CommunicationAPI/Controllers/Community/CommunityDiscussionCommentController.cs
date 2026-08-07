//using AutoMapper;
//using CombatAnalysis.CommunicationAPI.Models.Community;
//using CombatAnalysis.CommunicationBL.DTO.Community;
//using CombatAnalysis.CommunicationBL.Interfaces;
//using CombatAnalysis.CommunicationDAL.Entities.Community;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;

//namespace CombatAnalysis.CommunicationAPI.Controllers.Community;

//[Route("api/v1/[controller]")]
//[ApiController]
//[Authorize]
//public class CommunityDiscussionCommentController(IService<CommunityDiscussionCommentDto, int> service, IMapper mapper, ILogger<CommunityDiscussionCommentController> logger) : ControllerBase
//{
//    private readonly IService<CommunityDiscussionCommentDto, int> _service = service;
//    private readonly IMapper _mapper = mapper;
//    private readonly ILogger<CommunityDiscussionCommentController> _logger = logger;

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

//    [HttpGet("findByDiscussionId/{id:int:min(1)}")]
//    public async Task<IActionResult> FindByDiscussionId(int id)
//    {
//        var result = await _service.GetByParamAsync(c => c.CommunityDiscussionId, id);

//        return Ok(result);
//    }

//    [HttpPost]
//    public async Task<IActionResult> Create([FromBody] CommunityDiscussionCommentModel communityDiscussionComment)
//    {
//        try
//        {
//            if (!ModelState.IsValid)
//            {
//                _logger.LogWarning("Invalid CommunityDiscussionComment create request received: {@CommunityDiscussionComment}", communityDiscussionComment);

//                return ValidationProblem(ModelState);
//            }

//            var map = _mapper.Map<CommunityDiscussionCommentDto>(communityDiscussionComment);
//            var result = await _service.CreateAsync(map);

//            return Ok(result);
//        }
//        catch (DbUpdateException ex)
//        {
//            _logger.LogError(ex, "Failed to create community discussion comment.");

//            return StatusCode(500, "Internal server error.");
//        }
//    }

//    [HttpPut("{id:int:min(1)}")]
//    public async Task<IActionResult> Update(int id, [FromBody] CommunityDiscussionCommentModel communityDiscussionComment)
//    {
//        try
//        {
//            if (!ModelState.IsValid)
//            {
//                _logger.LogWarning("Invalid CommunityDiscussionComment update request received: {@CommunityDiscussionComment}", communityDiscussionComment);

//                return ValidationProblem(ModelState);
//            }

//            if (id != communityDiscussionComment.Id)
//            {
//                return BadRequest("Route ID and body ID do not match.");
//            }

//            var map = _mapper.Map<CommunityDiscussionCommentDto>(communityDiscussionComment);
//            await _service.UpdateAsync(id, map);

//            return NoContent();
//        }
//        catch (DbUpdateException ex)
//        {
//            _logger.LogError(ex, "Failed to update community discussion comment.");

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
