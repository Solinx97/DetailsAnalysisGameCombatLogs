using AutoMapper;
using CombatAnalysis.CustomerBL.DTO;
using CombatAnalysis.CustomerBL.Interfaces;
using CombatAnalysis.Identity.Interfaces;
using CombatAnalysis.UserApi.Models;
using CombatAnalysis.UserApi.Models.Response;
using Microsoft.AspNetCore.Authorization;
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
    [AllowAnonymous]
    public async Task<IActionResult> Login(LoginModel model)
    {
        var user = await _service.GetAsync(model.Email, model.Password);
        if (user == null)
        {
            return NotFound();
        }

        var refreshToken = await _tokenService.GenerateTokensAsync(user.Id);
        var map = _mapper.Map<AppUserModel>(user);
        var response = new ResponseFromAccount(map, refreshToken);

        return Ok(response);
    }

    [HttpPost("registration")]
    [AllowAnonymous]
    public async Task<IActionResult> Register(RegisterModel model)
    {
        try
        {
            var user = await _service.GetAsync(model.Email);
            if (user != null)
            {
                return Ok();
            }

            var newUser = new AppUserModel { Id = Guid.NewGuid().ToString(), Email = model.Email, Password = model.Password, PhoneNumber = string.Empty, Birthday = DateTimeOffset.Now };
            var map = _mapper.Map<AppUserDto>(newUser);
            await _service.CreateAsync(map);

            var refreshToken = await _tokenService.GenerateTokensAsync(newUser.Id);
            var response = new ResponseFromAccount(newUser, refreshToken);

            return Ok(response);
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, ex.Message);

            return BadRequest();
        }
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var users = await _service.GetAllAsync();
            if (!users.Any())
            {
                return NotFound();
            }

            return Ok(users);
        }
        catch (Exception)
        {
            return BadRequest();
        }
    }

    [HttpGet("{id}")]
    [Authorize]
    public async Task<IActionResult> GetById(string id)
    {
        try
        {
            var user = await _service.GetByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }
        catch (Exception)
        {
            return BadRequest();
        }
    }

    [HttpPut]
    [Authorize]
    public async Task<IActionResult> Edit(AppUserModel user)
    {
        var map = _mapper.Map<AppUserDto>(user);
        var updatedUser = await _service.UpdateAsync(map);
        if (updatedUser == 0)
        {
            return BadRequest();
        }

        var login = new LoginModel { Email = user.Email, Password = user.Password };
        return await Login(login);
    }

    [HttpGet("find/{email}")]
    [Authorize]
    public async Task<IActionResult> Find(string email)
    {
        try
        {
            var user = await _service.GetAsync(email);
            if (user == null)
            {
                return NotFound();
                
            }

            return Ok(user);
        }
        catch (Exception)
        {
            return BadRequest();
        }
    }
}
