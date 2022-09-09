using AutoMapper;
using CombatAnalysis.BL.DTO.User;
using CombatAnalysis.BL.Interfaces;
using CombatAnalysis.Identity.Interfaces;
using CombatAnalysis.UserApi.Models;
using CombatAnalysis.UserApi.Models.Response;
using CombatAnalysis.UserApi.Models.User;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace CombatAnalysis.UserApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IUserService<UserDto> _service;
        private readonly IIdentityTokenService _tokenService;
        private readonly IMapper _mapper;

        public AccountController(IUserService<UserDto> service, IIdentityTokenService tokenService, IMapper mapper)
        {
            _service = service;
            _tokenService = tokenService;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model)
        {
            var user = await _service.GetAsync(model.Email, model.Password);
            if (user != null)
            {
                var tokens = await _tokenService.GenerateTokensAsync(HttpContext.Response.Cookies, user.Id);
                var map = _mapper.Map<UserModel>(user);
                var response = new ResponseFromAccount(map, tokens.Item1, tokens.Item2);

                return Ok(response);
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPost("registration")]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            var user = await _service.GetAsync(model.Email);
            if (user == null)
            {
                var newUser = new UserModel { Id = Guid.NewGuid().ToString(), Email = model.Email, Password = model.Password };
                var map = _mapper.Map<UserDto>(newUser);
                await _service.CreateAsync(map);

                var tokens = await _tokenService.GenerateTokensAsync(HttpContext.Response.Cookies, newUser.Id);
                var response = new ResponseFromAccount(newUser, tokens.Item1, tokens.Item2);

                return Ok(response);
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
}
