using CombatAnalysis.Core.Consts;
using CombatAnalysis.Core.Interfaces;
using CombatAnalysis.Core.Models.Response;
using CombatAnalysis.Core.Models.User;
using Microsoft.Extensions.Caching.Memory;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace CombatAnalysis.Core.ViewModels
{
    public class LoginViewModel : MvxViewModel
    {
        private readonly IMemoryCache _memoryCache;
        private readonly IHttpClientHelper _httpClient;
        private readonly IMvxNavigationService _mvvmNavigation;

        private IImprovedMvxViewModel _basicTemplate;
        private string _email;
        private string _password;
        private bool _authIsFailed;
        private bool _serverIsNotAvailable;

        public LoginViewModel(IMemoryCache memoryCache, IHttpClientHelper httpClient, IMvxNavigationService mvvmNavigation)
        {
            _memoryCache = memoryCache;
            _httpClient = httpClient;
            _mvvmNavigation = mvvmNavigation;
            _httpClient.BaseAddress = Port.UserApi;

            LoginCommand = new MvxCommand(Login);
            CancelCommand = new MvxCommand(Cancel);

            BasicTemplate = Templates.Basic;
        }

        public IImprovedMvxViewModel BasicTemplate
        {
            get { return _basicTemplate; }
            set
            {
                SetProperty(ref _basicTemplate, value);
            }
        }

        public string Email
        {
            get { return _email; }
            set
            {
                SetProperty(ref _email, value);
            }
        }

        public string Password
        {
            get { return _password; }
            set
            {
                SetProperty(ref _password, value);
            }
        }

        public bool AuthIsFailed
        {
            get { return _authIsFailed; }
            set
            {
                SetProperty(ref _authIsFailed, value);
            }
        }

        public bool ServerIsNotAvailable
        {
            get { return _serverIsNotAvailable; }
            set
            {
                SetProperty(ref _serverIsNotAvailable, value);
            }
        }

        public IMvxCommand LoginCommand { get; set; }

        public IMvxCommand CancelCommand { get; set; }

        public void Login()
        {
            AuthIsFailed = false;
            ServerIsNotAvailable = false;

            var loginModel = new LoginModel { Email = Email, Password = Password };

            Action action = async () =>
            {
                try
                {
                    var responseMessage = await _httpClient.PostAsync("Account", JsonContent.Create(loginModel));
                    if (responseMessage.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        var result = await responseMessage.Content.ReadFromJsonAsync<ResponseFromAccount>();

                        _memoryCache.Set("accessToken", result.AccessToken, new MemoryCacheEntryOptions { Size = 10 });
                        _memoryCache.Set("refreshToken", result.RefreshToken, new MemoryCacheEntryOptions { Size = 10 });
                        _memoryCache.Set("user", result.User, new MemoryCacheEntryOptions { Size = 50 });

                        BasicTemplate.Handler.PropertyUpdate<BasicTemplateViewModel>(BasicTemplate, "IsAuth", true);
                        BasicTemplate.Handler.PropertyUpdate<BasicTemplateViewModel>(BasicTemplate, "Email", result.User.Email);

                        await _mvvmNavigation.Close(this);
                    }
                    else
                    {
                        AuthIsFailed = true;
                    }
                }
                catch (HttpRequestException)
                {
                    ServerIsNotAvailable = true;
                }

            };

            Task.Run(action);
        }

        public void Cancel()
        {
            Task.Run(() => _mvvmNavigation.Close(this));
        }
    }
}
