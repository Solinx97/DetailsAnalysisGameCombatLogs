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
    public class RegistrationViewModel : MvxViewModel
    {
        private readonly IMemoryCache _memoryCache;
        private readonly IHttpClientHelper _httpClient;
        private readonly IMvxNavigationService _mvvmNavigation;

        private IImprovedMvxViewModel _basicTemplate;
        private string _email;
        private string _password;
        private string _confirmPassword;
        private bool _confirmPasswordIsNotCorrect;
        private bool _inputDataIsEmpty;
        private bool _accountIsReady;
        private bool _serverIsNotAvailable;

        public RegistrationViewModel(IMemoryCache memoryCache, IHttpClientHelper httpClient, IMvxNavigationService mvvmNavigation)
        {
            _memoryCache = memoryCache;
            _httpClient = httpClient;
            _mvvmNavigation = mvvmNavigation;
            _httpClient.BaseAddress = Port.UserApi;

            RegistrationCommand = new MvxCommand(Validate);
            CancelCommand = new MvxCommand(Cancel);

            BasicTemplate = Templates.Basic;
            if (BasicTemplate.Parent is LoginViewModel)
            {
                Task.Run(() => _mvvmNavigation.Close(BasicTemplate.Parent));
            }

            BasicTemplate.Handler.PropertyUpdate<BasicTemplateViewModel>(BasicTemplate, "IsLoginNotActivated", true);
            BasicTemplate.Parent = this;
        }

        public IMvxCommand RegistrationCommand { get; set; }

        public IMvxCommand CancelCommand { get; set; }

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

        public string ConfirmPassword
        {
            get { return _confirmPassword; }
            set
            {
                SetProperty(ref _confirmPassword, value);
            }
        }

        public bool InputDataIsEmpty
        {
            get { return _inputDataIsEmpty; }
            set
            {
                SetProperty(ref _inputDataIsEmpty, value);
            }
        }

        public bool AccountIsReady
        {
            get { return _accountIsReady; }
            set
            {
                SetProperty(ref _accountIsReady, value);
            }
        }

        public bool ConfirmPasswordIsNotCorrect
        {
            get { return _confirmPasswordIsNotCorrect; }
            set
            {
                SetProperty(ref _confirmPasswordIsNotCorrect, value);
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

        public void Validate()
        {
            if (string.IsNullOrWhiteSpace(Email)
                || string.IsNullOrWhiteSpace(Password)
                || string.IsNullOrWhiteSpace(ConfirmPassword))
            {
                InputDataIsEmpty = true;
            }
            else
            {
                InputDataIsEmpty = false;
                CheckConfirmPassword();
            }
        }

        public void CheckConfirmPassword()
        {
            if (Password.Equals(ConfirmPassword))
            {
                ConfirmPasswordIsNotCorrect = false;
                Registration();
            }
            else
            {
                ConfirmPasswordIsNotCorrect = true;
            }
        }

        public void Registration()
        {
            ServerIsNotAvailable = false;
            AccountIsReady = false;
            ConfirmPasswordIsNotCorrect = false;
            InputDataIsEmpty = false;

            Task.Run(async () => await RegistrationAsync());
        }

        public void Cancel()
        {
            BasicTemplate.Handler.PropertyUpdate<BasicTemplateViewModel>(BasicTemplate, "IsRegistrationNotActivated", true);
            Task.Run(() => _mvvmNavigation.Close(this));
        }

        private async Task RegistrationAsync()
        {
            try
            {
                var registrationModel = new RegistrationModel { Id = Guid.NewGuid().ToString(), Email = Email, Password = Password };
                var responseMessage = await _httpClient.PostAsync("Account/registration", JsonContent.Create(registrationModel));
                if (responseMessage.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var result = await responseMessage.Content.ReadFromJsonAsync<ResponseFromAccount>();

                    _memoryCache.Set("accessToken", result.AccessToken, new MemoryCacheEntryOptions { Size = 10 });
                    _memoryCache.Set("refreshToken", result.RefreshToken, new MemoryCacheEntryOptions { Size = 10 });
                    _memoryCache.Set("user", result.User, new MemoryCacheEntryOptions { Size = 50 });

                    BasicTemplate.Handler.PropertyUpdate<BasicTemplateViewModel>(BasicTemplate, "IsAuth", true);
                    BasicTemplate.Handler.PropertyUpdate<BasicTemplateViewModel>(BasicTemplate, "Email", result.User.Email);

                    BasicTemplate.Handler.PropertyUpdate<BasicTemplateViewModel>(BasicTemplate, "IsRegistrationNotActivated", true);
                    if (BasicTemplate.Parent is LoginViewModel)
                    {
                        await _mvvmNavigation.Close(BasicTemplate.Parent);
                    }

                    await _mvvmNavigation.Close(this);
                }
                else
                {
                    AccountIsReady = true;
                }
            }
            catch (HttpRequestException)
            {
                ServerIsNotAvailable = true;
            }
        }
    }
}
