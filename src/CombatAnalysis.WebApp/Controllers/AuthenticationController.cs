using CombatAnalysis.DAL.Data;
using CombatAnalysis.DAL.Entities.User;
using CombatAnalysis.WebApp.Consts;
using CombatAnalysis.WebApp.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace CombatAnalysis.WebApp.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly CombatAnalysisContext _dbContext;
        private readonly IHttpClientHelper _httpClient;

        public AuthenticationController(CombatAnalysisContext dbContext, IHttpClientHelper httpClient)
        {
            _dbContext = dbContext;
            _httpClient = httpClient;
            _httpClient.BaseAddress = Port.UserApi;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> Refresh()
        {
            if (!HttpContext.Request.Cookies.TryGetValue("refreshToken", out var refreshToken))
            {
                return Unauthorized();
            }

            var response = await _httpClient.GetAsync($"authentication/validateRefreshToken/{refreshToken}");
            if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                Response.Cookies.Delete("accessToken");
                Response.Cookies.Delete("refreshToken");

                return Unauthorized();
            }

            response = await _httpClient.GetAsync($"authentication/find/{refreshToken}");
            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                Response.Cookies.Delete("accessToken");
                Response.Cookies.Delete("refreshToken");

                return Unauthorized();
            }

            var email = HttpContext.User.Identity.Name;
            var user = await _dbContext.User.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null)
            {
                await _httpClient.GetAsync($"authentication/check/{user.Id}");
            }

            return await CheckAccessToken(user);
        }

        private async Task<IActionResult> CheckAccessToken(User user)
        {
            if (!HttpContext.Request.Cookies.TryGetValue("accessToken", out var accessToken))
            {
                Response.Cookies.Delete("refreshToken");

                return Unauthorized();
            }

            var response = await _httpClient.GetAsync($"authentication/validateAccessToken/{accessToken}");
            if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                Response.Cookies.Delete("accessToken");
                Response.Cookies.Delete("refreshToken");

                return Unauthorized();
            }

            return Ok(user);
        }
    }
}
