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
        private readonly IHttpClientHelper _httpClientHelper;
        private readonly IMvxNavigationService _mvvmNavigation;

        private IImprovedMvxViewModel _basicTemplate;
        private string _email;
        private string _password;
        private bool _authIsFailed;
        private bool _serverIsNotAvailable;

        public LoginViewModel(IMemoryCache memoryCache, IHttpClientHelper httpClient, IMvxNavigationService mvvmNavigation)
        {
            _memoryCache = memoryCache;
            _httpClientHelper = httpClient;
            _mvvmNavigation = mvvmNavigation;

            LoginCommand = new MvxCommand(Login);
            CancelCommand = new MvxAsyncCommand(CancelAsync);

            BasicTemplate = Templates.Basic;
            if (BasicTemplate.Parent is RegistrationViewModel)
            {
                _mvvmNavigation.Close(BasicTemplate.Parent).GetAwaiter().GetResult();
            }

            BasicTemplate.Handler.PropertyUpdate<BasicTemplateViewModel>(BasicTemplate, "IsRegistrationNotActivated", true);
            BasicTemplate.Parent = this;
        }

        #region Commands

        public IMvxCommand LoginCommand { get; set; }

        public IMvxAsyncCommand CancelCommand { get; set; }

        #endregion

        #region Properties

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

        #endregion

        public void Login()
        {
            AuthIsFailed = false;
            ServerIsNotAvailable = false;

            var loginModel = new LoginModel { Email = Email, Password = Password };

            Action action = async () =>
            {
                try
                {
                    _httpClientHelper.BaseAddress = Port.UserApi;
                    var responseMessage = await _httpClientHelper.PostAsync("Account", JsonContent.Create(loginModel));
                    if (responseMessage.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        var response = await responseMessage.Content.ReadFromJsonAsync<ResponseFromAccount>();

                        SetMemoryCache(response);

                        BasicTemplate.Handler.PropertyUpdate<BasicTemplateViewModel>(BasicTemplate, "IsAuth", true);
                        BasicTemplate.Handler.PropertyUpdate<BasicTemplateViewModel>(BasicTemplate, "Email", response.User.Email);

                        BasicTemplate.Handler.PropertyUpdate<BasicTemplateViewModel>(BasicTemplate, "IsLoginNotActivated", true);

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

        public async Task CancelAsync()
        {
            BasicTemplate.Handler.PropertyUpdate<BasicTemplateViewModel>(BasicTemplate, "IsLoginNotActivated", true);
            BasicTemplate.Parent = null;
            await _mvvmNavigation.Close(this);
        }

        private void SetMemoryCache(ResponseFromAccount response)
        {
            _memoryCache.Set("accessToken", response.AccessToken, new MemoryCacheEntryOptions { Size = 10 });
            _memoryCache.Set("refreshToken", response.RefreshToken, new MemoryCacheEntryOptions { Size = 10 });
            _memoryCache.Set("account", response.User, new MemoryCacheEntryOptions { Size = 50 });
        }
    }
}
