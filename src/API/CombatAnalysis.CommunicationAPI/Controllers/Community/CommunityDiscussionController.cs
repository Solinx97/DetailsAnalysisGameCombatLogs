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
//public class CommunityDiscussionController(IService<CommunityDiscussionDto, int> service, IMapper mapper, ILogger<CommunityDiscussionController> logger) : ControllerBase
//{
//    private readonly IService<CommunityDiscussionDto, int> _service = service;
//    private readonly IMapper _mapper = mapper;
//    private readonly ILogger<CommunityDiscussionController> _logger = logger;

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

//    [HttpGet("findByCommunityId/{communityId:int:min(1)}")]
//    public async Task<IActionResult> FindByCommunityId(int communityId)
//    {
//        var result = await _service.GetByParamAsync(c => c.CommunityId, communityId);

//        return Ok(result);
//    }

//    [HttpPost]
//    public async Task<IActionResult> Create([FromBody] CommunityDiscussionModel communityDiscussion)
//    {
//        try
//        {
//            if (!ModelState.IsValid)
//            {
//                _logger.LogWarning("Invalid CommunityDiscussion create request received: {@CommunityDiscussion}", communityDiscussion);

//                return ValidationProblem(ModelState);
//            }

//            var map = _mapper.Map<CommunityDiscussionDto>(communityDiscussion);
//            var result = await _service.CreateAsync(map);

//            return Ok(result);
//        }
//        catch (DbUpdateException ex)
//        {
//            _logger.LogError(ex, "Failed to create community discussion.");

//            return StatusCode(500, "Internal server error.");
//        }
//    }

//    [HttpPut("{id:int:min(1)}")]
//    public async Task<IActionResult> Update(int id, [FromBody] CommunityDiscussionModel communityDiscussion)
//    {
//        try
//        {
//            if (!ModelState.IsValid)
//            {
//                _logger.LogWarning("Invalid CommunityDiscussion update request received: {@CommunityDiscussion}", communityDiscussion);

//                return ValidationProblem(ModelState);
//            }

//            if (id != communityDiscussion.Id)
//            {
//                return BadRequest("Route ID and body ID do not match.");
//            }

//            var map = _mapper.Map<CommunityDiscussionDto>(communityDiscussion);
//            await _service.UpdateAsync(id, map);

//            return NoContent();
//        }
//        catch (DbUpdateException ex)
//        {
//            _logger.LogError(ex, "Failed to update community discussion.");

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
