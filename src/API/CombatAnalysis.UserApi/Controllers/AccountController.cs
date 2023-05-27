using AutoMapper;
using CombatAnalysis.CustomerBL.DTO;
using CombatAnalysis.CustomerBL.Interfaces;
using CombatAnalysis.Identity.Interfaces;
using CombatAnalysis.UserApi.Models;
using CombatAnalysis.UserApi.Models.Response;
using Microsoft.AspNetCore.Mvc;

namespace CombatAnalysis.UserApi.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class AccountController : ControllerBase
{
    private readonly IUserService<AppUserDto> _service;
    private readonly IIdentityTokenService _tokenService;
    private readonly IMapper _mapper;
    private readonly ILogger _logger;

    public AccountController(IUserService<AppUserDto> service, IIdentityTokenService tokenService, IMapper mapper, ILogger logger)
    {
        _service = service;
        _tokenService = tokenService;
        _mapper = mapper;
        _logger = logger;
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginModel model)
    {
        var user = await _service.GetAsync(model.Email, model.Password);
        if (user == null)
        {
            return BadRequest();
        }

        var tokens = await _tokenService.GenerateTokensAsync(HttpContext.Response.Cookies, user.Id);
        var map = _mapper.Map<AppUserModel>(user);
        var response = new ResponseFromAccount(map, tokens.Item1, tokens.Item2);

        return Ok(response);
    }

    [HttpPost("registration")]
    public async Task<IActionResult> Register(RegisterModel model)
    {
        try
        {
            var user = await _service.GetAsync(model.Email);
            if (user != null)
            {
                return BadRequest();
            }

            var newUser = new AppUserModel { Id = Guid.NewGuid().ToString(), Email = model.Email, Password = model.Password };
            var map = _mapper.Map<AppUserDto>(newUser);
            await _service.CreateAsync(map);

            var tokens = await _tokenService.GenerateTokensAsync(HttpContext.Response.Cookies, newUser.Id);
            var response = new ResponseFromAccount(newUser, tokens.Item1, tokens.Item2);

            return Ok(response);
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, ex.Message);

            return BadRequest();
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var users = await _service.GetAllAsync();
        if (users.Any())
        {
            return Ok(users);
        }
        else
        {
            return BadRequest();
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(string id)
    {
        var user = await _service.GetByIdAsync(id);
        if (user != null)
        {
            return Ok(user);
        }
        else
        {
            return BadRequest();
        }
    }

    [HttpGet("logout/{refreshToken}")]
    public async Task<IActionResult> Logout(string refreshToken)
    {
        var refreshTokenModel = await _tokenService.FindRefreshTokenAsync(refreshToken);
        await _tokenService.RemoveRefreshTokenAsync(refreshTokenModel);

        return Ok();
    }

    [HttpGet("find/{email}")]
    public async Task<IActionResult> Find(string email)
    {
        var user = await _service.GetAsync(email);
        if (user != null)
        {
            return Ok(user);
        }
        else
        {
            return BadRequest();
        }
    }
}
