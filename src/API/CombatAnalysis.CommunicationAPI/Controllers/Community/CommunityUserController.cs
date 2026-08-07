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
//public class CommunityUserController(IService<CommunityUserDto, string> service, IMapper mapper, ILogger<CommunityUserController> logger) : ControllerBase
//{
//    private readonly IService<CommunityUserDto, string> _service = service;
//    private readonly IMapper _mapper = mapper;
//    private readonly ILogger<CommunityUserController> _logger = logger;

//    [HttpGet]
//    public async Task<IActionResult> GetAll()
//    {
//        var result = await _service.GetAllAsync();

//        return Ok(result);
//    }

//    [HttpGet("{id}")]
//    public async Task<IActionResult> GetById(string id)
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

//    [HttpGet("findByUserId/{userId}")]
//    public async Task<IActionResult> FindByUserId(string userId)
//    {
//        var result = await _service.GetByParamAsync(c => c.AppUserId, userId);

//        return Ok(result);
//    }

//    [HttpPost]
//    public async Task<IActionResult> Create([FromBody] CommunityUserModel communityUser)
//    {
//        try
//        {
//            if (!ModelState.IsValid)
//            {
//                _logger.LogWarning("Invalid CommunityUser create request received: {@CommunityUser}", communityUser);

//                return ValidationProblem(ModelState);
//            }

//            var map = _mapper.Map<CommunityUserDto>(communityUser);
//            var result = await _service.CreateAsync(map);

//            return Ok(result);
//        }
//        catch (DbUpdateException ex)
//        {
//            _logger.LogError(ex, "Failed to create community user.");

//            return StatusCode(500, "Internal server error.");
//        }
//    }

//    [HttpPut("{id}")]
//    public async Task<IActionResult> Update(string id, [FromBody] CommunityUserModel communityUser)
//    {
//        try
//        {
//            if (!ModelState.IsValid)
//            {
//                _logger.LogWarning("Invalid CommunityUser update request received: {@CommunityUser}", communityUser);

//                return ValidationProblem(ModelState);
//            }

//            if (id != communityUser.Id)
//            {
//                return BadRequest("Route ID and body ID do not match.");
//            }

//            var map = _mapper.Map<CommunityUserDto>(communityUser);
//            await _service.UpdateAsync(id, map);

//            return NoContent();
//        }
//        catch (DbUpdateException ex)
//        {
//            _logger.LogError(ex, "Failed to update community user.");

//            return StatusCode(500, "Internal server error.");
//        }
//    }

//    [HttpDelete("{id}")]
//    public async Task<IActionResult> Delete(string id)
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
