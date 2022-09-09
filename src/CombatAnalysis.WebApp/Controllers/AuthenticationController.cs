using CombatAnalysis.WebApp.Consts;
using CombatAnalysis.WebApp.Interfaces;
using CombatAnalysis.WebApp.Models.Response;
using CombatAnalysis.WebApp.Models.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace CombatAnalysis.WebApp.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IHttpClientHelper _httpClient;

        public AuthenticationController(IHttpClientHelper httpClient)
        {
            _httpClient = httpClient;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> Refresh()
        {
            if (!HttpContext.Request.Cookies.TryGetValue("refreshToken", out var refreshToken))
            {
                return Unauthorized();
            }

            _httpClient.BaseAddress = Port.UserApi;
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

            var responseMessage = await _httpClient.GetAsync($"account/find/{email}");
            if (responseMessage.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var user = await responseMessage.Content.ReadFromJsonAsync<UserModel>();
                if (user == null)
                {
                    await _httpClient.GetAsync($"authentication/check/{user.Id}");
                }

                return await CheckAccessToken(user);
            }


            return BadRequest();
        }

        private async Task<IActionResult> CheckAccessToken(UserModel user)
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
