//using AutoMapper;
//using CombatAnalysis.CommunicationAPI.Models.Post;
//using CombatAnalysis.CommunicationBL.DTO.Post;
//using CombatAnalysis.CommunicationBL.Interfaces;
//using CombatAnalysis.CommunicationDAL.Entities.Post;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;

//namespace CombatAnalysis.CommunicationAPI.Controllers.Post;

//[Route("api/v1/[controller]")]
//[ApiController]
//[Authorize]
//public class UserPostLikeController(IService<UserPostLikeDto, int> service, IMapper mapper, ILogger<UserPostLikeController> logger) : ControllerBase
//{
//    private readonly IService<UserPostLikeDto, int> _service = service;
//    private readonly IMapper _mapper = mapper;
//    private readonly ILogger<UserPostLikeController> _logger = logger;

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

//    [HttpGet("findByPostId/{userPostId:int:min(1)}")]
//    public async Task<IActionResult> SearchByPostId(int userPostId)
//    {
//        var result = await _service.GetByParamAsync(c => c.UserPostId, userPostId);

//        return Ok(result);
//    }

//    [HttpPost]
//    public async Task<IActionResult> Create([FromBody] UserPostLikeModel userPostLike)
//    {
//        try
//        {
//            if (!ModelState.IsValid)
//            {
//                _logger.LogWarning("Invalid UserPostLike cretae request received: {@UserPostLike}", userPostLike);

//                return ValidationProblem(ModelState);
//            }

//            var map = _mapper.Map<UserPostLikeDto>(userPostLike);
//            var result = await _service.CreateAsync(map);

//            return Ok(result);
//        }
//        catch (DbUpdateException ex)
//        {
//            _logger.LogError(ex, "Failed to create user post like.");

//            return StatusCode(500, "Internal server error.");
//        }
//    }

//    [HttpPut("{id:int:min(1)}")]
//    public async Task<IActionResult> Update(int id, [FromBody] UserPostLikeModel userPostLike)
//    {
//        try
//        {
//            if (!ModelState.IsValid)
//            {
//                _logger.LogWarning("Invalid UserPostLike update request received: {@UserPostLike}", userPostLike);

//                return ValidationProblem(ModelState);
//            }

//            if (id != userPostLike.Id)
//            {
//                return BadRequest("Route ID and body ID do not match.");
//            }

//            var map = _mapper.Map<UserPostLikeDto>(userPostLike);
//            await _service.UpdateAsync(id, map);

//            return NoContent();
//        }
//        catch (DbUpdateException ex)
//        {
//            _logger.LogError(ex, "Failed to update user post like.");

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
