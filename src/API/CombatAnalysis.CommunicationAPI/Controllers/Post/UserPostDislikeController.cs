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
//public class UserPostDislikeController(IService<UserPostDislikeDto, int> service, IMapper mapper, ILogger<UserPostDislikeController> logger) : ControllerBase
//{
//    private readonly IService<UserPostDislikeDto, int> _service = service;
//    private readonly IMapper _mapper = mapper;
//    private readonly ILogger<UserPostDislikeController> _logger = logger;

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
//    public async Task<IActionResult> Create([FromBody] UserPostDislikeModel userPostDislike)
//    {
//        try
//        {
//            if (!ModelState.IsValid)
//            {
//                _logger.LogWarning("Invalid UserPostDislike cretae request received: {@UserPostDislike}", userPostDislike);

//                return ValidationProblem(ModelState);
//            }

//            var map = _mapper.Map<UserPostDislikeDto>(userPostDislike);
//            var result = await _service.CreateAsync(map);

//            return Ok(result);
//        }
//        catch (DbUpdateException ex)
//        {
//            _logger.LogError(ex, "Failed to create user post dislike.");

//            return StatusCode(500, "Internal server error.");
//        }
//    }

//    [HttpPut("{id:int:min(1)}")]
//    public async Task<IActionResult> Update(int id, [FromBody] UserPostDislikeModel userPostDislike)
//    {
//        try
//        {
//            if (!ModelState.IsValid)
//            {
//                _logger.LogWarning("Invalid UserPostDislike update request received: {@UserPostDislike}", userPostDislike);

//                return ValidationProblem(ModelState);
//            }

//            if (id != userPostDislike.Id)
//            {
//                return BadRequest("Route ID and body ID do not match.");
//            }

//            var map = _mapper.Map<UserPostDislikeDto>(userPostDislike);
//            await _service.UpdateAsync(id, map);

//            return NoContent();
//        }
//        catch (DbUpdateException ex)
//        {
//            _logger.LogError(ex, "Failed to update user post dislike.");

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
