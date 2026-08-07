//using AutoMapper;
//using CombatAnalysis.CommunicationAPI.Models.Community;
//using CombatAnalysis.CommunicationBL.DTO.Community;
//using CombatAnalysis.CommunicationBL.Interfaces;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;

//namespace CombatAnalysis.CommunicationAPI.Controllers.Community;

//[Route("api/v1/[controller]")]
//[ApiController]
//[Authorize]
//public class InviteToCommunityController(IService<InviteToCommunityDto, int> service, IMapper mapper, ILogger<InviteToCommunityController> logger) : ControllerBase
//{
//    private readonly IService<InviteToCommunityDto, int> _service = service;
//    private readonly IMapper _mapper = mapper;
//    private readonly ILogger<InviteToCommunityController> _logger = logger;

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

//    [HttpGet("findByUserId/{userId}")]
//    public async Task<IActionResult> SearchByUserId(string userId)
//    {
//        var result = await _service.GetByParamAsync(c => c.ToAppUserId, userId);

//        return Ok(result);
//    }

//    [HttpPost]
//    public async Task<IActionResult> Create([FromBody] InviteToCommunityModel inviteToCommunity)
//    {
//        try
//        {
//            if (!ModelState.IsValid)
//            {
//                _logger.LogWarning("Invalid InviteToCommunity create request received: {@InviteToCommunity}", inviteToCommunity);

//                return ValidationProblem(ModelState);
//            }

//            var map = _mapper.Map<InviteToCommunityDto>(inviteToCommunity);
//            var result = await _service.CreateAsync(map);

//            return Ok(result);
//        }
//        catch (DbUpdateException ex)
//        {
//            _logger.LogError(ex, "Failed to create invite to community.");

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
