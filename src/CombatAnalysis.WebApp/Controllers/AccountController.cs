using CombatAnalysis.WebApp.Consts;
using CombatAnalysis.WebApp.Interfaces;
using CombatAnalysis.WebApp.Models.User;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace CombatAnalysis.WebApp.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IHttpClientHelper _httpClient;

        public AccountController(IHttpClientHelper httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = Port.UserApi;
        }

        [HttpPost("registration")]
        public async Task Registration(RegisterModel registerModel)
        {
            var responseMessage = await _httpClient.PostAsync("account/registration", JsonContent.Create(registerModel));
            //var statusCode = responseMessage.StatusCode;
        }

        [HttpPost]
        public async Task Authorization(LoginModel loginModel)
        {
            var responseMessage = await _httpClient.PostAsync("account", JsonContent.Create(loginModel));
            //var statusCode = responseMessage.StatusCode;
        }
    }
}
